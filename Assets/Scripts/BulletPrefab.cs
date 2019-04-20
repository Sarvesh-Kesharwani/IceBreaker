using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPrefab : MonoBehaviour
{
    public float speed;
    public float lifeTime;
    public float distance;
    public LayerMask WhatIsSolid;
    public GameObject FrozenIce;


    public GameObject ExplosionPraticleEffect;

    public GameObject destroyEffect;
    public void Start()
    {
        Invoke("DestroProjectile",lifeTime);
    }

    private void Update()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position,transform.right,distance,WhatIsSolid);
        if (hitInfo.collider != null)
        {
            if (hitInfo.collider.CompareTag("Enemy"))
            {
                    Instantiate(FrozenIce, hitInfo.transform.position, Quaternion.identity);
                    SoundManager.PlaySound("BulletClashSound");
                    hitInfo.rigidbody.bodyType = RigidbodyType2D.Static;
                //Take Damage or Freeze
            }
            DestroProjectile();
        }
        //transform.Translate(transform.right * speed * Time.deltaTime);
        gameObject.GetComponent<Rigidbody2D>().AddRelativeForce(new Vector3(1, 0, 0) * speed * Time.deltaTime);
        
    }

    void DestroProjectile()
    {
       // Instantiate(destroyEffect, transform.position, Quaternion.identity);
        /*Instantiate(ExplosionPraticleEffect,transform.position,Quaternion.identity);
         DestroyImmediate(ExplosionPraticleEffect);*/
        Destroy(gameObject);
    }

    
}
