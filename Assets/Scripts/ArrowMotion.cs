using UnityEngine;

public class ArrowMotion : MonoBehaviour
{
    private Rigidbody2D rigidbody2D;

    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // transform.forward = Vector3.Slerp(transform.forward, rigidbody2D.velocity.normalized, Time.deltaTime);
    }


}
