using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Parameters")]
    [SerializeField]
    [Tooltip("The base speed the player can move")]
    private float MoveSpeed;
    [SerializeField]
    [Tooltip("The running speed the player can move")]
    private float RunSpeed;
    [SerializeField]
    [Tooltip("The current magnitude representing the speed the player is currently moving at")]
    private float MoveForce;
    [SerializeField]
    [Tooltip("The force at which the player will accelerate")]
    private float AccelAmount;
    [SerializeField]
    [Tooltip("The force at which the player will deaccelerate")]
    private float DeaccelAmount;
    [SerializeField]
    [Tooltip("The force at which the player will jump")]
    private float JumpStrength;
    [SerializeField]
    [Tooltip("The number of jumps the player has")]
    private int MaxJumps = 2;

    //[Tooltip("The current acceleration force applied to the character's horizontal movement")]
    private float CurrentAccel;
    //[Tooltip("The direction the player is currently facing")]
    private float FacingDirection;
    //[Tooltip("The number of jumps the player has currently left to expend")]
    private int JumpsLeft;

    [Header("Dash Parameters")]
    public float DashBurstForce;
    public float CurrentDashBurstForce;
    public float DashBurstForceDecay;
    public bool DashBurstSpent;

    [Header("Forces")]
    [Tooltip("The force of the user's input we'll apply to the rigidbody")]
    public Vector2 Velocity;
    [Tooltip("Any additional forces that need to act upon the rigidbody")]
    public Vector2 AddForces;
    [Tooltip("The power to which we magnitude the forces applied")]
    public float ForcePower;
    public float KnockbackPower = 5f;

    [Header("Conditionals")]
    public bool CanMove = false;
    public bool LockAnimation = false;
    public bool NoClip = false;

    private bool isDashing = false;
    private bool isJumping = false;
    // currently unused
    // private bool jumpThisFrame;
    private bool isGrounded;
    private bool isSkidding;

    public Vector2 GroundCheckSize;
    public Vector2 GroundCheckPos;
    public Vector2 ExternalForces;

    public LayerMask TerrainLayer;

    private BoxCollider2D pc;
    private StateMachine sm;
    private PlayerInputs pi;
    private Rigidbody2D rb;
    private Animator pa;

    public bool IsDashing { get => isDashing; }
    public bool IsJumping { get => isJumping; }
    // currently unused
    // public bool JumpThisFrame { get => jumpThisFrame; }
    public bool IsGrounded { get => isGrounded; }
    public bool IsSkidding { get => isSkidding; }
    public BoxCollider2D PC { get => pc; }
    public StateMachine SM { get => sm; }
    public PlayerInputs PI { get => pi; }
    public Rigidbody2D RB { get => rb; }
    public Animator PA { get => pa; }

    void Awake()
    {
        pa = GetComponentInChildren<Animator>();
        pc = GetComponent<BoxCollider2D>();
        pi = GetComponent<PlayerInputs>();
        sm = GetComponent<StateMachine>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        GroundedCheck();
        StateManagement();
    }

    private void StateManagement()
    {
        if (IsGrounded && !IsJumping)
            SM.ChangeState("Grounded");
        else
            SM.ChangeState("Aerial");

        if (PI.IsPressed(PlayerInputs.PlayerAction.Dash))
            isDashing = true;
        else
        {
            isDashing = false;
            DashBurstSpent = false;
        }
    }

    private void GroundedCheck()
    {
        isGrounded = Physics2D.BoxCast((Vector2)transform.position + GroundCheckPos,
                                       GroundCheckSize, 0, Vector3.down, 0f);
    }

    void FixedUpdate()
    {
        MovePlayer();
    }

    public void MoveLeft()
    {

    }

    public void MoveRight()
    {

    }

    public void Jump()
    {
        if (JumpsLeft > 0)
        {
            --JumpsLeft;
            isJumping = true;
            rb.velocity = new Vector2(rb.velocity.x, JumpStrength);
        }
    }

    private void MovePlayer()
    {
        float H_Axis = Input.GetAxisRaw(PI.H_AxisName);
        float targetSpeed;
        MoveForce = Mathf.Abs(RB.velocity.x);

        if(IsDashing)
            targetSpeed = H_Axis * RunSpeed;
        else
            targetSpeed = H_Axis * MoveSpeed;

        float speedDiff = targetSpeed - RB.velocity.x;

        //If the ABS of targetSpeed is greater than 0, apply Accel, otherwise Deaccel.
        CurrentAccel = (Mathf.Abs(targetSpeed) > 0.01f) ? AccelAmount : DeaccelAmount;

        float movement = Mathf.Pow(Mathf.Abs(speedDiff) * CurrentAccel, ForcePower) * Mathf.Sign(speedDiff);

        if (RB.velocity.x > 0.1f)
            FacingDirection = 1;
        if (RB.velocity.x < -0.1f)
            FacingDirection = -1;

        if(IsGrounded)
        {
            JumpsLeft = MaxJumps;
            if (Mathf.Sign(RB.velocity.x) != Mathf.Sign(H_Axis) && H_Axis != 0)
                isSkidding = true;
            else
                isSkidding = false;
        }

        if (IsDashing && DashBurstSpent == false)
        {
            AddForces += (FacingDirection * Vector2.right) * DashBurstForce;
            DashBurstSpent = true;
        }

        RB.AddForce(movement * Vector2.right);
        RB.AddForce(AddForces);
        AddForces = Vector3.zero;

        if (rb.velocity.y < 0)
            isJumping = false;
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
    }

    void OnGUI()
    {
        GUI.Box(new Rect(10, 10, 200, 25), "State: " + SM.CurrentState.Key);
    }
}
