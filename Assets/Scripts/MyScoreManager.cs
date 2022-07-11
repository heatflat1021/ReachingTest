using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyScoreManager : IScoreManager
{
    private ScoreInfo scoreInfo = new ScoreInfo();

    public void SetScoreInfo(ScoreInfoType scoreInfoType, object value)
    {
        switch (scoreInfoType)
        {
            case ScoreInfoType.AccumulatedDistance:
                scoreInfo.accumulatedDistance = (float)value;
                break;
        }
    }

    public float GetAccumulatedDistance()
    {
        return scoreInfo.accumulatedDistance;
    }

    public float GetAccumulatedProgress()
    {
        return scoreInfo.accumulatedProgress;
    }
}
