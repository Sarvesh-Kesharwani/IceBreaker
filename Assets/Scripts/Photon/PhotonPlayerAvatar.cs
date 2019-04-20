using Photon.Pun;
using System.Collections;
using System.IO;
using System.Runtime.CompilerServices;
using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Button = UnityEngine.UIElements.Button;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PhotonPlayerAvatar : MonoBehaviour
{
    private Rigidbody2D myRidigbody;
    private Animator animator;
    private CapsuleCollider2D capsulecollider2d;
    private BoxCollider2D boxcollider;

    private FixedJoystick MoveJoystick;
    private FixedJoystick AimJoystick;

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
    private float ShootWhenJoysticIsAt= 0.2f;

    private PhotonView PV;
    public Slider HealthBar;
    public Slider GunFuelBar;


    public GameObject[] GunCollection;
    public GameObject[] BulletCollection;
    public Transform[] ShotPointCollection;

    private GameObject SelectedGun;
    private GameObject BulletPrefab;
    private Transform ShotPoint;

    public int BulletsCanBeShot = 10;
    private float Health;
    private int GunFuel;
    private GameObject GunSwitchGameObject;
    public GameObject FreezeImage;

    void Start()
    {
        PV = GetComponent<PhotonView>();
        myRidigbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        capsulecollider2d = GetComponent<CapsuleCollider2D>();
        boxcollider = GetComponent<BoxCollider2D>();
        GravityScaleAtStart = myRidigbody.gravityScale;

        AimJoystick = GameObject.FindGameObjectWithTag("AimJoystic").GetComponent<FixedJoystick>();
        MoveJoystick = GameObject.FindGameObjectWithTag("MoveJoystic").GetComponent<FixedJoystick>();
        HealthBar = GameObject.FindGameObjectWithTag("HealthBar").GetComponent<Slider>();
        GunFuelBar = GameObject.FindGameObjectWithTag("GunFuelBar").GetComponent<Slider>();

        Health = 100f;
        GunFuel = 100;
        GameObject.FindGameObjectWithTag("RoomNameText").GetComponent<Text>().text = PhotonNetwork.CurrentRoom.Name;//Updating RoomName Publicly.
        InitializeGuns();
        GunSwitchGameObject = GameObject.FindGameObjectWithTag("GunSwitchButton");


    }

    void Update()
    {

        if (!PV.IsMine)
        {
            return;
        }

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
        ActivateBow();
        IsPFootCollidingArrow();
        UpdateUI();
        AimAnimation();
        GunFuelManager();
            
    }


    private void InitializeGuns()
    {
        SelectedGun = GunCollection[0];
        BulletPrefab = BulletCollection[0];
        ShotPoint = ShotPointCollection[0];
    }
    public void GunSwitch()
    {
        if (GunSwitchGameObject.GetComponentInChildren<Text>().text == "1")
        {
            SelectedGun = GunCollection[1];
            BulletPrefab = BulletCollection[1];
            ShotPoint = ShotPointCollection[1];
            GunSwitchGameObject.GetComponentInChildren<Text>().text = "2";
        }
        else
        {
            SelectedGun = GunCollection[0];
            BulletPrefab = BulletCollection[0];
            ShotPoint = ShotPointCollection[0];
            GunSwitchGameObject.GetComponentInChildren<Text>().text = "1";
        }
    }
    private void ActivateBow()
    {
        //if finger is touching any UI element.
        if (Input.touchCount > 0)
        {
            if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
            {
                Bow.SetActive(false);
            }
            else
            {
                Bow.SetActive(true);
            }
        }
    }
    private void GunFuelManager()
    {
        if (GunFuel <= 0)
        {
            StartCoroutine(AutoFillGunFuel());
        }

        if (GunFuel == 100)
        {
            StopCoroutine(AutoFillGunFuel());
        }
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
            myRidigbody.velocity = new Vector2(myRidigbody.velocity.x, MoveJoystick.Vertical * climbSpeed);
            Debug.Log("1");
            animator.SetBool("Climbing", Mathf.Abs(MoveJoystick.Vertical) > 0.1);//Mathf.Abs(myRidigbody.velocity.y) > Mathf.Epsilon
            Debug.Log("2");
        }
    }

    private void Run()
    {
        Vector2 playerVelocity = new Vector2(MoveJoystick.Horizontal * runSpeed, myRidigbody.velocity.y);
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
        if (MoveJoystick.Vertical >= 0.5)
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
        if (capsulecollider2d.IsTouchingLayers(LayerMask.GetMask("Enemy", "Hazards")) || Health <= 0)
        {
            IsAlive = false;
            animator.SetTrigger("Die");
            FindObjectOfType<SoundManager>().DeathSoundPlay();

            GetComponent<Rigidbody2D>().velocity = DeathKick;
            capsulecollider2d.isTrigger = true;
            StartCoroutine(WaitAfterDie());

            Hashtable temphash = new Hashtable();
            temphash.Add("IsDead", true);
            PhotonNetwork.LocalPlayer.SetCustomProperties(temphash);
            GameObject.FindGameObjectWithTag("ResultManger").GetComponent<ShowGameResultsScript>().ShowGameResults();
        }
    }

    IEnumerator WaitAfterDie()
    {
        yield return new WaitForSecondsRealtime(2);
       // var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
       // SceneManager.LoadScene(currentSceneIndex);
    }
   
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
        Vector2 refrence = new Vector2(AimJoystick.Horizontal * GunAimSpeed, AimJoystick.Vertical * GunAimSpeed);
        float rotAngle = Mathf.Atan2(refrence.y, refrence.x) * Mathf.Rad2Deg;
        SelectedGun.transform.rotation = Quaternion.Euler(0f, 0f, rotAngle);
    }

    private float TimebtwShots = 0;
    public float StartTimebtwShots;


    [PunRPC]
    void InstantiateLocalBulletRPC()
    {
        Instantiate(BulletPrefab, ShotPoint.position, SelectedGun.transform.rotation);
    }

    private void InstantiateBullets()//////////////////////////////////////////////////////////////////////////////
    {
        StopCoroutine(AutoFillGunFuel());
        if (GunFuel > 0)
        {
            if (TimebtwShots <= 0)
            {
                if (Mathf.Abs(AimJoystick.Horizontal) >= ShootWhenJoysticIsAt ||
                    Mathf.Abs(AimJoystick.Vertical) >= ShootWhenJoysticIsAt)
                {
                    InstantiateLocalBulletRPC();
                    TimebtwShots = StartTimebtwShots;
                    SoundManager.PlaySound("ShotSound");
                    GunFuel -= BulletsCanBeShot;
                }
            }
            else
            {
                TimebtwShots -= Time.deltaTime;
            }
        }
    }
    private void AimAnimation()
    {
        if (Mathf.Abs(AimJoystick.Horizontal) > 0 || Mathf.Abs(AimJoystick.Vertical) > 0)
        {
            SelectedGun.SetActive(true);
            animator.SetFloat("Aim", 1);
        }
        else
        {
            SelectedGun.SetActive(false);
            animator.SetFloat("Aim", 0);
        }
    }

    public void UpdateUI()
    {
        //CoinText.text = "" + Coins;
        HealthBar.value = Health;
        GunFuelBar.value = GunFuel;
    }

    public void GetDamage(float Damage)
    {
        Health -= Damage;
    }

    IEnumerator AutoFillGunFuel()
    {
        while (GunFuel != 100)
        {
            GunFuel += BulletsCanBeShot;
            yield return new WaitForSecondsRealtime(3);
        }
    }

    public void Freeze()
    {
        myRidigbody.bodyType = RigidbodyType2D.Static;
        animator.SetBool("Frozen",true);
        GameObject.FindGameObjectWithTag("UnFreezeButton").SetActive(true);
    }

    public void UnFreeze()
    {
        Instantiate(FreezeImage, GameObject.FindGameObjectWithTag("Player").transform.position, Quaternion.identity);
        myRidigbody.bodyType = RigidbodyType2D.Dynamic;
        animator.SetBool("Frozen", false);
    }
}


