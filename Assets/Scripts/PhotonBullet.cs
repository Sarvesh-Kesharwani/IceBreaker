using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PhotonBullet : MonoBehaviour
{
    private float speed;
   // public float lifeTime;
    private float distance = 0.4f;
    public LayerMask WhatIsSolid;
   // public GameObject FrozenIce;
    //public GameObject ExplosionPraticleEffect;
   // public GameObject destroyEffect;
    private PhotonView PV;
    public bool CanDamage = true;


    public void Start()
    {
        PV = GetComponent<PhotonView>();
       // Invoke("DestroyProjectile", lifeTime);
    }

    private void Update()
    {
        if (!PV.IsMine)
        {
            return;
        }
        RPC_Shooting();
    }

    void DestroyProjectile()
    {
        // Instantiate(destroyEffect, transform.position, Quaternion.identity);
        /*Instantiate(ExplosionPraticleEffect,transform.position,Quaternion.identity);
         DestroyImmediate(ExplosionPraticleEffect);*/
        PhotonNetwork.Destroy(this.gameObject);
    }

    [PunRPC]
    void RPC_Shooting()
    {
       RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, transform.right, distance, WhatIsSolid);
        if (hitInfo.collider != null)
        {
            if (hitInfo.collider.CompareTag("Player"))
            {
               // Debug.Log("HIT!");
                if (!hitInfo.transform.gameObject.GetComponent<PhotonView>().IsMine)
                {
                    if (CanDamage == true)//Using CanDamage To Shoot Once.
                    {
                        hitInfo.transform.gameObject.GetComponent<PhotonPlayerAvatar>().GetDamage(5);
                        CanDamage = false;
                    }
                    //Debug.Log("Shooted Enemy");
                    DestroyProjectile();
                }
            }
            if (hitInfo.collider.CompareTag("Ground"))
            {
                DestroyProjectile();
            }
        }
        gameObject.GetComponent<Rigidbody2D>().AddRelativeForce(new Vector3(1, 0, 0) * speed * Time.deltaTime);

    }
}
