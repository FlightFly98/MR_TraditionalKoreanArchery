using UnityEngine;

public class KkagjHand : ArduinoBase
{
    private float fsr;
    private int rssi;
    private float previousTime; // 적분 할 때 필요
    private Vector3 acc_raw;
    private Vector3 gyro_raw;

    private Vector3 acc_cor;
    private Vector3 gyro_cor;
    private float[] gyro_offset = {7, 0.9f,  0.3f};

    private int[] acc_offset = {-700, -500, -17650};

    private float pitch, roll, yaw;
    

    // 보정 계수
    private const float alpha = 0.98f;
    private const float gyroSensitivity = 16.4f;

    public float pressureThresholdHigh = 1000.0f;
    public float pressureThresholdLow = 500.0f;

    public bool checkValsi = false;

    // 환경 상수
    private float RSSI0 = -30f; // 새로운 기준 거리에서의 RSSI 값 (예: 0.1미터에서 측정한 값)
    private float n = 1.5f;     // 경로 손실 지수 (근거리 환경에 맞게 조정)
    private float d0 = 0.1f;    // 기준 거리 (0.1미터)

    // 필터 객체
    private MovingAverageFilter movingAverageFilter;
    private LowPassFilter lowPassFilter;
    private KalmanFilter kalmanFilter;

    // 필터 설정값
    private int movingAverageWindowSize = 5; // 근거리에서는 빠른 반응을 위해 작은 값 사용
    private float lowPassAlpha = 0.2f;       // 알파 값 조정
    private float kalmanProcessNoise = 0.01f;
    private float kalmanMeasurementNoise = 0.1f;

    protected override void Start()
    {
        port = 12347;
        base.Start();

        // 필터 객체 초기화
        movingAverageFilter = new MovingAverageFilter(movingAverageWindowSize);
        lowPassFilter = new LowPassFilter(lowPassAlpha);
        kalmanFilter = new KalmanFilter(kalmanProcessNoise, kalmanMeasurementNoise);
    }

    protected override void ProcessData(string data)
    {
        // 데이터 포맷: "Rssi: %d, FSR: %d, Accel: %d, %d, %d, Gyro: %d, %d, %d"
        string[] parts = data.Split(',');
        float ax, ay, az, gx, gy, gz;
        if (parts.Length == 8)
        {
            int.TryParse(parts[0].Split(':')[1].Trim(), out rssi);
            float.TryParse(parts[1].Split(':')[1].Trim(), out fsr);

            float.TryParse(parts[2].Split(':')[1].Trim(), out ax);
            float.TryParse(parts[3].Trim(), out ay);
            float.TryParse(parts[4].Trim(), out az);

            float.TryParse(parts[5].Split(':')[1].Trim(), out gx);
            float.TryParse(parts[6].Trim(), out gy);
            float.TryParse(parts[7].Trim(), out gz);

            acc_raw = new Vector3(ax, ay, az);
            gyro_raw = new Vector3(gx, gy, gz);
            
            for(int i = 0; i < 3; i++) 
            {
                acc_cor[i] = acc_raw[i] + acc_offset[i];
                gyro_cor[i] = (gyro_raw[i] / gyroSensitivity) + gyro_offset[i];
            }
            Debug.Log("Rssi: " + rssi);
            Debug.Log("FSR: " + fsr);
            Debug.Log("Acc: x " + acc_cor[0]  + " y " + acc_cor[1] + " z " + acc_cor[2]);
            Debug.Log("Gyro: x " + gyro_cor[0] + " y " + gyro_cor[1] + " z " + gyro_cor[2]);
        }
    }

    void FsrDetected()
    {
        if(fsr > pressureThresholdHigh)
        {
            GameManager.instance.isPressureHigh = true;
            GameManager.instance.isImpactDetected = false;  // 충격 감지 초기화
            checkValsi = true;
        }

        if((fsr < pressureThresholdLow) && checkValsi)
        {
            GameManager.instance.startTime = Time.time;
            //Debug.Log("KkagiHandTime: " + GameManager.instance.startTime);
            GameManager.instance.isPressureHigh = false;
            GameManager.instance.SetlaunchAngle();
            GameManager.instance.SetInitialVelocityRssi();
            Player.instance.valSi = true;
            checkValsi = false;
        }
    }
    void AngleCal()
    {
        // 시간 계산
        float currentTime = Time.time;
        float deltaTime = currentTime - previousTime;
        previousTime = currentTime;

        // 자이로스코프 데이터를 이용한 각도 변화 계산
        float gyroPitch = gyro_cor[0] * Time.fixedDeltaTime;
        float gyroRoll = gyro_cor[1] * Time.fixedDeltaTime;
        float gyroYaw = gyro_cor[2] * Time.fixedDeltaTime;

        // 가속도계를 이용한 각도 계산
        float vecPitch = Mathf.Sqrt(Mathf.Pow(acc_cor[0], 2) + Mathf.Pow(acc_cor[2], 2));
        float accelPitch = Mathf.Atan2(acc_cor[1], vecPitch) * Mathf.Rad2Deg;
        
        float vecRoll = Mathf.Sqrt(Mathf.Pow(acc_cor[1], 2) + Mathf.Pow(acc_cor[2], 2));
        float accelRoll = Mathf.Atan2(acc_cor[0], vecRoll) * Mathf.Rad2Deg;
        
        // 저역 필터
        pitch = alpha * (pitch + gyroPitch) + (1.0f - alpha) * accelPitch;
        roll = alpha * (roll + gyroRoll) + (1.0f - alpha) * accelRoll;

       // Debug.Log("pitch: " + pitch + " roll: " + roll + " yaw: " + yaw);
        
        //GameManager.instance.SetkkagjHandYaw(yaw);
    }

    void rssiDistanceCal()
    {
        // 1. 이동 평균 필터 적용
        float filteredRSSI = movingAverageFilter.AddValue(rssi);

        // 2. 저역 통과 필터 적용
        filteredRSSI = lowPassFilter.ApplyFilter(filteredRSSI);

        // 3. 칼만 필터 적용
        filteredRSSI = kalmanFilter.Update(filteredRSSI);

        // 거리 계산
        float distance = CalculateDistance(filteredRSSI);

        distance = Mathf.Min(distance, 1.0f);

        GameManager.instance.SetPulledLength(distance);
    }
 
    void FixedUpdate()
    {
        lock (queueLock)
        {
            while (dataQueue.Count > 0)
            {
                string data = dataQueue.Dequeue();
                ProcessData(data);
            }
        }
        rssiDistanceCal();
        FsrDetected();
        AngleCal();
    }
    
    float CalculateDistance(float rssi)
    {
        // 유효하지 않은 RSSI 값 처리
        if (rssi == 0)
        {
            return -1.0f; // 거리 측정 불가
        }

        // 거리 계산 공식 적용
        float exponent = (RSSI0 - rssi) / (10 * n);
        float distance = d0 * Mathf.Pow(10, exponent);

        return distance;
    }
}

