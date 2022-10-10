using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ManipulationDataType
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
    SharpenedKnifeNumber,
}

public class ManipulationData
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
    private int _sharpenedKnifeNumber;

    public Queue<Vector3> baseTrackerPositionHistory;
    public Queue<Vector3> handTrackerPositionHistory;
    private const int HistoryMaxLength = 4;

    public ManipulationData()
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
            baseTrackerPositionHistory.Enqueue(_baseTrackerPosition.Clone());
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
            handTrackerPositionHistory.Enqueue(_handTrackerPosition.Clone());
        }
    }

    public float accumulatedDistance
    {
        get { return _accumulatedDistance; }
        set { _accumulatedDistance = value; }
    }

    public float accumulatedProgress
    {
        get { return _accumulatedProgress; }
        set { _accumulatedProgress = value; }
    }

    public float hmdDirection
    {
        get
        {
            float hmdDirectionDiff = _hmdDirection - _baseHmdDirection;

            if (1 < hmdDirectionDiff)
                hmdDirectionDiff -= 2;

            if (hmdDirectionDiff < -1)
                hmdDirectionDiff += 2;

            return hmdDirectionDiff;
        }
        set
        {
            _hmdDirection = value;
        }
    }

    public int sharpenedKnifeNumber
    {
        get
        {
            return _sharpenedKnifeNumber;
        }
    }

    public void SetValue(ManipulationDataType manipulationDataType, object value)
    {
        switch (manipulationDataType)
        {
            case ManipulationDataType.BaseTrackerPosition:
                this._baseTrackerPosition = (Vector3)value;
                break;
            case ManipulationDataType.HandTrackerPosition:
                this._handTrackerPosition = (Vector3)value;
                break;
            case ManipulationDataType.CalibratedMinDistance:
                this.calibratedMinDistance = System.Convert.ToSingle(value);
                break;
            case ManipulationDataType.CalibratedMaxDistance:
                this.calibratedMaxDistance = System.Convert.ToSingle(value);
                break;
            case ManipulationDataType.Progress:
                this.progress = System.Convert.ToSingle(value);
                break;
            case ManipulationDataType.AccumulatedDistance:
                this._accumulatedDistance = System.Convert.ToSingle(value);
                break;
            case ManipulationDataType.AccumulatedProgress:
                this._accumulatedProgress = System.Convert.ToSingle(value);
                break;
            case ManipulationDataType.BaseHMDDirection:
                this._baseHmdDirection = System.Convert.ToSingle(value);
                break;
            case ManipulationDataType.HMDDirection:
                this._hmdDirection = System.Convert.ToSingle(value);
                break;
            case ManipulationDataType.SharpenedKnifeNumber:
                this._sharpenedKnifeNumber = System.Convert.ToInt32(value);
                break;
        }
    }

    public static ManipulationDataType? FromUDPDataType(UDPDataType udpDataType)
    {
        switch(udpDataType) {
            case UDPDataType.Progress:
                return ManipulationDataType.Progress;
            case UDPDataType.AccumulatedProgress:
                return ManipulationDataType.AccumulatedProgress;
            case UDPDataType.AccumulatedDistance:
                return ManipulationDataType.AccumulatedDistance;
            case UDPDataType.HMDDirection:
                return ManipulationDataType.HMDDirection;
            case UDPDataType.SharpenedKnifeNumber:
                return ManipulationDataType.SharpenedKnifeNumber;
        }
        return null;
    }
}
