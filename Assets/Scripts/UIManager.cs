using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    Text progressText;

    [SerializeField]
    Image knifeProgressBar;

    public void UpdateProgress(float progress)
    {
        UpdateProgressText(progress);
        UpdateProgressBar(progress);
    }

    private void UpdateProgressText(float progress)
    {
        progressText.text = (progress * 100).ToString("F0");
    }

    private void UpdateProgressBar(float progress)
    {
        if (progress > 0.97f)
        {
            knifeProgressBar.fillAmount = 1;
        }
        else
        {
            knifeProgressBar.fillAmount = Mathf.Sqrt(progress);
        }
    }
}
