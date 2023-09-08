using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AerialState : State
{
    private PlayerController Player;
    protected override void OnStateInitialize()
    {
        Player = GetComponent<PlayerController>();
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
        if (Player.RB.velocity.y < 0)
            Player.IsJumping = false;

        if (Player.IsGrounded)
            GetComponent<StateMachine>().ChangeState("Grounded");
    }
}
