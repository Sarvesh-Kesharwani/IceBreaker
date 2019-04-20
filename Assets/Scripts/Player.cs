using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    private Rigidbody2D myRidigbody;
    private Animator animator;
    private CapsuleCollider2D capsulecollider2d;
    private BoxCollider2D boxcollider;

    public FixedJoystick Movejoystick;
    public FixedJoystick AimJoystic;

    private Vector3 touchPosition;
    private Vector3 InitialTouchPosition;
    public GameObject Bow;

    private float runSpeed = 10f;
    private float jumpSpeed = 12f;
    private float climbSpeed = 4f;
    private float GravityScaleAtStart;
    private bool IsAlive = true;
    private Vector2 DeathKick = new Vector2(0f, 25f);

    private Touch touchShoot;

    public GameObject PlayerFoot;
    public GameObject CheckCollisionWithArrow_Ground;

    public float GunAimSpeed;
    public GameObject Gun;
    public GameObject Bullet;
    public Transform ShotPoint;
    private float ShootWhenJoysticIsAt = 0.2f;

    public ParticleSystem particleSystem;

    public int Coins = 0;
    private float Health = 100;
    private float GunFule = 100;

    public TextMeshProUGUI CoinText;

    void Start()
    {
        myRidigbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        capsulecollider2d = GetComponent<CapsuleCollider2D>();
        boxcollider = GetComponent<BoxCollider2D>();
        GravityScaleAtStart = myRidigbody.gravityScale;
    }

    void Update()
    {
        if (!IsAlive)
        {
            return;
        }
        Run();
        Jump();
        GunAim();
        InstantiateBullets();
        FlipSprite();
        ClimbLadder();
        Die();

        //if finger is touching any UI element.
        if (Input.touchCount > 0)
        {
            if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
            {
                Bow.SetActive(false);
            }
            else
            {
                //Bow.SetActive(true);
            }
        }
        IsPFootCollidingArrow();
        UpdateUI();
        AimAnimation();
    }


    private void ClimbLadder()
    {
        if (!capsulecollider2d.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            animator.SetBool("Climbing", false);
            myRidigbody.gravityScale = GravityScaleAtStart;
            return;
        }
        else
        {
            myRidigbody.gravityScale = 0f;
            myRidigbody.velocity = new Vector2(myRidigbody.velocity.x, Movejoystick.Vertical * climbSpeed);
            Debug.Log("1");
            animator.SetBool("Climbing", Mathf.Abs(Movejoystick.Vertical) > 0.1);//Mathf.Abs(myRidigbody.velocity.y) > Mathf.Epsilon
            Debug.Log("2");

        }
    }




    private void Run()
    {
        Vector2 playerVelocity = new Vector2(Movejoystick.Horizontal * runSpeed, myRidigbody.velocity.y);
        myRidigbody.velocity = playerVelocity;
        animator.SetBool("Running", Mathf.Abs(playerVelocity.x) > Mathf.Epsilon);
    }

    private void FlipSprite()
    {
        if (Mathf.Abs(myRidigbody.velocity.x) > Mathf.Epsilon)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRidigbody.velocity.x), 1);
        }
    }

    public void Jump()
    {
        /* if (Input.touchCount == 1)
         {
             Touch touch1 = Input.GetTouch(0);

             if (touch1.position.x > Screen.width / 2 && boxcollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
             {
                 myRidigbody.velocity += new Vector2(0f, jumpSpeed);
                 animator.SetBool("Jump", true);
             }
         }
         else if (Input.touchCount > 1)
         {
             Touch touch2 = Input.GetTouch(1);

             if (touch2.position.x > Screen.width / 2 && boxcollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
             {
                 myRidigbody.velocity += new Vector2(0f, jumpSpeed);
                 animator.SetBool("Jump", true);
             }
         }*/
        if (Movejoystick.Vertical >= 0.5)
        {
            if (CheckCollisionWithArrow_Ground.GetComponent<BoxCollider2D>().IsTouchingLayers(LayerMask.GetMask("Ground"))
                        || CheckCollisionWithArrow_Ground.GetComponent<BoxCollider2D>().IsTouchingLayers(LayerMask.GetMask("Arrow")))
            {
                myRidigbody.velocity += new Vector2(0f, jumpSpeed);
                animator.SetBool("Jump", true);
                SoundManager.PlaySound("JumpSound");
            }
        }
    }

    private void Die()
    {
        if (capsulecollider2d.IsTouchingLayers(LayerMask.GetMask("Enemy", "Hazards")))
        {
            IsAlive = false;
            animator.SetTrigger("Die");
            FindObjectOfType<SoundManager>().DeathSoundPlay();

            GetComponent<Rigidbody2D>().velocity = DeathKick;
            capsulecollider2d.isTrigger = true;
            StartCoroutine(WaitAfterDie());
        }
    }

    IEnumerator WaitAfterDie()
    {
        yield return new WaitForSecondsRealtime(2);
        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);

    }
    // void GetTouchPosition()
    // {
    //     if (Input.touchCount > 0)
    //     {
    //         touchShoot = Input.GetTouch(0);
    //         touchPosition = Camera.main.ScreenToWorldPoint(touchShoot.position);
    //         touchPosition.z = 0f;
    //         if (touchShoot.phase == TouchPhase.Began)
    //         {
    //             InitialTouchPosition = touchPosition;

    //             ArrowInstance = Instantiate(ArrowPrefab) as GameObject;
    //             ArrowInstance.transform.position = ShootingPoint.transform.position;
    //             arrowReleased = false;
    //         }
    //         else if (touchShoot.phase == TouchPhase.Moved)
    //         {
    //             Aim();
    //         }
    //         else if (touchShoot.phase == TouchPhase.Ended)
    //         {
    //             arrowReleased = true;
    //         }
    //     }

    // }


    // void Aim()
    // {
    //     var offset = new Vector2(touchPosition.x - ArrowInstance.transform.position.x,
    //         touchPosition.y - ArrowInstance.transform.position.y);
    //     float angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg * AimSenstivity;
    //     ArrowInstance.transform.rotation = Quaternion.Euler(0, 0, angle);


    // }

    //void ShootArrowInstance()
    // {
    //     if (arrowReleased == true)
    //     {
    //         ArrowInstance.GetComponent<Rigidbody2D>().AddRelativeForce(new Vector3(0, 1, 0) * moveSpeed * Time.deltaTime);
    //     }
    // }

    private void IsPFootCollidingArrow()
    {
        if (CheckCollisionWithArrow_Ground.GetComponent<BoxCollider2D>()
            .IsTouchingLayers(LayerMask.GetMask("ArrowCollisionCheck")))
        {
            PlayerFoot.GetComponent<BoxCollider2D>().isTrigger = false;
        }
        else
        {
            PlayerFoot.GetComponent<BoxCollider2D>().isTrigger = true;
        }
    }

    private void GunAim()
    {
        Vector2 refrence = new Vector2(AimJoystic.Horizontal * GunAimSpeed, AimJoystic.Vertical * GunAimSpeed);
        float rotAngle = Mathf.Atan2(refrence.y, refrence.x) * Mathf.Rad2Deg;
        Gun.transform.rotation = Quaternion.Euler(0f, 0f, rotAngle);
        // Debug.Log("H:" + AimJoystic.Horizontal + "V:" +AimJoystic.Vertical);
    }


    private float TimebtwShots = 0;
    public float StartTimebtwShots;
    private void InstantiateBullets()
    {
        if (TimebtwShots <= 0)
        {
            if (Mathf.Abs(AimJoystic.Horizontal) >= 0.2 || Mathf.Abs(AimJoystic.Vertical) >= 0.2)
            {
                particleSystem.Play();
            }
            else
            {
                particleSystem.Stop();
            }

            if (Mathf.Abs(AimJoystic.Horizontal) >= ShootWhenJoysticIsAt || Mathf.Abs(AimJoystic.Vertical) >= ShootWhenJoysticIsAt)
            {
                particleSystem.Stop();
                Instantiate(Bullet, ShotPoint.position, Gun.transform.rotation);
                TimebtwShots = StartTimebtwShots;
                SoundManager.PlaySound("ShotSound");
            }
        }
        else
        {
            TimebtwShots -= Time.deltaTime;
        }
    }

    private void AimAnimation()
    {
        if (Mathf.Abs(AimJoystic.Horizontal) > 0 || Mathf.Abs(AimJoystic.Vertical) > 0)
        {
            Gun.SetActive(true);
            animator.SetFloat("Aim", 1);
        }
        else
        {
            Gun.SetActive(false);
            animator.SetFloat("Aim", 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Coin"))
            Coins += 10;
    }

    public void UpdateUI()
    {
        CoinText.text = "" + Coins;
    }
}
