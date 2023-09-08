using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float AccelSpeed = 5f;
    public float DeaccelSpeed = 5f;
    public float MoveSpeed = 5f;
    public float DashSpeed = 10f;
    public float JumpStrength = 8f;

    public bool IsGrounded;
    public bool IsJumping;
    public bool IsDashing;

    public float GroundCheckRadius;
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
    }

    private void StateManagement()
    {
        if (IsGrounded && !IsJumping)
            SM.ChangeState("Grounded");
        else
            SM.ChangeState("Aerial");
    }

    private void GroundedCheck()
    {
        IsGrounded = Physics2D.OverlapCircle((Vector2)transform.position + GroundCheckPos, GroundCheckRadius, TerrainLayer);
    }

    void FixedUpdate()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        if(IsDashing)
        {
            if (Mathf.Abs(RB.velocity.x) < DashSpeed)
                RB.AddForce(new Vector2(PI.RawInput.x * MoveSpeed, 0));
        }
        else
        {
            if (Mathf.Abs(RB.velocity.x) < MoveSpeed)
                RB.AddForce(new Vector2(PI.RawInput.x * MoveSpeed, 0));
        }
    }

    private void OnDrawGizmos()
    {
        Color GroundCheckColor = Color.red;
        GroundCheckColor.a = 0.25f;
        Gizmos.color = GroundCheckColor;
        Gizmos.DrawSphere((Vector2)transform.position + GroundCheckPos, GroundCheckRadius);
    }

    void OnGUI()
    {
        GUI.Box(new Rect(10, 10, 200, 25), "State: " + SM.CurrentState.Key);
    }
}
