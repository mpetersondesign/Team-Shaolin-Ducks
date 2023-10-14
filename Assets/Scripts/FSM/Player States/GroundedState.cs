using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GroundedState : State
{
    private PlayerController Player;
    protected override void OnStateInitialize()
    {
        Player = GetComponent<PlayerController>();
    }
    public override void Enter(string previous_key, State previous_state)
    {
        if (Player.IsJumping == false && Player.RB.velocity.y > 0)
            Player.RB.velocity = new Vector2(Player.RB.velocity.x, 0);

        Player.SlingshotSpent = false;
        Player.IsSlinging = false;
        Player.PC.size = Player.DefaultColliderSize;
        Player.PA.Play("Idle");
        Player.IsSlung = false;
    }

    public override void Exit(string next_key, State next_state)
    {

    }

    public override void Tick()
    {
        // Moved fallowing code to SlingingState
        /*
        if (Player.IsSlung && Player.RB.velocity.y > GetComponent<SlingingState>().BounceThreshold)
        {
            Player.IsSlung = false;
            var groundNormal = Physics2D.Raycast(transform.position, (Vector2)transform.position + Player.RB.velocity, 0.1f, Player.TerrainLayer).normal;

            //We want this to be a reflection of the normal being hit
            Vector2 bounceDirection = Math.Reflect(Player.RB.velocity, groundNormal);
            float bouncePower = (Mathf.Abs(Player.RB.velocity.y) * GetComponent<SlingingState>().BounceForce);
            Mathf.Clamp(bouncePower, 0, GetComponent<SlingingState>().MaxBounceForce);
            Player.RB.AddForce(bounceDirection * bouncePower, ForceMode2D.Impulse);
        }*/

        if (Player.PI.IsPressed(PlayerInputs.PlayerAction.Jump))
        {
            //Replaced with IsJumping check in AerialState
            //Player.IsJumping = true;

            // Jump
            Player.RB.velocity = new Vector2(Player.RB.velocity.x, Player.JumpStrength);
        }

        if(!Player.IsGrounded && !Player.IsSlinging)
        {
            Player.SM.ChangeState("Aerial");
        }

        if (Player.PI.IsPressed(PlayerInputs.PlayerAction.Dash))
            Player.IsDashing = true;
        else
        {
            Player.IsDashing = false;
            Player.DashBurstSpent = false;
        }
    }
}
