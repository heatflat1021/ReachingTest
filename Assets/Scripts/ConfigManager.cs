using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigManager : SingletonMonoBehaviour<ConfigManager>
{
    [SerializeField]
    [Range(1, 5)]
    private int trackerSensitivityLevel = 3;

    private readonly Dictionary<int, float> trackerSensitivityLevelToTrackerSensitivityThreshold = new Dictionary<int, float>() {
        { 1, 0.009f },
        { 2, 0.007f },
        { 3, 0.005f },
        { 4, 0.003f },
        { 5, 0.001f },
    };

    public float GetTrackerSensitivityThreshold()
    {
        if (!trackerSensitivityLevelToTrackerSensitivityThreshold.ContainsKey(trackerSensitivityLevel))
        {
            if (trackerSensitivityLevel < 1)
            {
                return trackerSensitivityLevelToTrackerSensitivityThreshold[1];
            }
            else
            {
                return trackerSensitivityLevelToTrackerSensitivityThreshold[5];
            }
        }

        return trackerSensitivityLevelToTrackerSensitivityThreshold[trackerSensitivityLevel];
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
