using UnityEngine;

public class ShockSenor : ArduinoBase
{

    protected override void Start()
    {
        port = 12348;  
        base.Start();
    }

    protected override void ProcessData(string data)
    {
        if (data == "Shock detected")
        {
            Debug.Log("Shock detected!");
            GameManager.instance.isImpactDetected = true;
            GameManager.instance.SetInitialVelocity();
            Player.instance.shock = true;
            //InGameUI.instance.SetSoonText(false);
        }
    }

    void FixedUpdate()
    {
        //ProcessRSSI();

        lock (queueLock)
        {
            while (dataQueue.Count > 0)
            {
                string data = dataQueue.Dequeue();
                ProcessData(data);
            }
        }
    }
}
