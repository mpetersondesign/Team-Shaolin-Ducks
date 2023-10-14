using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlingingState : State
{
    private PlayerController Player;
    public GameObject SlingIndicator;
    public float SlowPercent;
    public float LaunchPower;
    public float BounceThreshold;
    public float BounceForce;
    public float MaxBounceForce;

    protected override void OnStateInitialize()
    {
        Player = GetComponent<PlayerController>();
        //Machine.SetCurrentState(Key);
    }

    public override void Enter(string previous_key, State previous_state)
    {
        Player.PC.size = Player.SlingshotColliderSize;
        Time.timeScale = SlowPercent;
        SlingIndicator.gameObject.SetActive(true);
        Player.PA.Play("Slung");
    }

    public override void Exit(string next_key, State next_state)
    {
        Time.timeScale = 1f;
        SlingIndicator.gameObject.SetActive(false);
    }

    public override void Tick()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;
        Vector2 targetDir = (Vector2)mousePos - (Vector2)SlingIndicator.transform.position;
        SlingIndicator.transform.up = targetDir;

        if(Input.GetMouseButtonDown(0))
        {
            Time.timeScale = 1f;
            Player.RB.velocity = Vector3.zero;
            Player.RB.angularVelocity = 0;
            Player.RB.AddForce((Vector2)SlingIndicator.transform.up.normalized * LaunchPower, ForceMode2D.Impulse);
            Player.SlingshotSpent = true;
            Player.IsSlinging = false;
            Player.IsSlung = true;
        }

        if (Player.IsGrounded)
        {
            // Bounce mechanic
            if (-Player.RB.velocity.y > GetComponent<SlingingState>().BounceThreshold)
            {
                Player.IsSlung = false;

                // for some reason this doesn't work (is returning (0,0) so for a temporary solution we will assume a level ground i.e. normal = (0,1)
                //var groundNormal = Physics2D.Raycast(transform.position, (Vector2)transform.position + Player.RB.velocity, 0.1f, Player.TerrainLayer).normal;
                var groundNormal = new Vector2(0, 1);

                //We want this to be a reflection of the normal being hit
                Vector2 bounceDirection = Math.Reflect(Player.RB.velocity, groundNormal);
                bounceDirection.Normalize();
                float bouncePower = Mathf.Abs(Player.RB.velocity.y);
                Mathf.Clamp(bouncePower, 0, GetComponent<SlingingState>().MaxBounceForce);
                Player.RB.velocity = bounceDirection * bouncePower;

                Player.IsGrounded = false;
                GetComponent<StateMachine>().ChangeState("Aerial");
            }
            else
            {
                GetComponent<StateMachine>().ChangeState("Grounded");
                Player.IsSlinging = false;
            }
        }

        if (!Player.PI.IsPressed(PlayerInputs.PlayerAction.Dash))
            Player.IsSlinging = false;
    }
}
