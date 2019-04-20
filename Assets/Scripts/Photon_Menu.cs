using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using Hashtable = ExitGames.Client.Photon.Hashtable;


public class Photon_Menu : MonoBehaviour
{
    public List<GameObject> gameobjectToHide;
    public List<GameObject> gameobjectToShow;

    public void Photon_ShowPanels()
    {
        for (int i = 0; i < gameobjectToShow.Count; i++)
        {
            gameobjectToShow[i].SetActive(true);
        }
    }

    public void Photon_HidePanels()
    {
        for (int i = 0; i < gameobjectToHide.Count; i++)
        {
            gameobjectToHide[i].SetActive(false);
        }
    }

    public void Photon_LoadMainMenu()
    {
        // GameConsole.Print("Left the Room");
        KillPlayer();
        PhotonNetwork.LeaveRoom();
        DestroyRoomController();
        SceneManager.LoadScene(0);
        // PhotonNetwork.Disconnect();
    }

    [PunRPC]
    void DestroyRoomController()
    {
        PhotonNetwork.Destroy(PhotonView.Find(1));
    }

    void KillPlayer()
    {
        Hashtable temphash = new Hashtable();
        temphash.Add("IsDead", true);
        PhotonNetwork.LocalPlayer.SetCustomProperties(temphash);
    }
}
        
