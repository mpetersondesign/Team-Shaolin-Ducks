using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
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
    public float HomingDistance = 4f;
    public GameObject LockedOnTarget;
    public Vector2 targetDir;

    protected override void OnStateInitialize()
    {
        Player = GetComponent<PlayerController>();
        Machine.SetCurrentState(Key);
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)targetDir * HomingDistance);
    }

    public override void Tick()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f; 
        var EnemyHit = Physics2D.Raycast(transform.position, targetDir, 4f);
        
        if(EnemyHit.collider != null && EnemyHit.collider.gameObject.tag == "Enemy")
        {
            var nme = EnemyHit.collider.gameObject;
            LockedOnTarget = nme;
            targetDir = nme.transform.position - transform.position;
        }
        else
        {
            LockedOnTarget = null;
            targetDir = (Vector2)mousePos - (Vector2)SlingIndicator.transform.position;
        }

        SlingIndicator.transform.up = targetDir;

        if(Player.IsSlinging && Input.GetMouseButtonDown(0))
        {
            // temp location?
            if(GetComponent<AudioCue>() != null)
                GetComponent<AudioCue>().PlayAudioCue();

            Player.RB.velocity = Vector2.zero;
            Time.timeScale = 1f;
            Player.RB.angularVelocity = 0;
            Player.RB.AddForce((Vector2)SlingIndicator.transform.up.normalized * LaunchPower, ForceMode2D.Impulse);
            Player.SlingshotSpent = true;
            Player.IsSlinging = false;
            Player.IsSlung = true;
            if(GetComponent<PlayerEffectsActivator>() != null)
            {
                SendMessage("OnSling", targetDir);
            }
        }

        if (Player.IsGrounded)
        {
            // Old Bounce Physics
            /*if (Player.IsSlung && -Player.RB.velocity.y > GetComponent<SlingingState>().BounceThreshold)
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
                //GetComponent<StateMachine>().ChangeState("Aerial");
            }*/

            //New Version
            /*
            if(Player.IsSlinging && Player.IsGrounded)
            {
                GetComponent<StateMachine>().ChangeState("Grounded");
                Player.IsSlinging = false;
                return;
            }    
            

            float speed = Player.RB.velocity.magnitude;
            

            float parallel = Math.Dot(Player.RB.velocity,)
            if (Player.IsSlung && speed > GetComponent<SlingingState>().BounceThreshold)
            {
                
            }
            else
            {
                GetComponent<StateMachine>().ChangeState("Grounded");
                Player.IsSlinging = false;
            }*/
        }

        //if (!Player.PI.IsPressed(PlayerInputs.PlayerAction.Dash))
        //    Player.IsSlinging = false;
    }
}
