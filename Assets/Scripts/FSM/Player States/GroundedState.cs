using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundedState : State
{
    private PlayerController Player;
    private PlayerEffectsActivator Effects;

    protected override void OnStateInitialize()
    {
        Player = GetComponent<PlayerController>();
        Effects = GetComponent<PlayerEffectsActivator>();
    }
    public override void Enter(string previous_key, State previous_state)
    {
        if (Player.IsJumping == false && Player.RB.velocity.y > 0)
            Player.RB.velocity = new Vector2(Player.RB.velocity.x, 0);

        Player.SlingshotSpent = false;
        Player.IsSlinging = false;
        Player.PC.size = Player.DefaultColliderSize;
        Player.PA.Play("Grounded");
        Player.IsSlung = false;
        GetComponent<AudioCue>().PlayAudioCue(1); // plays player landing sfx, can be moved to events later
    }

    public override void Exit(string next_key, State next_state)
    {
        // in case of dashing off a cliff
        Effects.StopDashEffects();
    }

    public override void Tick()
    {
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
            Player.IsJumping = true;
            Player.RB.velocity = new Vector2(Player.RB.velocity.x, Player.JumpStrength);
            Debug.Log("Jump");
        }

        if (Player.PI.IsPressed(PlayerInputs.PlayerAction.Dash))
        {
            Player.IsDashing = true;
            Effects.StartDashEffects();
        }
        else
        {
            Player.IsDashing = false;
            Player.DashBurstSpent = false;
            Effects.StopDashEffects();
        }

    }
}
