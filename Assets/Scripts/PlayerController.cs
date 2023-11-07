using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Parameters")]
    [SerializeField]
    [Tooltip("The base speed the player can move")]
    public float MoveSpeed;
    [SerializeField]
    [Tooltip("The running speed the player can move")]
    public float RunSpeed;
    [SerializeField]
    [Tooltip("The current magnitude representing the speed the player is currently moving at")]
    public float MoveForce;
    [SerializeField]
    [Tooltip("The force at which the player will accelerate")]
    public float AccelAmount;
    [SerializeField]
    [Tooltip("The force at which the player will deaccelerate")]
    public float DeaccelAmount;
    [SerializeField]
    [Tooltip("The force at which the player will jump")]
    public float JumpStrength;
    [SerializeField]
    [Tooltip("The number of jumps the player has")]
    public int MaxJumps = 2;
    [Tooltip("The current acceleration force applied to the character's horizontal movement")]
    public float CurrentAccel;
    [Tooltip("The direction the player is currently facing")]
    public float FacingDirection;
    [Tooltip("The number of jumps the player has currently left to expend")]
    public int JumpsLeft;

    public Vector2 CurrentVelocity;

    [Header("Dash Parameters")]
    public float DashBurstForce;
    public float CurrentDashBurstForce;
    public float DashBurstForceDecay;
    public bool DashBurstSpent;

    [Header("Forces")]
    [Tooltip("Any additional forces that need to act upon the rigidbody")]
    public Vector2 AddForces;
    [Tooltip("The power to which we magnitude the forces applied")]
    public float ForcePower;
    public float KnockbackPower = 5f;

    [Header("Conditionals")]
    public bool CanMove = false;
    public bool LockAnimation = false;
    public bool NoClip = false;
    public bool IsDashing = false;
    public bool IsJumping = false;
    public bool IsGrounded;
    public bool IsSkidding;
    public bool IsSlinging;
    public bool IsSlung;
    public bool SlingshotSpent;
    public int AgainstWall = 0;

    [Header("Collider Modifications")]
    public Vector2 DefaultColliderSize;
    public Vector2 DefaultColliderOffset;
    public Vector2 SlingshotColliderSize;
    public Vector2 SlingshotColliderOffset;
    public Vector2 SlidingColliderSize;
    public Vector2 SlidingColliderOffset;
    public float WallDetectorLength;

    [Header("Ground Check Modifications")]
    public Vector2 GroundCheckSize;
    public Vector2 GroundCheckPos; 
    public float GroundVelocityThreashold = 0.01f;

    public Vector2 ExternalForces;

    public LayerMask TerrainLayer;

    public BoxCollider2D PC;
    public StateMachine SM;
    public PlayerInputs PI;
    public Rigidbody2D RB;
    public Animator PA;
    public GameObject PSP; //PlayerSprite Parent
    public GameObject PS; //PlayerSprite

    void Awake()
    {
        //Initialize references
        PA = GetComponentInChildren<Animator>();
        PC = GetComponent<BoxCollider2D>();
        PI = GetComponent<PlayerInputs>();
        SM = GetComponent<StateMachine>();
        RB = GetComponent<Rigidbody2D>();

        //Set defaults
        DefaultColliderSize = PC.size;
        DefaultColliderOffset = PC.offset;
        SlingshotColliderSize = new Vector2(PC.size.x, PC.size.y / 2);
    }

    void Update()
    {
        if (CanMove)
            PA.SetFloat("Movement", Mathf.Abs(Input.GetAxisRaw(PI.H_AxisName)));
        else
            PA.SetFloat("Movement", 0);

        if (PI.RawInput.x != 0)
            PSP.transform.localScale = new Vector3(PI.RawInput.x, PSP.transform.localScale.y, PSP.transform.localScale.z);

        //Ground/Wall Detection
        GroundedCheck();
        WallDetection();

        //Manage our states per-frame if needed
        StateManagement();

        //Update our inspector readout for velocity
        CurrentVelocity = RB.velocity;
    }

    private void WallDetection()
    {
        //1 = right
        //-1 = left
        // 0 = none

        if (Physics2D.Raycast(transform.position, Vector2.right, WallDetectorLength, TerrainLayer))
            AgainstWall = 1;
        else if (Physics2D.Raycast(transform.position, Vector2.left, WallDetectorLength, TerrainLayer))
            AgainstWall = -1;
        else
            AgainstWall = 0;
    }

    private void StateManagement()
    {
        //If we've pressed dash
        if (PI.IsPressed(PlayerInputs.PlayerAction.Dash))
            IsDashing = true;
        else
        {
            //We're not dashing
            IsDashing = false;

            //And our dash burst should reset
            DashBurstSpent = false;
        }

        // Moved to GroundedState (on Tick)
        // so that all state changes happen in their own state
        //
        //If we're not slinging, and we're not on the ground
        //if(!IsSlinging && !IsGrounded)
        //    SM.ChangeState("Aerial"); //Switch to aerial
    }

    private void GroundedCheck()
    {
        //Set our grounded bool to be the result of this boxcast per-frame
        //Ignore check if we have a non-negligable upwords velocity
        if (RB.velocity.y <= GroundVelocityThreashold)
            IsGrounded = Physics2D.BoxCast((Vector2)transform.position + GroundCheckPos,
                                           GroundCheckSize, 0, Vector3.down, 0.1f);
    }

    void FixedUpdate()
    {
        if(CanMove)
            MovePlayer();
    }

    private void MovePlayer()
    {
        //Interpret our H_Axis variable from our input script
        float H_Axis = Input.GetAxisRaw(PI.H_AxisName);

        //Identify what speed we want to be moving at
        float targetSpeed;

        //MoveForce is a variable that will tell us the current speed of our player
        MoveForce = Mathf.Abs(RB.velocity.x);

        //If we're dashing
        if(IsDashing)
            //Then our target speed becomes our RunSpeed
            targetSpeed = H_Axis * RunSpeed;
        else //Otherwise
            //Our target speed is our default MoveSpeed
            targetSpeed = H_Axis * MoveSpeed;

        //Our speed difference is the difference between our current speed and our target
        float speedDiff = targetSpeed - RB.velocity.x;

        //If the ABS of targetSpeed is greater than 0, apply Accel, otherwise Deaccel.
        CurrentAccel = (Mathf.Abs(targetSpeed) > 0.01f) ? AccelAmount : DeaccelAmount;

        //I don't remember what this does lol but it's very important
        float movement = Mathf.Pow(Mathf.Abs(speedDiff) * CurrentAccel, ForcePower) * Mathf.Sign(speedDiff);

        //Decide what direction we're facing based on movement affecting us
        if (RB.velocity.x > 0.1f)
            FacingDirection = 1;
        if (RB.velocity.x < -0.1f)
            FacingDirection = -1;

        //If we're grounded
        if(IsGrounded)
        {
            //Refresh our amount of jumps
            JumpsLeft = MaxJumps;

            //Determine whether we're moving in an opposing direction to our current velocity
            if (Mathf.Sign(RB.velocity.x) != Mathf.Sign(H_Axis) && H_Axis != 0)
                IsSkidding = true;
            else
                IsSkidding = false;
        }

        //Dash burst is kinda broken so please fix this lol
        if (IsDashing && DashBurstSpent == false)
        {
            AddForces += (FacingDirection * Vector2.right) * DashBurstForce;
            DashBurstSpent = true;
        }

        //Move our player with our input
        RB.AddForce(movement * Vector2.right);

        //Additional forces acting on our player
        RB.AddForce(AddForces);
        AddForces = Vector3.zero;

        //Set jumping to false if we aren't
        if (RB.velocity.y < 0)
            IsJumping = false;
    }

    private void OnDrawGizmos()
    {
        BoxCollider2D BC = GetComponent<BoxCollider2D>();

        //Draw main collider
        Color colliderColor = Color.green;
        colliderColor.a = 0.5f;
        Gizmos.color = colliderColor;
        Gizmos.DrawCube(new Vector3(BC.transform.position.x + BC.offset.x,
                                    BC.transform.position.y + BC.offset.y,
                                    BC.transform.position.z),
                                    GetComponent<BoxCollider2D>().size);

        //Draw ground check
        Color groundcheckColor = Color.red;
        groundcheckColor.a = 0.5f;
        Gizmos.color = groundcheckColor;
        Gizmos.DrawCube((Vector2)transform.position + GroundCheckPos, GroundCheckSize);

        //Draw velocity ray
        Color velocityColor = Color.yellow;
        Gizmos.color = velocityColor;
        if(RB != null)
            Gizmos.DrawLine(transform.position, transform.position + (Vector3)RB.velocity);
    }

    void OnGUI()
    {
        GUI.Box(new Rect(10, 10, 200, 25), "State: " + SM.CurrentState.Key);
        GUI.Box(new Rect(10, 35, 200, 25), "Dashing: " + IsDashing);
        GUI.Box(new Rect(10, 60, 200, 25), "Jumping: " + IsJumping);
        GUI.Box(new Rect(10, 85, 200, 25), "Slinging: " + IsSlinging);
        GUI.Box(new Rect(10, 110, 200, 25), "Slung: " + IsSlung);
    }
}
