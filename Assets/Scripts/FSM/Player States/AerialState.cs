using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AerialState : State
{
    private PlayerController Player;
    private PlayerEffectsActivator Effects;
    private AudioCue audioCue;
    private string wallSlideKey = "wallSlide";

    protected override void OnStateInitialize()
    {
        Player = GetComponent<PlayerController>();
        Effects = GetComponent<PlayerEffectsActivator>();
        audioCue = GetComponent<AudioCue>();
        Machine.SetCurrentState(Key);
    }

    public override void Enter(string previous_key, State previous_state)
    {

    }

    public override void Exit(string next_key, State next_state)
    {
        if(next_state != this)
        {
            // to avoid conditions where wall slide check is skipped due to slinging or grounding
            if (Effects != null)
            {
                Effects.StopWallSlideEffects();
            }

            if (audioCue != null)
                audioCue.StopAudioCue(wallSlideKey, 0.2f);
        }
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
                Player.IsWallSliding = true;

                //Placeholder animation (we don't want to be in our slung animation)
                Player.PA.Play("Wall Slide");
                
                //Disable the sling if we were in one
                Player.IsSlung = false;

                //Slow the player's descent by half
                Player.RB.velocity = new Vector2(Player.RB.velocity.x, Player.RB.velocity.y / 2);

                if (Effects != null)
                {
                    Effects.StartWallSlideEffects(Player.AgainstWall);
                }

                if (audioCue != null)
                    audioCue.StartAudioCue(2, wallSlideKey);

                if(Player.PI.IsPressed(PlayerInputs.PlayerAction.Jump))
                {
                    Player.RB.velocity = new Vector2((Player.PI.RawInput.x * -(Player.JumpStrength)), Player.JumpStrength);
                    Player.IsWallSliding = false;
                    Effects.StopWallSlideEffects();
                    audioCue.StopAudioCue(wallSlideKey, 0.2f);
                    audioCue.PlayAudioCue(3);
                    if (!Player.IsSlung)
                        Player.PA.Play("Aerial");
                }
            }
            else
            {
                Player.IsWallSliding = false;
                Effects.StopWallSlideEffects();
                if(!Player.IsSlung)
                    Player.PA.Play("Aerial");
                if (audioCue != null)
                    audioCue.StopAudioCue(wallSlideKey, 0.2f);
                
            }
        }

        if (!Player.IsWallSliding && !Player.IsSlinging && !Player.IsSlung)
        {
            Player.PA.Play("Aerial");
            Player.PA.SetFloat("YVel", Player.RB.velocity.y);
        }

        //If we become grounded while aerial
        if (Player.IsGrounded && !Player.IsJumping)
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
