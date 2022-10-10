using System;

public enum UDPDataType
{
    Progress,
    AccumulatedProgress,
    AccumulatedDistance,
    HMDDirection,
    SharpenedKnifeNumber,
}
public class UDPData
{
    public int playerID;
    public UDPDataType dataType;
    public string data;

    public UDPData(int playerID, UDPDataType dataType, string data)
    {
        this.playerID = playerID;
        this.dataType = dataType;
        this.data = data;
    }

    public UDPData(string receivedData)
    {
        string[] splitedReceivedData = receivedData.Split(':');

        this.playerID = Int32.Parse(splitedReceivedData[0]);
        this.dataType = (UDPDataType)Enum.ToObject(typeof(UDPDataType), Int32.Parse(splitedReceivedData[1]));
        this.data = splitedReceivedData[2];
    }

    public string ParseToString()
    {
        return playerID + ":" + (int)dataType + ":" + data;
    }
}
