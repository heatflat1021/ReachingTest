using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UniRx;

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

    [HideInInspector]
    public bool udpCommunicationFlag = false;

    private Subject<string> subject = new Subject<string>();

    // Start is called before the first frame update
    void Start()
    {
        IPAddress myIP = IPAddress.Parse(myIPv4);
        IPEndPoint mySenderIPE = new IPEndPoint(myIP, mySenderPort);
        udpClient = new UdpClient(mySenderIPE);
        udpClient.Client.ReceiveTimeout = 1000;
        udpClient.Connect(otherIPv4, otherReceiverPort);

        udpClient.BeginReceive(OnReceived, udpClient);
        subject
            .ObserveOnMainThread()
            .Subscribe(receivedData => {
                UDPData udpData = new UDPData(receivedData);
                ManipulationDataSource.SetManipulationData(
                    udpData.playerID,
                    ManipulationData.FromUDPDataType(udpData.dataType).Value,
                    udpData.data);
            }).AddTo(this);

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Send(int playerID, UDPDataType udpDataType, string data)
    {
        UDPData udpData = new UDPData(playerID, udpDataType, data);
        string sendData = udpData.ParseToString();

        if (udpCommunicationFlag)
        {
            var msg = Encoding.UTF8.GetBytes(sendData);
            udpClient.SendAsync(msg, msg.Length);
        }
    }

    private void OnReceived(System.IAsyncResult result)
    {
        UdpClient getUdp = (UdpClient)result.AsyncState;
        IPEndPoint ipEnd = null;

        byte[] getByte = getUdp.EndReceive(result, ref ipEnd);

        var message = Encoding.UTF8.GetString(getByte);
        subject.OnNext(message);

        getUdp.BeginReceive(OnReceived, getUdp);
    }

    private void OnDestroy()
    {
        udpClient.Close();
    }
}
