using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Empty now, might move wall sliding lodgic from ArialState to here for better code orginization
// Currently commented class is just a copy of AerialState

public class WallGrabingState : State
{
    private PlayerController Player;

    protected override void OnStateInitialize()
    {
        Player = GetComponent<PlayerController>();
        //Machine.SetCurrentState(Key);
    }

    public override void Enter(string previous_key, State previous_state)
    {
        Player.RB.isKinematic = true;
        Player.RB.velocity = new Vector2(0, 0);

        //Debug.Log("Test");
    }

    public override void Exit(string next_key, State next_state)
    {
        Player.RB.isKinematic = false;

        //Debug.Log("Test");
    }

    public override void Tick()
    {
        if(Player.PI.OnPress(PlayerInputs.PlayerAction.Jump))
        {
            Player.IsJumping = true;
            Player.RB.velocity = new Vector2(Player.AgainstWall * Player.JumpStrength / -2, Player.JumpStrength / 2);
            Player.SM.ChangeState("Aerial");
        }

        if(!Player.PI.IsPressed(PlayerInputs.PlayerAction.Interact))
        {
            Player.SM.ChangeState("Aerial");
            return;
        }

        /*
        //If we're sliding against a wall && the player is pressing against it
        if ((Player.PI.RawInput.x > 0 && Player.AgainstWall == 1) ||
            (Player.PI.RawInput.x < 0 && Player.AgainstWall == -1))
        {
            //Placeholder animation (we don't want to be in our slung animation)
            Player.PA.Play("Idle");

            //Disable the sling if we were in one
            Player.IsSlung = false;

            //Slow the player's descent by half
            Player.RB.velocity = new Vector2(Player.RB.velocity.x, Player.RB.velocity.y / 2);
        }

        //If we become grounded while aerial
        if (Player.IsGrounded)
            Player.SM.ChangeState("Grounded"); //Switch to grounded

        //If we press our dash key in the air and we haven't already slung ourselves
        if (Player.PI.IsPressed(PlayerInputs.PlayerAction.Dash) && !Player.SlingshotSpent)
        {
            //Start slinging
            Player.IsSlinging = true;
            Player.SM.ChangeState("Slinging");
        }
        */
    }
}