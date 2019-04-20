using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Enemy_Movement : MonoBehaviour
{
    private Rigidbody2D Myrigidbody;
    private float Runspeed = 5f;

    private Animator animator;
    void Start()
    {
        Myrigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (IsFacingRight())
        {
            Myrigidbody.velocity = new Vector2(Runspeed,0f);
        }
        else
        {
            Myrigidbody.velocity = new Vector2(-Runspeed,0f);
        }
    }

    private bool IsFacingRight()
    {
        return transform.localScale.x > 0;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        transform.localScale = new Vector2(-Mathf.Sign(Myrigidbody.velocity.x),1f);
    }

}
