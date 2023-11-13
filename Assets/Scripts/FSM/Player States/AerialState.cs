using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AerialState : State
{
    private PlayerController Player;
    private PlayerEffectsActivator Effects;
    private AudioCue audioCue;
    private string wallSlideKey = "wallSlide";
    private static bool hasDoubleJump;

    // for debug info
    public static bool HasDoubleJump { get => hasDoubleJump; }

    protected override void OnStateInitialize()
    {
        Player = GetComponent<PlayerController>();
        Effects = GetComponent<PlayerEffectsActivator>();
        audioCue = GetComponent<AudioCue>();
        Machine.SetCurrentState(Key);
    }

    public override void Enter(string previous_key, State previous_state)
    {
        //Debug.Log("Enter Aerial");
        hasDoubleJump = true;
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


            }
            else
            {
                Effects.StopWallSlideEffects();

                if (audioCue != null)
                    audioCue.StopAudioCue(wallSlideKey, 0.2f);
                
                Player.IsWallSliding = false;
            }
        }

        if (!Player.IsWallSliding && !Player.IsSlinging && !Player.IsSlung)
        {
            Player.PA.Play("Aerial");
            Player.PA.SetFloat("YVel", Player.RB.velocity.y);
        }

        //If we become grounded while aerial
        if (Player.IsGrounded)
        {
            if (!Player.IsSlung)
            {
                if (!Player.IsJumping)
                    Player.SM.ChangeState("Grounded"); //Switch to grounded
            }
            else
            {
                AfterSlingBounce();
            }
        }

        if (hasDoubleJump && Player.PI.OnPress(PlayerInputs.PlayerAction.Jump))
        {
            Player.RB.velocity = new Vector2(Player.RB.velocity.x, Player.JumpStrength / 2);
            hasDoubleJump = false;
            Debug.Log("Double Jump");
        }

        if (Player.PI.IsPressed(PlayerInputs.PlayerAction.Dash))
        {
            Debug.Log("Test");
        }

        //If we press our dash key in the air and we haven't already slung ourselves
        if(Player.PI.IsPressed(PlayerInputs.PlayerAction.Dash) && !Player.SlingshotSpent)
        {
            //Start slinging
            Player.IsSlinging = true;
            Player.SM.ChangeState("Slinging");
        }
    }

    private void AfterSlingBounce()
    {
        float speed = Player.RB.velocity.magnitude;

        if (speed <= GetComponent<SlingingState>().BounceThreshold)
        {
            Player.SM.ChangeState("Grounded");
            return;
        }

        Player.IsSlung = false;

        //temporaraly replaced with reducing speed only in perp direction so there is
        //more horrizontal motion to counteract horrizontal acceleration dampaning
        //can be replaced back when horrizontal acceleration dampaning is reduced/fixed
        //speed -= GetComponent<SlingingState>().BounceThreshold;

        // for some reason this doesn't work (is returning (0,0) so for a temporary solution we will assume a level ground i.e. normal = (0,1)
        //var groundNormal = Physics2D.Raycast(transform.position, (Vector2)transform.position + Player.RB.velocity, 0.1f, Player.TerrainLayer).normal;
        var groundNormal = new Vector2(0, 1);
        //We want this to be a reflection of the normal being hit
        Vector2 bounceDirection = Math.Reflect(Player.RB.velocity, groundNormal);
        bounceDirection.Normalize();
        Player.RB.velocity = bounceDirection * speed;
        Player.RB.velocity -= groundNormal * GetComponent<SlingingState>().BounceThreshold;
    }
}
