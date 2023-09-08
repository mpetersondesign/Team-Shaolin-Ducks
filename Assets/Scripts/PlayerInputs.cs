using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputs : MonoBehaviour
{

    public string H_AxisName;
    public string V_AxisName;
    public Vector2 RawInput;

    private void Update()
    {
        RawInput.x = Input.GetAxisRaw(H_AxisName);
        RawInput.y = Input.GetAxisRaw(V_AxisName);
    }

    public enum PlayerAction
    {
        //Movement Controls
        Crouch,
        Jump,
        Interact,
        Dash,
    }

    private Dictionary<PlayerAction, List<KeyCode>> playerKeys = new Dictionary<PlayerAction, List<KeyCode>>()
    {
        { PlayerAction.Crouch, new List<KeyCode> { KeyCode.LeftControl } },
        { PlayerAction.Jump, new List<KeyCode> { KeyCode.Space } },
        { PlayerAction.Dash, new List<KeyCode> { KeyCode.LeftShift } },
        { PlayerAction.Interact, new List<KeyCode> {KeyCode.E } },
        // Add more mappings as needed
    };

    //Pass in an action to check for an input for
    public bool IsActionPressed(PlayerAction action)
    {
        //If the action doesn't exist, return false
        if (!playerKeys.ContainsKey(action))
            return false;


        //If it does exist, check every key mapped to said action
        foreach (KeyCode key in playerKeys[action])
        {
            //If the key is pressed, return true
            if (Input.GetKey(key))
                return true;
        }

        return false;
    }
}
