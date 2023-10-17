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

        /* Moved to SlingingState (on Exit)
        Player.SlingshotSpent = false;
        Player.IsSlinging = false;
        Player.IsSlung = false;*/

        Player.PC.size = Player.DefaultColliderSize;
        Player.PA.Play("Grounded");
    }

    public override void Exit(string next_key, State next_state)
    {

    }

    public override void Tick()
    {
        // Moved to SlingingState
        /* Bounce mechanic (someone take a look at this pls :3 )
        if (Player.IsSlung && Player.RB.velocity.y > GetComponent<SlingingState>().BounceThreshold)
        {
            Player.IsSlung = false;
            var groundNormal = Physics2D.Raycast(transform.position, (Vector2)transform.position + Player.RB.velocity, 0.1f, Player.TerrainLayer).normal;

            //We want this to be a reflection of the normal being hit
            Vector2 bounceDirection = (Vector2.up);
            float bouncePower = (Mathf.Abs(Player.RB.velocity.y) * GetComponent<SlingingState>().BounceForce);
            Mathf.Clamp(bouncePower, 0, GetComponent<SlingingState>().MaxBounceForce);
            Player.RB.AddForce(bounceDirection * bouncePower, ForceMode2D.Impulse);
        } */
        
        if (Player.PI.IsPressed(PlayerInputs.PlayerAction.Jump))
        {
            //Moved to AerialState (on Enter)
            //Player.IsJumping = true;

            Player.RB.velocity = new Vector2(Player.RB.velocity.x, Player.JumpStrength);
            Player.SM.ChangeState("Aerial");
        }

        //Correct me if I'm wrong but shouldn't IsSlining only be true when we are in SlingingState?
        //Why would we need to check as we are in GroundedState?
        //if (!Player.IsGrounded && !Player.IsSlinging)
        if (!Player.IsGrounded)
        {
            Player.SM.ChangeState("Aerial");
        }

        if (Player.PI.IsPressed(PlayerInputs.PlayerAction.Dash))
        {
            Player.IsDashing = true;
        }
        else
        {
            Player.IsDashing = false;
            Player.DashBurstSpent = false;
        }

    }
}
