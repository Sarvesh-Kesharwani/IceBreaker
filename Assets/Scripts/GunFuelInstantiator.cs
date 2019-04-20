using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class GunFuelInstantiator : MonoBehaviour
{
    public GameObject GunFuelPrefab;

    public Transform[] SpawnPoints;

    public void SpawnAtRandom()
    {
        StartCoroutine(Spawn());
    }

    private IEnumerator Spawn()
    {
        while (PhotonNetwork.IsConnected)
        {
            Debug.Log("GunFuelSpawned");
            Instantiate(GunFuelPrefab, SpawnPoints[Random.Range(0, SpawnPoints.Length)]);
            yield return new WaitForSecondsRealtime(15);
        }
        
    }
}
