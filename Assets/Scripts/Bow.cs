using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour
{
    // TrajectoryPoint and Ball will be instantiated
    public GameObject TrajectoryPointPrefeb;
    public GameObject BallPrefb;
    private GameObject ball;

    private bool isPressed, isBallThrown;
    private bool ballthrown;
    private int numOfTrajectoryPoints = 30;
    private List<GameObject> trajectoryPoints;

    public GameObject PlayerGameObject;

    private Touch touchAim;
    private Vector3 touchInitPos;
    private Vector3 touchPosition;

    private float ArrowMass_TrajectoryGap = 20f;//increse it to decrease the gap between trajectories.
    private float ArrowShootSpeed = 17f; //it is devided with addforce method.

    public GameObject jumpbutton, controljoystick;

    //---------------------------------------    
    void Start()
    {
        trajectoryPoints = new List<GameObject>();
        isPressed = isBallThrown = false;
        ballthrown = false;
        //   TrajectoryPoints are instatiated
        for (int i = 0; i < numOfTrajectoryPoints; i++)
        {
            GameObject dot = (GameObject)Instantiate(TrajectoryPointPrefeb);
            dot.GetComponent<Renderer>().enabled = false;
            trajectoryPoints.Insert(i, dot);
        }
    }
    //---------------------------------------    
    void Update()
    {
        GetTouchRefrence();

        if (ballthrown)
        {
            ball = null;
            ballthrown = false;
        }

        if (touchAim.phase == TouchPhase.Began)
        {
            touchInitPos = touchPosition;
            isPressed = true;
            if (!ball) //checking if ball exists or not!!!!!!!!!!!!!!!!!!!
            {
                createBall();
            }
        }
        if (touchAim.phase == TouchPhase.Moved)
        {
            ///just setting angle of the canon.
            Vector3 vel = GetForceFrom(touchInitPos, touchPosition);
            float angle = Mathf.Atan2(vel.y, vel.x) * Mathf.Rad2Deg;
            transform.eulerAngles = new Vector3(0, 0, angle);
            ball.transform.eulerAngles = new Vector3(0, 0, angle);
            setTrajectoryPoints(transform.position, vel / (ball.GetComponent<Rigidbody2D>().mass * ArrowMass_TrajectoryGap));
            //move arrow with player untill it is shot;
            ball.transform.position = PlayerGameObject.transform.position;

            //cheking and disabling contols if finger gets over the other UIs.
            SetOtherUI(false);

        }
        if (touchAim.phase == TouchPhase.Ended)
        {
            isPressed = false;
            if (!isBallThrown)
            {
                throwBall();
            }

            for (int i = 0; i < numOfTrajectoryPoints; i++)
            {
                trajectoryPoints[i].GetComponent<Renderer>().enabled = false;
            }

            SetOtherUI(true);
        }
        // when mouse button is pressed, cannon is rotated as per mouse movement and projectile trajectory path is displayed.
    }
    //---------------------------------------    
    // Following method creates new ball
    //---------------------------------------    
    private void createBall()
    {
        ball = (GameObject)Instantiate(BallPrefb);
        Vector3 pos = transform.position;
        pos.z = 1;
        ball.transform.position = pos;
        ball.SetActive(false);
    }
    //---------------------------------------    
    // Following method gives force to the ball
    //---------------------------------------    
    private void throwBall()
    {
        ball.SetActive(true);
        ball.GetComponent<Rigidbody2D>().gravityScale = 1;
        ball.GetComponent<Rigidbody2D>().AddForce(-GetForceFrom(touchInitPos, touchPosition) / ArrowShootSpeed, ForceMode2D.Impulse);
        isBallThrown = false;
        ballthrown = true;
    }
    //---------------------------------------    
    // Following method returns force by calculating distance between given two points
    //---------------------------------------    
    private Vector2 GetForceFrom(Vector3 fromPos, Vector3 toPos)
    {
        return (new Vector2(toPos.x, toPos.y) - new Vector2(fromPos.x, fromPos.y));
    }
    //---------------------------------------    
    // Following method displays projectile trajectory path. It takes two arguments, start position of object(ball) and initial velocity of object(ball).
    //---------------------------------------    
    void setTrajectoryPoints(Vector3 pStartPosition, Vector3 pVelocity)
    {
        float velocity = Mathf.Sqrt((pVelocity.x * pVelocity.x) + (pVelocity.y * pVelocity.y));
        float angle = Mathf.Rad2Deg * (Mathf.Atan2(pVelocity.y, pVelocity.x));
        float fTime = 0;

        fTime += 0.005f;// no use of it, just initializes 
        for (int i = 0; i < numOfTrajectoryPoints; i++)
        {
            float dx = -velocity * fTime * Mathf.Cos(angle * Mathf.Deg2Rad);
            float dy = -velocity * fTime * Mathf.Sin(angle * Mathf.Deg2Rad) - (Physics2D.gravity.magnitude * fTime * fTime / 2.0f);
            Vector3 pos = new Vector3(pStartPosition.x + dx, pStartPosition.y + dy, 2);
            trajectoryPoints[i].transform.position = pos;
            trajectoryPoints[i].GetComponent<Renderer>().enabled = true;
            trajectoryPoints[i].transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(pVelocity.y - (Physics.gravity.magnitude) * fTime, pVelocity.x) * Mathf.Rad2Deg);
            fTime += 0.025f; // decrease it to decrease gap between trajectories and decrase the arc of curve.

            if (trajectoryPoints[i].GetComponent<CircleCollider2D>().IsTouchingLayers(LayerMask.GetMask("Climbing")))
            {
                Debug.Log("disabling");
                for (int j = i; j < numOfTrajectoryPoints; j++)
                    trajectoryPoints[i].GetComponent<Renderer>().enabled = false;
            }
            else
            {
                Debug.Log("enabling");

                for (int j = i; j < numOfTrajectoryPoints; j++)
                    trajectoryPoints[i].GetComponent<Renderer>().enabled = true;
            }
        }
    }

    void GetTouchRefrence()
    {
        if (Input.touchCount > 0)
        {
            touchAim = Input.GetTouch(0);

            touchPosition = Camera.main.ScreenToWorldPoint(touchAim.position);
            touchPosition.z = 0f;
        }
    }

    void SetOtherUI(bool state)
    {
        jumpbutton.SetActive(state);
        controljoystick.SetActive(state);
    }
}


