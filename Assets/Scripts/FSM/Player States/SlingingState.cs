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

    public override void Tick()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;
        Vector2 targetDir = (Vector2)mousePos - (Vector2)SlingIndicator.transform.position;
        SlingIndicator.transform.up = targetDir;

        if(Input.GetMouseButtonDown(0))
        {
            Player.RB.velocity = Vector2.zero;
            Time.timeScale = 1f;
            Player.RB.AddForce((Vector2)SlingIndicator.transform.up.normalized * LaunchPower, ForceMode2D.Impulse);
            Player.SlingshotSpent = true;
            Player.IsSlinging = false;
            Player.IsSlung = true;
        }

        if (Player.IsGrounded)
        {
            GetComponent<StateMachine>().ChangeState("Grounded");
            Player.IsSlinging = false;
        }

        if (!Player.PI.IsActionPressed(PlayerInputs.PlayerAction.Dash))
            Player.IsSlinging = false;
    }
}
