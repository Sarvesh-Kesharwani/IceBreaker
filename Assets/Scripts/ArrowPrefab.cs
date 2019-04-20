using UnityEngine;

public class ArrowPrefab : MonoBehaviour
{

    private void OnCollisionEnter2D(Collision2D collidedTile)
    {
        if (collidedTile.gameObject.tag == "Ground")
        {
            GetComponentInParent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            GetComponentInParent<Rigidbody2D>().gravityScale = 0;
        }
    }
}