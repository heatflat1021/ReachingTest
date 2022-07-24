using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    Text progressText;

    [SerializeField]
    Image knifeProgressBar;

    [SerializeField]
    Text accumulatedDistanceText;

    [SerializeField]
    Text accumulatedProgressText;

    [SerializeField]
    Text durationText;

    [SerializeField]
    Text knifeText;

    [SerializeField]
    Text othersAccumulatedDistanceText;

    [SerializeField]
    Text othersAccumulatedProgressText;

    private DateTime startTime;

    void Start()
    {
        startTime = DateTime.Now;
    }

    public void UpdateProgress(float progress)
    {
        progressText.text = (progress * 100).ToString("F0");

        if (progress > 0.98f)
        {
            knifeProgressBar.fillAmount = 1;
        }
        else
        {
            knifeProgressBar.fillAmount = Mathf.Sqrt(progress);
        }
    }

    public void UpdateAccumulatedDistance(float accumulatedDistance)
    {
        accumulatedDistanceText.text = accumulatedDistance.ToString("F1");
    }

    public void UpdateAccumulatedProgress(float accumulatedProgress)
    {
        accumulatedProgressText.text = accumulatedProgress.ToString("F1");
    }

    public void UpdateDuration()
    {
        int seconds = (int)((DateTime.Now - startTime).TotalSeconds);
        durationText.text = $"{seconds / 60}•ª {seconds % 60}•b";
    }

    public void UpdateKnife(int knife)
    {
        knifeText.text = knife.ToString();
    }

    public void UpdateOthersAccumulatedDistance(float othersAccumulatedDistance)
    {
        othersAccumulatedDistanceText.text = othersAccumulatedDistance.ToString("F1");
    }

    public void UpdateOthersAccumulatedProgress(float othersAccumulatedProgress)
    {
        othersAccumulatedProgressText.text = othersAccumulatedProgress.ToString("F1");
    }
}
