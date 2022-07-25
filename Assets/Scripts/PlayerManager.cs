using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [HideInInspector]
    public bool isMyPlayerManager = false;

    [HideInInspector]
    public bool isVRoidAvatar = false;

    [HideInInspector]
    public int playerID = 0;

    public ITrackingSource trackingSource;
    public IScoreManager scoreManager;

    [Space(20)]
    [SerializeField]
    GameObject headDirection;

    private readonly Vector3 InitialHandsPosition = new Vector3(0, 0.12f, 1.42f);
    private readonly Vector3 InitialHandsRotation = new Vector3(17.3f, 0, 0);
    private readonly Vector3 InitialKnifePosition = new Vector3(0.19f, 0.04f, 1.5f);
    private readonly Vector3 InitialKnifeRotation = new Vector3(-90, -90, -180);
    private readonly Vector3 InitialHeadPosition = new Vector3(0, 1.5f, 0.5f);

    private const int StandardCameraRigDirectionOffsetY = -720;
    private readonly Vector3 StandardCameraRigPositionOffset = new Vector3(0, 0.5f, -0.2f);

    private readonly Vector3 MaxMoveDistance = new Vector3(0, 0, 0.9f);

    private GameObject hands;
    private GameObject leftHand;
    private GameObject rightHand;
    //public GameObject neck;
    private GameObject knife;
    private int sharpenedKnifeNumber = 0;
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
        uiManager.UpdateKnife(sharpenedKnifeNumber);

        GameObject udpManagerObject = GameObject.Find("UDPManager");
        udpManager = udpManagerObject.GetComponent<UDPManager>();
    }

    // Update is called once per frame
    void Update()
    {
        float progress = trackingSource.GetProgress();
        float accumulatedDistance = trackingSource.GetAccumulatedDistance();
        float accumulatedProgress = trackingSource.GetAccumulatedProgress();
        float hmdDirection = trackingSource.GetHMDDirection();

        // アバターの操作
        MoveHands(progress);
        MoveKnife(progress);
        MoveHead(progress, hmdDirection);

        if (isMyPlayerManager)
        {
            // ナイフ関連の処理
            KnifeManager knifeManager = knife.GetComponent<KnifeManager>();
            knifeManager.UpdateAccumulatedProgress(accumulatedProgress);
            if (knifeManager.IsSharpened())
            {
                if (!knifeManager.updatedUI)
                {
                    uiManager.UpdateKnife(++sharpenedKnifeNumber);
                    knifeManager.updatedUI = true;
                }

                if (knifeManager.IsShownEnough)
                {
                    Destroy(knife);
                    InitializeKnife();
                    knifeManager = knife.GetComponent<KnifeManager>();
                    knifeManager.SetStartAccumulatedProgress(accumulatedProgress);
                }
            }

            // UIの更新
            int nextOtherPlayerID = ((playerID + 1) <= GameManager.Instance.PlayerSpawnNumber) ? (playerID+1) : (playerID + 1) % GameManager.Instance.PlayerSpawnNumber;
            TrackerInfo nextOthersTrackerInfo = OthersTrackerManager.GetTrackerInfo(nextOtherPlayerID);
            uiManager.UpdateProgress(progress);
            uiManager.UpdateAccumulatedDistance(accumulatedDistance);
            uiManager.UpdateAccumulatedProgress(accumulatedProgress);
            uiManager.UpdateDuration();
            uiManager.UpdateOthersAccumulatedDistance(nextOthersTrackerInfo.accumulatedDistance);
            uiManager.UpdateOthersAccumulatedProgress(nextOthersTrackerInfo.accumulatedProgress);

            // データの送信
            udpManager.Send(playerID + ":" + (int)TrackerInfoType.Progress + ":" + progress);
            udpManager.Send(playerID + ":" + (int)TrackerInfoType.AccumulatedDistance + ":" + accumulatedDistance);
            udpManager.Send(playerID + ":" + (int)TrackerInfoType.AccumulatedProgress + ":" + accumulatedProgress);
            udpManager.Send(playerID + ":" + (int)TrackerInfoType.HMDDirection + ":" + hmdDirection);
            udpManager.Send(playerID + ":" + (int)TrackerInfoType.SharpenedKnifeNumber + ":" + sharpenedKnifeNumber);
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

        trackingSource.CalibrateCameraDirection();
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
        int knifeId = Random.Range(1, 3 + 1);
        knife = (GameObject)Resources.Load($"Prefabs/CookKnife_{knifeId}");
        Vector3 spawnPosition = rightHand.transform.position;
        knife.name = $"knife_{knifeId}";
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

    private void MoveHead(float progress, float direction)
    {
        headDirection.transform.localPosition = InitialHeadPosition + MaxMoveDistance * progress * 0.3f;

        Vector3 headDirectionEuler;

        // VRoidアバターでない場合は、頭の回転量を修正する必要がある。
        if (isVRoidAvatar)
        {
            headDirectionEuler = new Vector3(0, -direction * 180, 0);
        }
        else
        {
            headDirectionEuler = new Vector3(0, -direction * 180, 0) + new Vector3(0, -90, -90);
        }
        headDirection.transform.localRotation = Quaternion.Euler(headDirectionEuler);
    }
}
