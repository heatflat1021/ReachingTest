using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OthersScoreManager : IScoreManager
{
    private static Dictionary<int, ScoreInfo> othersScoreInfo = new Dictionary<int, ScoreInfo>();

    public int playerID;

    public OthersScoreManager(int playerID)
    {
        this.playerID = playerID;

        if (!othersScoreInfo.ContainsKey(playerID))
        {
            othersScoreInfo.Add(playerID, new ScoreInfo());
        }
    }

    public float GetAccumulatedDistance()
    {
        ScoreInfo scoreInfo = othersScoreInfo[playerID];
        return scoreInfo.accumulatedDistance;
    }

    public float GetAccumulatedProgress()
    {
        ScoreInfo scoreInfo = othersScoreInfo[playerID];
        return scoreInfo.accumulatedProgress;
    }
}
