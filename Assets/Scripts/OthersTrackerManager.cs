using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OthersTrackerManager : ITrackingSource
{
    private static Dictionary<int, TrackerInfo> othersTrackerInfo = new Dictionary<int, TrackerInfo>();

    public int playerID;

    public OthersTrackerManager(int playerID)
    {
        this.playerID = playerID;

        if (!othersTrackerInfo.ContainsKey(playerID))
        {
            othersTrackerInfo.Add(playerID, new TrackerInfo());
        }
    }

    public static void SetTrackerInfo(int playerID, TrackerInfoType trackerInfoType, object value)
    {
        if (!othersTrackerInfo.ContainsKey(playerID))
        {
            othersTrackerInfo.Add(playerID, new TrackerInfo());
        }

        TrackerInfo trackerInfo = othersTrackerInfo[playerID];
        trackerInfo.SetValue(trackerInfoType, value);
    }

    public static TrackerInfo GetTrackerInfo(int playerID)
    {
        if (!othersTrackerInfo.ContainsKey(playerID))
        {
            return null;
        }

        return othersTrackerInfo[playerID];
    }

    public Vector3 BaseTrackerPosition()
    {
        return Vector3.zero;
    }


    public Vector3 HandTrackerPosition()
    {
        return Vector3.zero;
    }

    public void CalibrateMinDistance()
    {
        TrackerInfo trackerInfo = othersTrackerInfo[playerID];
        trackerInfo.calibratedMinDistance = (trackerInfo.handTrackerPosition - trackerInfo.baseTrackerPosition).magnitude;
    }

    public void CalibrateMaxDistance()
    {
        TrackerInfo trackerInfo = othersTrackerInfo[playerID];
        trackerInfo.calibratedMaxDistance = (trackerInfo.handTrackerPosition - trackerInfo.baseTrackerPosition).magnitude;
    }

    // todo: 別のインターフェースを用意して、このメソッドは削除する。
    public void CalibrateCameraDirection() { }

    public float GetProgress()
    {
        TrackerInfo trackerInfo = othersTrackerInfo[playerID];
        return trackerInfo.progress;
    }

    public float GetAccumulatedDistance()
    {
        TrackerInfo trackerInfo = othersTrackerInfo[playerID];
        return trackerInfo.accumulatedDistance;
    }

    public float GetAccumulatedProgress()
    {
        TrackerInfo trackerInfo = othersTrackerInfo[playerID];
        return trackerInfo.accumulatedProgress;
    }

    public float GetHMDDirection()
    {
        TrackerInfo trackerInfo = othersTrackerInfo[playerID];
        return trackerInfo.hmdDirection;
    }

    public int GetSharpenedKnife()
    {
        TrackerInfo trackerInfo = othersTrackerInfo[playerID];
        return trackerInfo.sharpenedKnifeNumber;
    }

    private Vector3 Clone(Vector3 vector3)
    {
        return new Vector3(vector3.x, vector3.y, vector3.z);
    }
}
