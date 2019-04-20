using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnFreezeButton : MonoBehaviour
{
    private int count = 0;
    public void OnTap()
    {
        count++;
        if (count >= 6)
        {
            UnFreezeRequestToPlayer();
        }
    }
    public void UnFreezeRequestToPlayer()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PhotonPlayerAvatar>().UnFreeze();
        this.gameObject.SetActive(false);
    }
}
