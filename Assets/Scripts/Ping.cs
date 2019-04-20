using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class Ping : MonoBehaviour
{
    private Text pingtext;

    void Start()
    {
        pingtext = GetComponentInChildren<Text>();
        DontDestroyOnLoad(gameObject);
    }
    void Update()
    {
        pingtext.text = PhotonNetwork.GetPing().ToString();
    }
}
