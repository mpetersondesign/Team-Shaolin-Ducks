using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AerialState : State
{
    private PlayerController Player;
    private PlayerEffectsActivator Effects;

    protected override void OnStateInitialize()
    {
        Player = GetComponent<PlayerController>();
        Effects = GetComponent<PlayerEffectsActivator>();
        Machine.SetCurrentState(Key);
    }

    public override void Enter(string previous_key, State previous_state)
    {

    }

    public override void Exit(string next_key, State next_state)
    {

    }

    public override void Tick()
    {
        //If the player is falling and not currently slinging
        if(Player.RB.velocity.y < 0 && !Player.IsSlinging)
        {
            //If we're sliding against a wall && the player is pressing against it
            if((Player.PI.RawInput.x > 0 && Player.AgainstWall == 1) ||
               (Player.PI.RawInput.x < 0 && Player.AgainstWall == -1))
            {
                //Placeholder animation (we don't want to be in our slung animation)
                Player.PA.Play("Idle");

                //Disable the sling if we were in one
                Player.IsSlung = false;

                //Slow the player's descent by half
                Player.RB.velocity = new Vector2(Player.RB.velocity.x, Player.RB.velocity.y / 2);

                if (Effects != null)
                {
                    Effects.StartWallSlideEffects(Player.AgainstWall);
                }
            }
            else if(Effects != null)
            {
                Effects.StopWallSlideEffects();
            }
        }

        //If we become grounded while aerial
        if (Player.IsGrounded)
            Player.SM.ChangeState("Grounded"); //Switch to grounded

        //If we press our dash key in the air and we haven't already slung ourselves
        if(Player.PI.IsPressed(PlayerInputs.PlayerAction.Dash) && !Player.SlingshotSpent)
        {
            //Start slinging
            Player.IsSlinging = true;
            Player.SM.ChangeState("Slinging");
        }
    }
}
