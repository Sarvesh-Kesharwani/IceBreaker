using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class LocalBullet1 : MonoBehaviour
{
    private float speed = 200;
    private float distance = 0.4f;
    public LayerMask WhatIsSolid;
    private PhotonView PV;
    public bool CanDamage = true;

    public ParticleSystem ExplosionPraticleEffect;
    public ParticleSystem NormalcolliderEffect;
    public ParticleSystem IceBoostPS;

    private ParticleSystem PlasmaInstance;
    private ParticleSystem NormalEffectInstance;


    public float BulletDamage = 10f;

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

    void PlayFreezeEffect()
    {
        PlasmaInstance = Instantiate(ExplosionPraticleEffect, transform.position, Quaternion.identity);
        Destroy(PlasmaInstance,1);
    }

    void PlayNormalEffect()
    {
        NormalEffectInstance = Instantiate(NormalcolliderEffect, transform.position, Quaternion.identity);
        Destroy(NormalEffectInstance,1);
    }

    void MoveBullet()
    {
        IceBoostPS.Play();
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
                    IceBoostPS.Stop();
                    PlayFreezeEffect();
                    GameObject.FindGameObjectWithTag("Player").GetComponent<PhotonPlayerAvatar>().Freeze();
                    DestroyProjectile();
                }
            }

            if (hitInfo.collider.CompareTag("Ground") || hitInfo.collider.CompareTag("Bullet") ||
                hitInfo.collider.CompareTag("FrozenIce"))
            {
                PlayNormalEffect();
                DestroyProjectile();
            }
        }

        gameObject.GetComponent<Rigidbody2D>().AddRelativeForce(new Vector3(1, 0, 0) * speed * Time.deltaTime);
    }
}
