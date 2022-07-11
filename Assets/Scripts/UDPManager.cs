using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Net;
using System.Net.Sockets; //UdpClient
using System.Threading;
using System.Threading.Tasks;
using Ststem.Text;

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
    
    // Start is called before the first frame update
    void Start()
    {
        mainContext = SynchronizationContext.Current;

        IPAddress myIP = IPAddress.Parse(myIPv4);
        IPEndPoint mySenderIPE = new IPEndPoint(myIP, mySenderPort);
        udpClient = new UdpClient(mySenderIPE);
        udpClient.Client.ReceiveTimeout = 1000;
        udpClient.Connect(otherIPv4, otherReceiverPort);

        Task.Run(() => ThreadReceive());
    }

    // Update is called once per frame
    void Update()
    {
        Send();
    }

    public void Send()
    {
        udpClient.Send("1:testtest");
    }

    private void ThreadReceive()
    {
        while (true)
        {
            if (udpClient.Available > 0)
            {
                IPEndPoint othersSenderIPE = null;
                byte[] receivedBytes = udpClient.Receive(ref othersSenderIPE);
                string[] receivedData = Encoding.ASCII.GetString(receivedBytes).Split(':');
                if (Int32.Parse(receivedData[0]) == UDP_DATA_TYPE.PROGRESS)
                {
                    Debug.Log(receivedData[1]);
                }
            }
        }
    }
}
