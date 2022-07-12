using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public bool isMyPlayerManager = false;
    public int playerID = 0;

    public int knifeID = 1;
    public ITrackingSource trackingSource;
    public IScoreManager scoreManager;

    private readonly Vector3 InitialHandsPosition = new Vector3(0, 0.44f, 1.42f);
    private readonly Vector3 InitialHandsRotation = new Vector3(17.3f, 0, 0);
    private readonly Vector3 InitialKnifePosition = new Vector3(0.19f, 0.11f, 1.5f);
    private readonly Vector3 InitialKnifeRotation = new Vector3(-90, -90, -180);

    private const int StandardCameraRigDirectionOffsetY = -720;
    private readonly Vector3 StandardCameraRigPositionOffset = new Vector3(0, 0.4f, 0);

    private readonly Vector3 MaxMoveDistance = new Vector3(0, 0, 0.9f);

    private GameObject hands;
    private GameObject leftHand;
    private GameObject rightHand;
    private GameObject knife;
    private GameObject uiCanvas;

    private UIManager uiManager;

    private UDPManager udpManager;

    // Start is called before the first frame update
    void Start()
    {
        InitializeHands();
        InitializeKnife();

        GameObject canvas = transform.Find("Canvas").gameObject;
        uiManager = canvas.GetComponent<UIManager>();

        GameObject udpManagerObject = GameObject.Find("UDPManager");
        udpManager = udpManagerObject.GetComponent<UDPManager>();
    }

    // Update is called once per frame
    void Update()
    {
        float progress = trackingSource.GetProgress();

        if (trackingSource.GetAccumulatedProgress() > 10)
        {
            GameObject particle = knife.transform.Find("Particle").gameObject;
            particle.SetActive(true);
        }


        MoveHands(progress);
        MoveKnife(progress);

        uiManager.UpdateProgress(progress);

        if (isMyPlayerManager)
        {
            udpManager.Send(playerID + ":" + 0 + ":" + progress);
        }
    }

    public void CalibrateCamera()
    {
        GameObject cameraRig = this.transform.Find("CameraRig(Clone)").gameObject;
        GameObject camera = cameraRig.transform.Find("Camera").gameObject;

        cameraRig.transform.eulerAngles = this.transform.eulerAngles;
        cameraRig.transform.eulerAngles += new Vector3(0, cameraRig.transform.eulerAngles.y - camera.transform.eulerAngles.y + StandardCameraRigDirectionOffsetY, 0);

        cameraRig.transform.position = this.transform.position;
        cameraRig.transform.position += new Vector3(cameraRig.transform.position.x - camera.transform.position.x, cameraRig.transform.position.y - camera.transform.position.y, cameraRig.transform.position.z - camera.transform.position.z) + StandardCameraRigPositionOffset;
    }

    private void InitializeHands()
    {
        hands = this.transform.Find("Hands").gameObject;
        leftHand = this.transform.Find("Hands/Hand_L").gameObject;
        rightHand = this.transform.Find("Hands/Hand_R").gameObject;

        hands.transform.localPosition = InitialHandsPosition;
        hands.transform.localRotation = Quaternion.Euler(InitialHandsRotation);
    }

    private void InitializeKnife()
    {
        knife = (GameObject)Resources.Load($"Prefabs/CookKnife_{knifeID}");
        Vector3 spawnPosition = rightHand.transform.position;
        knife.name = $"knife_{knifeID}";
        knife = Instantiate(knife, spawnPosition, Quaternion.identity) as GameObject;
        knife.transform.SetParent(this.transform, false);
        knife.transform.localPosition = InitialKnifePosition;
        knife.transform.localRotation = Quaternion.Euler(InitialKnifeRotation);
    }

    private void MoveHands(float progress)
    {
        hands.transform.localPosition = InitialHandsPosition + MaxMoveDistance * progress;
    }

    private void MoveKnife(float progress)
    {
        knife.transform.localPosition = InitialKnifePosition + MaxMoveDistance * progress;
    }
}
