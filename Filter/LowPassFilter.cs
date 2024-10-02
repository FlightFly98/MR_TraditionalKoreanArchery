public class LowPassFilter
{
    private float alpha; // 0 < alpha < 1, 필터의 반응속도를 결정 
    private float previousOutput;

    public LowPassFilter(float alpha, float initialOutput = 0f)
    {
        this.alpha = alpha;
        previousOutput = initialOutput;
    }

    public float ApplyFilter(float input)
    {
        // y(k)= α × x(k) + (1 − α) × y(k − 1)
        // y(k) = 현재 필터 출력값
        // x(k) = 현재 입력 값
        // y(k - 1) = 이전 필터 출력값
        // α = 필터 계수
        
        float output = alpha * input + (1 - alpha) * previousOutput;
        previousOutput = output;
        return output;
    }
}
