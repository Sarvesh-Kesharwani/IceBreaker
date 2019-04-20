using Photon.Pun;
using System.IO;
using UnityEngine;

public class PhotonPlayer : MonoBehaviour
{

    private PhotonView PV;
    private GameObject myAvatar;

    void Start()
    {
        PV = GetComponent<PhotonView>();
        int spawnPicker = Random.Range(0, GameSetup.GS.spawnPoints.Length);
        if (PV.IsMine)
        {
            myAvatar = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonPlayerAvatar"),
                                                                 GameSetup.GS.spawnPoints[spawnPicker].position,
                                                                 GameSetup.GS.spawnPoints[spawnPicker].rotation, 0);
        }

    }
}
