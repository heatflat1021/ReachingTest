using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManipulationDataSource
{
    private static Dictionary<int, ManipulationData> manipulationDataDictionary = new Dictionary<int, ManipulationData>();

    public int playerID;

    public ManipulationDataSource(int playerID)
    {
        this.playerID = playerID;

        if (!manipulationDataDictionary.ContainsKey(playerID))
        {
            manipulationDataDictionary.Add(playerID, new ManipulationData());
        }
    }

    public static void SetManipulationData(int playerID, ManipulationDataType manipulationDataType, object value)
    {
        if (!manipulationDataDictionary.ContainsKey(playerID))
        {
            manipulationDataDictionary.Add(playerID, new ManipulationData());
        }

        ManipulationData manipulationData = manipulationDataDictionary[playerID];
        manipulationData.SetValue(manipulationDataType, value);
    }

    public void SetManipulatinData(ManipulationDataType manipulationDataType, object value)
    {
        ManipulationData manipulationData = manipulationDataDictionary[playerID];
        manipulationData.SetValue(manipulationDataType, value);
    }
    public static ManipulationData GetManipulationData(int playerID)
    {
        if (!manipulationDataDictionary.ContainsKey(playerID))
        {
            return null;
        }

        return manipulationDataDictionary[playerID];
    }

    public ManipulationData GetManipulationData()
    {
        return manipulationDataDictionary[playerID];
    }

    public void CalibrateMinDistance()
    {
        ManipulationData manipulationData = manipulationDataDictionary[playerID];
        manipulationData.calibratedMinDistance = (manipulationData.handTrackerPosition - manipulationData.baseTrackerPosition).magnitude;
    }

    public void CalibrateMaxDistance()
    {
        ManipulationData manipulationData = manipulationDataDictionary[playerID];
        manipulationData.calibratedMaxDistance = (manipulationData.handTrackerPosition - manipulationData.baseTrackerPosition).magnitude;
    }
    public void SetHmdDirection(float direction)
    {
        ManipulationData manipulationData = manipulationDataDictionary[playerID];
        manipulationData.hmdDirection = direction;
    }
}
