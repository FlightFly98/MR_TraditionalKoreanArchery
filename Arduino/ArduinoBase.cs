using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public abstract class ArduinoBase : MonoBehaviour
{
    protected UdpClient udpClient;
    protected Thread receiveThread;
    protected bool isRunning;
    protected Queue<string> dataQueue = new Queue<string>();
    protected readonly object queueLock = new object();
    public int port;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        udpClient = new UdpClient(port);
        isRunning = true;
        receiveThread = new Thread(new ThreadStart(ReceiveData));
        receiveThread.IsBackground = true;
        receiveThread.Start();
    }
    private void ReceiveData()
    {
        IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, port);
        while (isRunning)
        {
            try
            {
                if (udpClient.Available > 0)
                {
                    byte[] data = udpClient.Receive(ref anyIP);
                    string text = Encoding.UTF8.GetString(data);
                    lock (queueLock)
                    {
                        dataQueue.Enqueue(text);
                    }
                }
                else
                {
                    Thread.Sleep(10); // Add a short sleep to prevent tight looping
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError(e.ToString());
            }
        }
    }

    protected abstract void ProcessData(string data);
    protected virtual void OnApplicationQuit()
    {
        isRunning = false;
        if (receiveThread != null)
        {
            receiveThread.Join();
            receiveThread = null;
        }
        if (udpClient != null)
        {
            udpClient.Close();
            udpClient = null;
        }
    }
}
