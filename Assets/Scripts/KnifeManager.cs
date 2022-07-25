using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeManager : MonoBehaviour
{
    [SerializeField]
    private GameObject particle;

    private const int ParticleShowTime = 150;

    [SerializeField]
    private float requiredProgress;

    private float startAccumulatedProgress;

    private bool isSharpened;

    private bool isImmediatelyAfterSharpened = true;

    private bool isShownEnough = false;

    private Material[] materials;

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

        SetSabiMaterial();
    }

    // Update is called once per frame
    void Update()
    {

        if (isSharpened)
        {
            if (isImmediatelyAfterSharpened)
            {
                PlaySoundKira();
                isImmediatelyAfterSharpened = false;
                SetSharpenedMaterial();
            }

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

    private void PlaySoundKira()
    {
        AudioSource audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.Play();
    }

    private void SetSabiMaterial()
    {
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        materials = meshRenderer.materials; // è„èëÇ´Ç≥ÇÍÇÈëOÇ…materialsÇ…ï€ë∂ÇµÇƒÇ®Ç≠
        meshRenderer.material = materials[1];
    }

    private void SetSharpenedMaterial()
    {
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material = materials[0];
    }
}
