using System.Collections.Generic;

public class MovingAverageFilter
{
    private Queue<float> rssiValues;
    private int windowSize;
    private float sum;

    public MovingAverageFilter(int windowSize)
    {
        this.windowSize = windowSize;
        rssiValues = new Queue<float>();
        sum = 0f;
    }

    public float AddValue(float newValue)
    {
        rssiValues.Enqueue(newValue);
        sum += newValue;

        if (rssiValues.Count > windowSize)
        {
            sum -= rssiValues.Dequeue();
        }

        return sum / rssiValues.Count;
    }
}
