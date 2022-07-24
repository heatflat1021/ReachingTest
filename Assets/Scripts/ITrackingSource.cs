using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITrackingSource
{
    public Vector3 BaseTrackerPosition();

    public Vector3 HandTrackerPosition();

    public void CalibrateMinDistance();

    public void CalibrateMaxDistance();

    public void CalibrateCameraDirection();

    public float GetProgress();

    public float GetAccumulatedDistance();

    public float GetAccumulatedProgress();

    public float GetHMDDirection();

    public int GetSharpenedKnife();
}
