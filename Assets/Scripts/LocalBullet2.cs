using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class LocalBullet2 : MonoBehaviour
{
    private float speed = 200;
    private float distance = 0.4f;
    public LayerMask WhatIsSolid;
    private PhotonView PV;
    public bool CanDamage = true;

    public ParticleSystem NormalcolliderEffect;
    public float BulletDamage = 5f;

    public void Start()
    {
        PV = GetComponent<PhotonView>();
    }

    private void Update()
    {
        MoveBullet();
    }

    void DestroyProjectile()
    {
        Destroy(gameObject);
    }

   
    void PlayNormalEffect()
    {
        Instantiate(NormalcolliderEffect, transform.position, Quaternion.identity);
        DestroyImmediate(NormalcolliderEffect);
    }

    void MoveBullet()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, transform.right, distance, WhatIsSolid);
        if (hitInfo.collider != null)
        {
            if (hitInfo.collider.CompareTag("Player"))
            {
                if (!hitInfo.collider.gameObject.GetComponent<PhotonView>().IsMine)
                {
                    if (CanDamage == true) //Using CanDamage To Shoot Once.
                    {
                        hitInfo.transform.gameObject.GetComponent<PhotonPlayerAvatar>().GetDamage(BulletDamage);
                        CanDamage = false;
                    }
                    PlayNormalEffect();
                    DestroyProjectile();
                }
            }

            if (hitInfo.collider.CompareTag("Ground") || hitInfo.collider.CompareTag("Bullet") ||
                hitInfo.collider.CompareTag("FrozenIce"))
            {
                DestroyProjectile();
                PlayNormalEffect();
            }
        }

        gameObject.GetComponent<Rigidbody2D>().AddRelativeForce(new Vector3(1, 0, 0) * speed * Time.deltaTime);
    }
}
