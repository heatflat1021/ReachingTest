using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
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
            .Subscribe(msg => {
                string[] receivedData = msg.Split(':');
                switch ((TrackerInfoType)Enum.ToObject(typeof(TrackerInfoType), Int32.Parse(receivedData[1])))
                {
                    // floatÇ…ïœä∑Ç∑ÇÈÉpÉ^Å[Éì
                    case TrackerInfoType.Progress:
                    case TrackerInfoType.AccumulatedDistance:
                    case TrackerInfoType.AccumulatedProgress:
                    case TrackerInfoType.HMDDirection:
                        OthersTrackerManager.SetTrackerInfo(Int32.Parse(receivedData[0]), (TrackerInfoType)Enum.ToObject(typeof(TrackerInfoType), Int32.Parse(receivedData[1])), float.Parse(receivedData[2]));
                        break;
                }
                Debug.Log(msg);
            }).AddTo(this);

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Send(string data)
    {
        if (udpCommunicationFlag)
        {
            var msg = Encoding.UTF8.GetBytes(data);
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
