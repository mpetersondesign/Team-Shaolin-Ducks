using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundedState : State
{
    private PlayerController Player;
    protected override void OnStateInitialize()
    {
        Player = GetComponent<PlayerController>();
    }
    public override void Enter(string previous_key, State previous_state)
    {
        if (Player.IsJumping == false && Player.RB.velocity.y > 0)
            Player.RB.velocity = new Vector2(Player.RB.velocity.x, 0);
    }

    public override void Exit(string next_key, State next_state)
    {

    }

    public override void Tick()
    {
        if (Player.PI.IsActionPressed(PlayerInputs.PlayerAction.Jump))
        {
            Player.IsJumping = true;
            Player.RB.velocity = new Vector2(Player.RB.velocity.x, Player.JumpStrength);
        }
    }
}
