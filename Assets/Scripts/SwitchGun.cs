using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchGun : MonoBehaviour
{
    public void OnSwitchButtonClickede()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PhotonPlayerAvatar>().GunSwitch();
    }
}
