using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class ShowGameResultsScript : MonoBehaviour
{
    public GameObject GameResultPanel;
    public Text[] PlayerText;
    public Text[] PlayerHitCount;
    public GameObject[] Playerempty;
    public GameObject WinText;
    public GameObject LooseText;

    public void ShowGameResults()
    {
        if (PhotonNetwork.PlayerList.Length == 1)
        {
            Playerempty[0].SetActive(true);
            ShowGameResultRPC();
            if ((bool)PhotonNetwork.LocalPlayer.CustomProperties["IsDead"] == true)
            {
                LooseText.SetActive(true);
            }
            else
            {
                WinText.SetActive(true);
            }
        }

        if (PhotonNetwork.PlayerList.Length == 2)
        {

            Playerempty[0].SetActive(true);
            Playerempty[1].SetActive(true);
            ShowGameResultRPC();
            if ((bool)PhotonNetwork.LocalPlayer.CustomProperties["IsDead"] == true)
            {
                LooseText.SetActive(true);
            }
            else
            {
                WinText.SetActive(true);
            }
        }

        if (PhotonNetwork.PlayerList.Length > 2)
        {
            Playerempty[0].SetActive(true);
            Playerempty[1].SetActive(true);
            Playerempty[2].SetActive(true);
            ShowGameResultLocally();
        }
    }

    private void ShowGameResultLocally()
    {
        GameResultPanel.SetActive(true);
        int i = 0;
        foreach (var player in PhotonNetwork.PlayerList)
        {
            PlayerText[i].text = player.NickName;
            PlayerHitCount[i].text = player.CustomProperties["hits"].ToString();
            i++;
        }
        LooseText.SetActive(true);
    }

    [PunRPC]
    void ShowGameResultRPC()
    {
        GameResultPanel.SetActive(true);
        int i = 0;
        foreach (var player in PhotonNetwork.PlayerList)
        {
            PlayerText[i].text = player.NickName;
            PlayerHitCount[i].text = player.CustomProperties["hits"].ToString();
            i++;
        }
    }
}
