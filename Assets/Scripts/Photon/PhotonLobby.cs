using System;
using System.Collections.Generic;
using System.Net.Sockets;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

//Manages everthing while we are in Lobby;
public class PhotonLobby : MonoBehaviourPunCallbacks
{
    public static PhotonLobby lobby;

    public GameObject battleButton;
    public GameObject cancelButton;
    public GameObject RetryButton;
    public Button CreateRoomButton;
    public Button JoinRoomButton;
    public Button playerDetails;

    public string tempRoomName;
    private GameConsole Gameconsole;


    private void Awake()
    {
        lobby = this;
    }

    private void Start()
    {
        //PhotonNetwork.ConnectUsingSettings();
       Gameconsole = GameObject.FindGameObjectWithTag("GameConsole").GetComponent<GameConsole>();
    }
    public void Retry_ConnectToMaster()
    {
        Gameconsole.Print("ReconnectingToTheMaster");
        Debug.Log("ReconnectingToTheMaster");
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("player has connected to the photon master sever");
        Gameconsole.Print("player has connected to the photon master sever");
        PhotonNetwork.AutomaticallySyncScene = true;
        RetryButton.SetActive(false);
        battleButton.SetActive(true);
        CreateRoomButton.interactable  = true;
        JoinRoomButton.interactable = true;
        playerDetails.interactable = true;
    }

    public void OnBattleButtonCliked()
    {
        battleButton.SetActive(false);
        cancelButton.SetActive(true);
        PhotonNetwork.JoinRandomRoom();
        Debug.Log("Trying To Join Random Room");
        Gameconsole.Print("Trying To Join Random Room");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("tried to join a random game but failed there must be no room available");
        Gameconsole.Print("tried to join a random game but failed there must be no room available");
        CreateRoom();
    }

    public void CreateRoom()
    {
        int randomRoomName = Random.Range(0, 1000);
        RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = 10,CleanupCacheOnLeave = true};
        PhotonNetwork.CreateRoom("Room"+ randomRoomName, roomOps);
        Debug.Log("Room Was Created");
        Gameconsole.Print("Room Was Created");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("tried to create a new room but failed , there must already be a room with the same name");
        Gameconsole.Print("tried to create a new room but failed , there must already be a room with the same name");
        CreateRoom();
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("We are in the room");
        Gameconsole.Print("We are in the room");
    }
   
    public void OnCancelButtonClicked()
    {
        Gameconsole.Print("Disconnected From MasterServer");
        cancelButton.SetActive(false);
        battleButton.SetActive(false);
        RetryButton.SetActive(true);
        CreateRoomButton.interactable = false;
        JoinRoomButton.interactable = false;
        playerDetails.interactable = false;
        PhotonNetwork.Disconnect();
    }

    public void NotConnectedToMasterError()
    {
        if(!PhotonNetwork.IsConnected)
        {
            Gameconsole.Print("Not connected to the network!");
        } 
    }

    public override void OnRoomListUpdateReceived()
    {
        Debug.Log(".................");
        foreach (RoomInfo room in PhotonNetwork.GetRoomList())
        {
            Debug.Log("RoomName:" + room.Name);
        }
    }






    public void CreateCustomRoom()
    {
        string tempName = GameObject.FindGameObjectWithTag("RoomNameToCreate").GetComponent<Text>().text;
        if (tempName != null || tempName != "")
        {
            PhotonNetwork.CreateRoom(tempName);
        }
        Debug.Log("Creating Custom Room with Name:" + tempName);
        Gameconsole.Print("Created Custom Room with Name:" + tempName);
    }

    public void JoinCustomRoom()
    {
        tempRoomName = GameObject.FindGameObjectWithTag("RoomNameToJoin").GetComponent<Text>().text;
        if (tempRoomName != null || tempRoomName != "")
        {
            PhotonNetwork.JoinRoom(tempRoomName);
        }
        Debug.Log("Trying To Join Custom Room with Name:" + tempRoomName);
        Gameconsole.Print("Trying To Join Custom Room with Name:" + tempRoomName);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("There is no room available with name: "+ tempRoomName);
        Gameconsole.Print("There is no room available with name: " + tempRoomName);
    }


    public void SetPlayerDetails()
    {
        PhotonNetwork.LocalPlayer.NickName = GameObject.FindGameObjectWithTag("PlayerNameText").GetComponent<Text>().text;
        Gameconsole.Print("PlayerDetailsSet");
         Debug.Log("PlayerDetailsSet");
    }
}
