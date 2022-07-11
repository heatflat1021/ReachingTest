using UnityEngine;

public enum ScoreInfoType
{
    AccumulatedDistance,
    AccumulatedProgress,
}

public class ScoreInfo
{
    public float accumulatedDistance;
    public float accumulatedProgress;

    public ScoreInfo()
    {
        this.accumulatedDistance = 0;
        this.accumulatedProgress = 0;
    }
}
