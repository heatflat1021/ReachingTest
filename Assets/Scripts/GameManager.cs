using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    [SerializeField]
    [Tooltip("プレイヤーの数"), Range(1, 8)]
    private int PLAYER_SPAWN_NUMBER;
    public int PlayerSpawnNumber
    {
        get
        {
            return PLAYER_SPAWN_NUMBER;
        }
    }

    [SerializeField]
    [Tooltip("自分のプレイヤーID"), Range(1, 8)]
    int myPlayerID;

    private static readonly Vector3 PLAYER_INITIAL_SPAWN_POSITION = new Vector3(-2.366f, 0.909f, 0.980f);
    private static readonly Vector3 PLAYER_SPAWN_DISTANCE = new Vector3(-0.700f, 0f, 0f);
    private static readonly Quaternion PLAYER_SPAWN_DIRECTION = Quaternion.AngleAxis(180, Vector3.up);
    private static readonly Vector3 MIRROR_RELATIVE_POSITION = new Vector3(0.5f, 2.2f, 0);

    private GameObject myPlayer;
    private PlayerManager myPlayerManager;

    void Start()
    {
        SpawnPlayers();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.DownArrow))
        {
            myPlayerManager.trackingSource.CalibrateMaxDistance();
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            myPlayerManager.trackingSource.CalibrateMinDistance();
        }
        if (Input.GetKeyUp(KeyCode.R))
        {
            myPlayerManager.CalibrateCamera();
        }
        if (Input.GetKeyUp(KeyCode.C))
        {
            GameObject udpManagerObject = GameObject.Find("UDPManager");
            UDPManager udpManager = udpManagerObject.GetComponent<UDPManager>();
            udpManager.udpCommunicationFlag = true;
        }
    }

    private void SpawnPlayers()
    {
        for (int i=0; i<PLAYER_SPAWN_NUMBER; i++)
        {
            GameObject player = (GameObject)Resources.Load("Prefabs/Player");
            Vector3 spawnPosition = PLAYER_INITIAL_SPAWN_POSITION + PLAYER_SPAWN_DISTANCE * i;
            player.name = $"Player_{i + 1}";
            player = Instantiate(player, spawnPosition, PLAYER_SPAWN_DIRECTION) as GameObject;
            PlayerManager playerManager = player.GetComponent<PlayerManager>();
            playerManager.playerID = i + 1;

            if (i+1 == myPlayerID)
            {
                playerManager.isMyPlayerManager = true;

                MountPlayer(player);
                player.gameObject.AddComponent<MyTrackerManager>();
                playerManager.trackingSource = player.gameObject.GetComponent<MyTrackerManager>();

                myPlayer = player;
                myPlayerManager = playerManager;
            }
            else
            {
                playerManager.trackingSource = new OthersTrackerManager(i+1);
                
                GameObject canvas = player.transform.Find("Canvas").gameObject;
                canvas.SetActive(false);
            }

            player.transform.localPosition = spawnPosition;
        }

    }

    private void MountPlayer(GameObject mountTarget)
    {
        GameObject cameraRig = (GameObject)Resources.Load("Prefabs/CameraRig");
        cameraRig = Instantiate(cameraRig, Vector3.zero, Quaternion.identity) as GameObject;

        cameraRig.transform.SetParent(mountTarget.transform, true);
        cameraRig.transform.localPosition = new Vector3(0, 0, 0);
        cameraRig.transform.localRotation = Quaternion.identity;
    }
}
