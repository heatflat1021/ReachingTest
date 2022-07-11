using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Net;
using System.Net.Sockets; //UdpClient
using System.Threading;
using System.Threading.Tasks;
using System.Text;
using Cysharp.Threading.Tasks;

using UDPDataStructure;

public class UDPManager : MonoBehaviour
{
    [SerializeField]
    private string myIPv4 = "192.168.12.15";

    [SerializeField]
    private string otherIPv4 = "192.168.12.1";

    [SerializeField]
    private int mySenderPort = 22222;

    [SerializeField]
    private int otherReceiverPort = 22223;

    private UdpClient udpClient;
    private SynchronizationContext mainContext;

    [HideInInspector]
    public bool udpCommunicationFlag = false;
    
    // Start is called before the first frame update
    void Start()
    {
        mainContext = SynchronizationContext.Current;

        IPAddress myIP = IPAddress.Parse(myIPv4);
        IPEndPoint mySenderIPE = new IPEndPoint(myIP, mySenderPort);
        udpClient = new UdpClient(mySenderIPE);
        udpClient.Client.ReceiveTimeout = 1000;
        udpClient.Connect(otherIPv4, otherReceiverPort);

        CancellationToken cancellationToken = this.GetCancellationTokenOnDestroy();
        Task.Run(() => ThreadReceive(), cancellationToken);

    }

    // Update is called once per frame
    void Update()
    {
        if (udpCommunicationFlag)
        {
            Send();
        }
    }

    public void Send()
    {
        Debug.Log("Sending");
        string data = "0:testtest";
        var msg = Encoding.UTF8.GetBytes(data);
        udpClient.SendAsync(msg, msg.Length);
    }

    private void ThreadReceive()
    {
        while (true)
        {
            Debug.Log(udpClient.Available);
            if (udpClient.Available > 0)
            {
                IPEndPoint othersSenderIPE = null;
                byte[] receivedBytes = udpClient.Receive(ref othersSenderIPE);
                string[] receivedData = Encoding.ASCII.GetString(receivedBytes).Split(':');
                if (Int32.Parse(receivedData[0]) == (int)UDP_DATA_TYPE.PROGRESS)
                {
                    Debug.Log(receivedData[1]);
                }
            }
        }
    }
}
