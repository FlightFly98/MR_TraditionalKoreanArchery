using UnityEngine;

public class ZoomHand : ArduinoBase
{
    
    private Vector3 acc_raw;
    private Vector3 gyro_raw;
    private Vector3 acc_cor;
    private Vector3 gyro_cor;
    private float[] gyro_offset = {2.3f, -0.3f, 1};
    private int[] acc_offset = {350, -510, -16100};
    private float pitch, roll, yaw;
    private float previousTime;

    private const float alpha = 0.98f;
    private const float gyroSensitivity = 16.4f;

    protected override void Start()
    {
        port = 12346;
        base.Start();

        previousTime = Time.time;
    }

    protected override void ProcessData(string data)
    {
        // "Accel: ax, ay, az, Gyro: gx, gy, gz"
        string[] parts = data.Split(',');
        if (parts.Length == 6)
        {
            float ax, ay, az, gx, gy, gz;
            float.TryParse(parts[0].Split(':')[1].Trim(), out ax);
            float.TryParse(parts[1].Trim(), out ay);
            float.TryParse(parts[2].Trim(), out az);
            float.TryParse(parts[3].Split(':')[1].Trim(), out gx);
            float.TryParse(parts[4].Trim(), out gy);
            float.TryParse(parts[5].Trim(), out gz);

            acc_raw = new Vector3(ax, ay, az);
            gyro_raw = new Vector3(gx, gy, gz);
            
            for(int i = 0; i < 3; i++) 
            {
                acc_cor[i] = acc_raw[i] + acc_offset[i];
                gyro_cor[i] = (gyro_raw[i] / gyroSensitivity) + gyro_offset[i];
            }
            //Debug.Log("Acc: x " + acc_cor[0]  + " y " + acc_cor[1] + " z " + acc_cor[2]);
            //Debug.Log("Gyro: x " + gyro_cor[0] + " y " + gyro_cor[1]+ " z " + gyro_cor[2]);
        }
    }

    void AngleCal()
    {
        float currentTime = Time.time;
        float deltaTime = currentTime - previousTime;
        previousTime = currentTime;

        float gyroPitch = gyro_cor[0] * deltaTime;
        float gyroRoll = gyro_cor[1] * deltaTime;
        float gyroYaw = gyro_cor[2] * deltaTime;
        
        float vecPitch = Mathf.Sqrt(Mathf.Pow(acc_cor[0], 2) + Mathf.Pow(acc_cor[2], 2));
        float accelPitch = Mathf.Atan2(acc_cor[1], vecPitch) * Mathf.Rad2Deg;
        
        float vecRoll = Mathf.Sqrt(Mathf.Pow(acc_cor[1], 2) + Mathf.Pow(acc_cor[2], 2));
        float accelRoll = Mathf.Atan2(acc_cor[0], vecRoll) * Mathf.Rad2Deg;

        pitch = alpha * (pitch + gyroPitch) + (1.0f - alpha) * accelPitch;
        roll = alpha * (roll + gyroRoll) + (1.0f - alpha) * accelRoll;
        yaw += gyroYaw;

        //Debug.Log("Pitch: " + pitch + "Roll: " + roll + "Yaw: " + yaw);

        GameManager.instance.SetZoomHandPitch(pitch);
        GameManager.instance.SetZoomHandRoll(roll);
        GameManager.instance.SetZoomHandYaw(yaw);

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
        AngleCal();
    }
}
