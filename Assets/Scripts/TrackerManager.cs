using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.XR;
using Valve.VR;
using UnityEngine.UI;

public class TrackerManager : MonoBehaviour
{
    //private TrackerInfo trackerInfo = new TrackerInfo();
    [HideInInspector]
    public GameObject playerManagerObject;
    private ManipulationData manipulationData;

    private List<XRNodeState> DevStat;

    private SteamVR_Action_Pose trackers = SteamVR_Actions.default_Pose;


    // Start is called before the first frame update
    void Start() {
        manipulationData = playerManagerObject.GetComponent<PlayerManager>().manipulationDataSource.GetManipulationData();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 baseTrackerPosition = trackers.GetLocalPosition(SteamVR_Input_Sources.Waist);
        Vector3 handTrackerPosition = trackers.GetLocalPosition(SteamVR_Input_Sources.Chest);

        manipulationData.baseTrackerPosition = baseTrackerPosition;
        manipulationData.handTrackerPosition = handTrackerPosition;

        float distance = (baseTrackerPosition - handTrackerPosition).magnitude;
        float previousDistance = (manipulationData.baseTrackerPositionHistory.Peek() - manipulationData.handTrackerPositionHistory.Peek()).magnitude;
        float distanceDifference = Math.Abs(distance - previousDistance);

        float previousProgress = manipulationData.progress;
        float progress = 1.0f - ((distance - manipulationData.calibratedMinDistance) / (manipulationData.calibratedMaxDistance - manipulationData.calibratedMinDistance));
        if (progress < 0.0f)
            progress = 0.0f;
        if (progress > 1.0f)
            progress = 1.0f;
        manipulationData.progress = progress;

        // 移動量が設定した閾値以上の場合のみ総移動距離・総移動回数を更新
        if (distanceDifference > ConfigManager.Instance.GetTrackerSensitivityThreshold())
        {
            manipulationData.accumulatedDistance += Math.Abs(distance - previousDistance);
            manipulationData.accumulatedProgress += Math.Abs(progress - previousProgress);
        }
    }
}
