using UnityEngine;

public class NakJeon : ArduinoBase
{

    protected override void Start()
    {
        port = 12345;
        base.Start();
    }

    protected override void ProcessData(string data)
    {
        if (data == "NakJeon")
        {
            Debug.Log("NakJeon");
            Player.instance.nakJeon = true;
        }
        else if(data == "ReadyForValsi")
        {
            Debug.Log("ReadyForValsi");
            Player.instance.nakJeon = false;
        }
    }
}
