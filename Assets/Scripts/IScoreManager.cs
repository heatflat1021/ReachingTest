using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IScoreManager
{
    public float GetAccumulatedDistance();

    public float GetAccumulatedProgress();
}
