using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TrackerInfoType
{
    BaseTrackerPosition,
    HandTrackerPosition,
    CalibratedMinDistance,
    CalibratedMaxDistance,
    Progress,
    AccumulatedDistance,
    AccumulatedProgress,
    BaseHMDDirection,
    HMDDirection,
}

public class TrackerInfo
{
    private Vector3 _baseTrackerPosition;
    private Vector3 _handTrackerPosition;
    public float calibratedMinDistance;
    public float calibratedMaxDistance;
    public float progress;
    private float _accumulatedDistance;
    private float _accumulatedProgress;
    private float _baseHmdDirection;
    private float _hmdDirection;

    public Queue<Vector3> baseTrackerPositionHistory;
    public Queue<Vector3> handTrackerPositionHistory;
    private const int HistoryMaxLength = 4;

    public TrackerInfo()
    {
        this._baseTrackerPosition = Vector3.zero;
        this._handTrackerPosition = Vector3.zero;
        this.calibratedMinDistance = 0.1f;
        this.calibratedMaxDistance = 0.42f;
        this.progress = 0;
        this._accumulatedDistance = 0;
        this._accumulatedProgress = 0;
        this.baseTrackerPositionHistory = new Queue<Vector3>();
        this.handTrackerPositionHistory = new Queue<Vector3>();
        this._baseHmdDirection = 0;
        this._hmdDirection = 0;
    }

    public Vector3 baseTrackerPosition
    {
        get { return _baseTrackerPosition; }
        set
        {
            _baseTrackerPosition = value;

            if (HistoryMaxLength <= baseTrackerPositionHistory.Count)
            {
                baseTrackerPositionHistory.Dequeue();
            }
            baseTrackerPositionHistory.Enqueue(Clone(_baseTrackerPosition));
        }
    }

    public Vector3 handTrackerPosition
    {
        get { return _handTrackerPosition; }
        set
        {
            _handTrackerPosition = value;

            if (HistoryMaxLength <= handTrackerPositionHistory.Count)
            {
                handTrackerPositionHistory.Dequeue();
            }
            handTrackerPositionHistory.Enqueue(Clone(_handTrackerPosition));
        }
    }

    public float accumulatedDistance
    {
        get { return _accumulatedDistance; }
    }

    public float accumulatedProgress
    {
        get { return _accumulatedProgress; }
    }

    public float hmdDirection
    {
        get {
            float hmdDirectionDiff = _hmdDirection - _baseHmdDirection;
            Debug.Log($"{_hmdDirection}, {_baseHmdDirection}, {hmdDirectionDiff}");
            
            if (1 < hmdDirectionDiff)
                hmdDirectionDiff -= 2;

            if (hmdDirectionDiff < -1)
                hmdDirectionDiff += 2;

            return hmdDirectionDiff;
        }
    }

    public void SetValue(TrackerInfoType trackerInfoType, object value)
    {
        switch (trackerInfoType)
        {
            case TrackerInfoType.BaseTrackerPosition:
                this._baseTrackerPosition = (Vector3)value;
                break;
            case TrackerInfoType.HandTrackerPosition:
                this._handTrackerPosition = (Vector3)value;
                break;
            case TrackerInfoType.CalibratedMinDistance:
                this.calibratedMinDistance = System.Convert.ToSingle(value);
                break;
            case TrackerInfoType.CalibratedMaxDistance:
                this.calibratedMaxDistance = System.Convert.ToSingle(value);
                break;
            case TrackerInfoType.Progress:
                this.progress = System.Convert.ToSingle(value);
                break;
            case TrackerInfoType.AccumulatedDistance:
                this._accumulatedDistance = System.Convert.ToSingle(value);
                break;
            case TrackerInfoType.AccumulatedProgress:
                this._accumulatedProgress = System.Convert.ToSingle(value);
                break;
            case TrackerInfoType.BaseHMDDirection:
                this._baseHmdDirection = System.Convert.ToSingle(value);
                break;
            case TrackerInfoType.HMDDirection:
                this._hmdDirection = System.Convert.ToSingle(value);
                break;
        }
    }

    public void AddAccumulatedDistance(float diff)
    {
        _accumulatedDistance += diff;
    }

    public void AddAccumulatedProgress(float diff)
    {
        _accumulatedProgress += diff;
    }

    private Vector3 Clone(Vector3 vector3)
    {
        return new Vector3(vector3.x, vector3.y, vector3.z);
    }
}
