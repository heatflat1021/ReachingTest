using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HMDManager : MonoBehaviour
{
    [HideInInspector]
    public GameObject playerManagerObject;
    private PlayerManager playerManager;

    private List<XRNodeState> DevStat;

    // Start is called before the first frame update
    void Start()
    {
        playerManager = playerManagerObject.GetComponent<PlayerManager>();

        DevStat = new List<XRNodeState>();
    }

    // Update is called once per frame
    void Update()
    {
        Quaternion hmdRotation;
        InputTracking.GetNodeStates(DevStat);

        foreach (XRNodeState s in DevStat)
        {
            if (s.nodeType == XRNode.Head)
            {
                s.TryGetRotation(out hmdRotation);
                playerManager.manipulationDataSource.SetHmdDirection(hmdRotation.y);
            }
        }
    }
}
