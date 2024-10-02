public class KalmanFilter
{
    public float Q; // 프로세스 노이즈 공분산
    public float R; // 측정 노이즈 공분산
    private float P = 1f; // 추정 오차 공분산
    private float X = 0f; // 상태 추정값
    private float K;      // 칼만 이득

    public KalmanFilter(float processNoise, float measurementNoise)
    {
        Q = processNoise;
        R = measurementNoise;
    }

    public float Update(float measurement)
    {
        // 예측 단계 (생략 가능)
        P = P + Q;

        // 칼만 이득 계산
        K = P / (P + R);

        // 상태 추정값 갱신
        X = X + K * (measurement - X);

        // 추정 오차 공분산 갱신
        P = (1 - K) * P;

        return X;
    }
}

