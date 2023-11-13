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
    public float BounceThreshold = 4f;
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

        if(Input.GetMouseButtonDown(0))
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

            Player.SM.ChangeState("Aerial");
        }
    }
}
