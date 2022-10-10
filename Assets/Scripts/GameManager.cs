using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    [SerializeField]
    private List<GameObject> players;

    [SerializeField]
    [Tooltip("自分のプレイヤーID"), Range(0, 4)]
    private int myPlayerID;

    [SerializeField]
    GameObject udpManagerObject;

    private static readonly Vector3 PLAYER_INITIAL_SPAWN_POSITION = new Vector3(-2.366f, 0.909f, 0.980f);
    private static readonly Vector3 PLAYER_SPAWN_DISTANCE = new Vector3(-0.700f, 0f, 0f);
    private static readonly Quaternion PLAYER_SPAWN_DIRECTION = Quaternion.AngleAxis(180, Vector3.up);

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
            myPlayerManager.manipulationDataSource.CalibrateMaxDistance();
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            myPlayerManager.manipulationDataSource.CalibrateMinDistance();
        }
        if (Input.GetKeyUp(KeyCode.R))
        {
            myPlayerManager.CalibrateCamera();
        }
        if (Input.GetKeyUp(KeyCode.C))
        {
            UDPManager udpManager = udpManagerObject.GetComponent<UDPManager>();
            udpManager.udpCommunicationFlag = true;
        }
    }

    private void SpawnPlayers()
    {
        for (int playerId=0; playerId<players.Count; playerId++)
        {
            GameObject player = players[playerId];
            Vector3 spawnPosition = PLAYER_INITIAL_SPAWN_POSITION + PLAYER_SPAWN_DISTANCE * playerId;
            player = Instantiate(player, spawnPosition, PLAYER_SPAWN_DIRECTION) as GameObject;
            PlayerManager playerManager = player.GetComponent<PlayerManager>();
            playerManager.playerID = playerId;
            playerManager.manipulationDataSource = new ManipulationDataSource(playerId);

            if (playerId == myPlayerID)
            {
                playerManager.isMyPlayerManager = true;

                MountPlayer(player);


                //player.gameObject.AddComponent<MyTrackerManager>();
                //playerManager.trackingSource = player.gameObject.GetComponent<MyTrackerManager>();

                myPlayer = player;
                myPlayerManager = playerManager;

                // VRoidから生成したアバターの場合は頭のレンダリングを消すことができる
                if (playerManager.IsVRoidAvatar)
                {
                    DeleteVRoidHeadRendering(player);
                }
            }
            else
            {
                
                //playerManager.trackingSource = new OthersTrackerManager(playerId);

                GameObject canvas = player.transform.Find("Canvas").gameObject;
                canvas.SetActive(false);
            }

            ManipulationDataSource m = new ManipulationDataSource(playerId);
            playerManager.manipulationDataSource = m;

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

    private void DeleteVRoidHeadRendering(GameObject player)
    {
        GameObject face = player.transform.Find("Avatar/Face").gameObject;
        SkinnedMeshRenderer faceRenderer = face.GetComponent<SkinnedMeshRenderer>();
        faceRenderer.enabled = false;
        GameObject hair = player.transform.Find("Avatar/Hair").gameObject;
        SkinnedMeshRenderer hairRenderer = hair.GetComponent<SkinnedMeshRenderer>();
        hairRenderer.enabled = false;
    }

    public int PlayersCount()
    {
        return players.Count;
    }
}
