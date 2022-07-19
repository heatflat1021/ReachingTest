using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.XR;
using Valve.VR;
using UnityEngine.UI;

public class MyTrackerManager : SingletonMonoBehaviour<MyTrackerManager>, ITrackingSource
{
    private TrackerInfo trackerInfo = new TrackerInfo();

    private SteamVR_Action_Pose trackers = SteamVR_Actions.default_Pose;
    private List<XRNodeState> DevStat;


    // Start is called before the first frame update
    void Start() {
        DevStat = new List<XRNodeState>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 baseTrackerPosition = trackers.GetLocalPosition(SteamVR_Input_Sources.Waist);
        Vector3 handTrackerPosition = trackers.GetLocalPosition(SteamVR_Input_Sources.Chest);

        trackerInfo.baseTrackerPosition = baseTrackerPosition;
        trackerInfo.handTrackerPosition = handTrackerPosition;

        float distance = (baseTrackerPosition - handTrackerPosition).magnitude;
        float previousDistance = (trackerInfo.baseTrackerPositionHistory.Peek() - trackerInfo.handTrackerPositionHistory.Peek()).magnitude;
        float distanceDifference = Math.Abs(distance - previousDistance);

        float previousProgress = trackerInfo.progress;
        float progress = 1.0f - ((distance - trackerInfo.calibratedMinDistance) / (trackerInfo.calibratedMaxDistance - trackerInfo.calibratedMinDistance));
        if (progress < 0.0f)
        {
            progress = 0.0f;
        }
        else if (progress > 1.0f)
        {
            progress = 1.0f;
        }
        trackerInfo.SetValue(TrackerInfoType.Progress, progress);

        // 移動量が設定した閾値以上の場合のみ総移動距離・総移動回数を更新
        if (distanceDifference > ConfigManager.Instance.GetTrackerSensitivityThreshold())
        {
            trackerInfo.AddAccumulatedDistance(Math.Abs(distance - previousDistance));
            trackerInfo.AddAccumulatedProgress(Math.Abs(progress - previousProgress));
        }

        Quaternion hmdRotation;
        InputTracking.GetNodeStates(DevStat);
        foreach (XRNodeState s in DevStat)
        {
            if (s.nodeType == XRNode.Head)
            {
                s.TryGetRotation(out hmdRotation);
                trackerInfo.SetValue(TrackerInfoType.HMDDirection, hmdRotation.y);
            }
        }
    }

    public Vector3 BaseTrackerPosition()
    {
        return Clone(trackerInfo.baseTrackerPosition);
    }


    public Vector3 HandTrackerPosition()
    {
        return Clone(trackerInfo.handTrackerPosition);
    }

    public void CalibrateMinDistance()
    {
        trackerInfo.calibratedMinDistance = (trackerInfo.handTrackerPosition - trackerInfo.baseTrackerPosition).magnitude;
    }

    public void CalibrateMaxDistance()
    {
        trackerInfo.calibratedMaxDistance = (trackerInfo.handTrackerPosition - trackerInfo.baseTrackerPosition).magnitude;
    }

    public float GetProgress()
    {
        return trackerInfo.progress;
    }

    public float GetAccumulatedDistance()
    {
        return trackerInfo.accumulatedDistance;
    }

    public float GetAccumulatedProgress()
    {
        return trackerInfo.accumulatedProgress;
    }

    public float GetHMDDirection()
    {
        return trackerInfo.hmdDirection;
    }

    private Vector3 Clone(Vector3 vector3)
    {
        return new Vector3(vector3.x, vector3.y, vector3.z);
    }
}
