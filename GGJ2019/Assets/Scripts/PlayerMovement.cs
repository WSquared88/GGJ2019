using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    float PlayerSpeed = 1.0f;
    [SerializeField]
    float friction = .9f;
    [SerializeField]
    float maxMag = 5f;
    [SerializeField]
    float accelerationFactor = 1.2f;
    [SerializeField]
    float decelerationFactor = 1.1f;
    [SerializeField]
    float maxAccel = 3.0f;
    [SerializeField]
    float minAccel = 0.3f;
    [SerializeField]
    float complexTimer = 0.25f;
    [SerializeField]
    float simpleTimer = .5f;
    [SerializeField]
    Camera PlayerCam;

    private bool ComplexGoal;
    private bool Rotating;
    private float CurrentTimer;
   // private float StartTimer;
    private Vector3 CurrentGoal;
    private Vector3 LastGood;
    private Vector3 Goal;
    private Vector3 SumMove;
    private Vector3 CurrMove;
    private float acceleration = .3f;
    private Vector3 PerspGoalFB;
    private Vector3 PerspGoalLR;
    


  //  private float ComplexTimer;
  //  private float SimpleTimer;
    


    // Use this for initialization
    void Start ()
    {
        Debug.Assert(PlayerSpeed > 0.0f + float.Epsilon, "The player speed is less than zero! This is going to cause weird problems!");
        // If no camera, default to main camera
        if (!PlayerCam)
        {
            PlayerCam = Camera.main;
        }
        CurrMove = new Vector3(0, 0, 0);
        CurrentGoal = new Vector3(0, 0, 0);
        LastGood = new Vector3(0, 0, 0);
        CurrentTimer = 0;
        ComplexGoal = false;
        Rotating = false;
        SumMove = new Vector3(0, 0, 0);
        complexTimer = .5f;
        simpleTimer = .75f;
	}

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + transform.forward* 20);
    }

    // Update is called once per frame
    void Update ()
    {
        Vector3 forwardToChange = transform.forward;
        PerspGoalFB = new Vector3(0, 0, 0);
        PerspGoalLR = new Vector3(0, 0, 0);
        Vector3 move_direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;

        Vector3 camFor = PlayerCam.transform.forward;
        Vector3 camRight = PlayerCam.transform.right;


        //W - Positive Horizontal -> Cam.forward
        //S - Neg Horizontal -> Cam.back
        //A - Neg vertical -> Cam.left
        //D - Pos Vertical -> Cam.Right

        PerspGoalFB = camFor * Input.GetAxis("Horizontal");


        PerspGoalLR = camRight * Input.GetAxis("Vertical");

        Goal = Vector3.Normalize(PerspGoalFB + PerspGoalLR);
        Debug.Log(Goal.magnitude);
        if (Goal.magnitude != 0) 
        {
            Rotating = true;
            CurrentTimer = 0;
        }
        if (Rotating)
        {
            CurrentTimer += Time.deltaTime;
            float animTimer = simpleTimer;
            if (PerspGoalLR.magnitude >= 0 + float.Epsilon && PerspGoalFB.magnitude >= 0 + float.Epsilon)
            {
                ComplexGoal = true;
                animTimer = complexTimer;
            }
            forwardToChange = Vector3.Slerp(transform.forward, Goal, (CurrentTimer / animTimer));
            if (CurrentTimer / animTimer >= 1)
            {
                //animation is done
                Rotating = false;
            }
        }

        



        if (move_direction.magnitude != 0)
        {
            acceleration = acceleration * accelerationFactor;
            if(acceleration > maxAccel)
            {
                acceleration = maxAccel;
            }
        }
        else
        {
            acceleration = acceleration * decelerationFactor;
            if(acceleration < minAccel)
            {
                acceleration = minAccel;
            }
        }

        move_direction = PlayerCam.transform.TransformDirection(move_direction);
        move_direction.y = 0;
        Rigidbody rigid_body = GetComponent<Rigidbody>();
        Debug.Assert(rigid_body != null, "The Rigidbody is missing from the player!");
        SumMove = CurrMove + move_direction;
        //rigid_body.MovePosition(transform.position + move_direction * Time.deltaTime * PlayerSpeed);
        CurrMove = Vector3.ClampMagnitude(SumMove, maxMag);

        rigid_body.MovePosition(transform.position + CurrMove * Time.deltaTime * PlayerSpeed);
        if (forwardToChange != new Vector3(0, 0, 0)) { transform.forward = forwardToChange.normalized; }
        // Debug.Log(transform.forward);
       // this.transform.LookAt(forwardToChange);

    }
}
