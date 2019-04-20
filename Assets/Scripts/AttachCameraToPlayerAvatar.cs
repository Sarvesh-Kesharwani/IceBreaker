using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Cinemachine;
using UnityEngine;

public class AttachCameraToPlayerAvatar : MonoBehaviour
{
    void Start()
    {
        GameObject temp = GameObject.FindGameObjectWithTag("PhotonCamera");
        Debug.Log("Attaching camera to player");
        temp.GetComponentInChildren<CinemachineVirtualCamera>().Follow = this.transform;
    }
}
