using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowFootCollisionCheck : MonoBehaviour
{
    public GameObject PlayerFoot;


    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerFoot.GetComponent<BoxCollider2D>().isTrigger = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "ArrowCollisionTag")
        {
            PlayerFoot.GetComponent<BoxCollider2D>().isTrigger = false;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        PlayerFoot.GetComponent<BoxCollider2D>().isTrigger = true;
    }
}
