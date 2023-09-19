using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Parameters")]
    [Tooltip("The base speed the player can move")]
    public float MoveSpeed;
    [Tooltip("The running speed the player can move")]
    public float RunSpeed;
    [Tooltip("The current magnitude representing the speed the player is currently moving at")]
    public float MoveForce;
    [Tooltip("The current acceleration force applied to the character's horizontal movement")]
    public float CurrentAccel;
    [Tooltip("The force at which the player will accelerate")]
    public float AccelAmount;
    [Tooltip("The force at which the player will deaccelerate")]
    public float DeaccelAmount;
    [Tooltip("The force at which the player will jump")]
    public float JumpStrength;
    [Tooltip("The direction the player is currently facing")]
    public float FacingDirection;
    [Tooltip("The number of jumps the player has")]
    public int JumpCount = 2;
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
    public bool JumpThisFrame;
    public bool IsGrounded;
    public bool IsSkidding;
    public bool SlingshotSpent;
    public bool IsSlinging;
    public bool IsSlung;

    public Vector2 GroundCheckSize;
    public Vector2 GroundCheckPos;
    public Vector2 ExternalForces;

    public LayerMask TerrainLayer;
    
    public BoxCollider2D PC;
    public StateMachine SM;
    public PlayerInputs PI;
    public Rigidbody2D RB;
    public Animator PA;

    void Awake()
    {
        PA = GetComponentInChildren<Animator>();
        PC = GetComponent<BoxCollider2D>();
        PI = GetComponent<PlayerInputs>();
        SM = GetComponent<StateMachine>();
        RB = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        GroundedCheck();
        StateManagement();
        CurrentVelocity = RB.velocity;
    }

    private void StateManagement()
    {
        if(!IsSlinging && !IsGrounded)
        {
            SM.ChangeState("Aerial");
        }
    }

    private void GroundedCheck()
    {
        IsGrounded = Physics2D.BoxCast((Vector2)transform.position + GroundCheckPos,
                                       GroundCheckSize, 0, Vector3.down, 0f);
    }

    void FixedUpdate()
    {
        MovePlayer();
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
            if (Mathf.Sign(RB.velocity.x) != Mathf.Sign(H_Axis) && H_Axis != 0)
                IsSkidding = true;
            else
                IsSkidding = false;
        }

        if (IsDashing && DashBurstSpent == false)
        {
            AddForces += (FacingDirection * Vector2.right) * DashBurstForce;
            DashBurstSpent = true;
        }

        RB.AddForce(movement * Vector2.right);
        RB.AddForce(AddForces);
        AddForces = Vector3.zero;
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
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)RB.velocity);
    }

    void OnGUI()
    {
        GUI.Box(new Rect(10, 10, 200, 25), "State: " + SM.CurrentState.Key);
    }
}
