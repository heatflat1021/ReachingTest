using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeManager : MonoBehaviour
{
    [SerializeField]
    private GameObject particle;

    private const int ParticleShowTime = 400;

    [SerializeField]
    private float requiredProgress;

    private float startAccumulatedProgress;

    private bool isSharpened;

    private bool isShownEnough = false;
    public bool IsShownEnough
    {
        get
        {
            return isShownEnough;
        }
    }

    private int shownTime = 0;

    public bool updatedUI = false;

    // Start is called before the first frame update
    void Start()
    {
        particle.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isSharpened)
        {
            if (ParticleShowTime < shownTime++)
            {
                isShownEnough = true;
            }
        }
    }

    public void SetStartAccumulatedProgress(float accumulatedProgress)
    {
        startAccumulatedProgress = accumulatedProgress;
    }

    public void UpdateAccumulatedProgress(float accumulatedProgress)
    {
        if (accumulatedProgress - startAccumulatedProgress > requiredProgress)
        {
            isSharpened = true;

            particle.SetActive(true);
        }
    }

    public bool IsSharpened()
    {
        return isSharpened;
    }
}
