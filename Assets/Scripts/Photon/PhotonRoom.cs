using System;
using Photon.Pun;
using Photon.Realtime;
using System.IO;
using ExitGames.Client.Photon;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//Manages everthing while we are in Room;

public class PhotonRoom : MonoBehaviourPunCallbacks
{
    public static PhotonRoom room;
    private PhotonView PV;

    public int multiplayerScene;
    private int currentScene;
    public Hashtable Hashtable;
    private GameConsole Gameconsole;

    private void Awake()
    {
        if (PhotonRoom.room == null)
        {
            PhotonRoom.room = this;
        }
        else
        {
            if (PhotonRoom.room != this)
            {
                Destroy(PhotonRoom.room.gameObject);
                PhotonRoom.room = this;
            }
        }
        DontDestroyOnLoad(this.gameObject);
        PV = GetComponent<PhotonView>();
        Hashtable = new Hashtable();
    }

    void start()
    {
       Gameconsole =  GameObject.FindGameObjectWithTag("GameConsole").GetComponent<GameConsole>();
    }

public override void OnEnable()
    {
        base.OnEnable();
        PhotonNetwork.AddCallbackTarget(this);
        SceneManager.sceneLoaded += OnSceneFinishedLoading;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        PhotonNetwork.RemoveCallbackTarget(this);
        SceneManager.sceneLoaded -= OnSceneFinishedLoading;
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        if (!PhotonNetwork.IsMasterClient)
            return;
        StartGame();
    }

    void StartGame()
    {
        Debug.Log("Loading Level");
        PhotonNetwork.LoadLevel(multiplayerScene);
    }

    void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        currentScene = scene.buildIndex;
        if (currentScene == multiplayerScene)
        {
            CreatePlayer();
        }
    }
    private void SetPlayerCustomPropertie()
    {
        Hashtable.Add("hits", 0);
        Hashtable.Add("IsDead", false);
        PhotonNetwork.LocalPlayer.SetCustomProperties(Hashtable);
    }
    private void CreatePlayer()
    {
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonNetworkPlayer"), transform.position, Quaternion.identity, 0);
        SetPlayerCustomPropertie();
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        Debug.Log(otherPlayer.NickName + "has left the game");
        Debug.Log("RoomController was destryed-------------------------------");
        DestroyImmediate(this.gameObject);
    }
}