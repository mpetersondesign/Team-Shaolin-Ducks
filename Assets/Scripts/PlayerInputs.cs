using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputs : MonoBehaviour
{

    public string H_AxisName;
    public string V_AxisName;
    public Vector2 RawInput;

    public PlayerInputs()
    {
        for (int i = 0; i < numActions; ++i)
        {
            keyStates[i] = KeyState.Released;
        }

        keyCodes[(int)PlayerAction.Crouch] = new List<KeyCode>() { KeyCode.LeftControl };
        keyCodes[(int)PlayerAction.Jump] = new List<KeyCode>() { KeyCode.Space };
        keyCodes[(int)PlayerAction.Interact] = new List<KeyCode>() { KeyCode.E };
        keyCodes[(int)PlayerAction.Dash] = new List<KeyCode>() { KeyCode.LeftShift };
    }

    private void Update()
    {
        RawInput.x = Input.GetAxisRaw(H_AxisName);
        RawInput.y = Input.GetAxisRaw(V_AxisName);

        for (int i = 0; i < numActions; ++i)
            UpdateAction(i);
    }

    private void UpdateAction(int i)
    {
        switch (keyStates[i])
        {
            case KeyState.OnPress:
            case KeyState.Pressed:
                for (int j = 0; j < keyCodes[i].Count; ++j)
                {
                    if (!Input.GetKey(keyCodes[i][j]))
                    {
                        keyStates[i] = KeyState.OnRelease;
                        return;
                    }
                }
                keyStates[i] = KeyState.Pressed;
                return;

            case KeyState.OnRelease:
            case KeyState.Released:
                for (int j = 0; j < keyCodes[i].Count; ++j)
                {
                    if (Input.GetKey(keyCodes[i][j]))
                    {
                        keyStates[i] = KeyState.OnPress;
                        return;
                    }
                }
                keyStates[i] = KeyState.Released;
                return;
        }
    }

    void CheckRelease(int i)
    {
        for (int j = 0; j < keyCodes[i].Count; ++j)
        {
            if (!Input.GetKey(keyCodes[i][j]))
            {
                keyStates[i] = KeyState.OnRelease;
                return;
            }
        }
    }

    public enum PlayerAction
    {
        //Movement Controls
        Crouch,
        Jump,
        Interact,
        Dash,

        Num,
    }

    public const int numActions = (int)PlayerAction.Num;

    public enum KeyState
    {
        OnPress,
        Pressed,
        OnRelease,
        Released,

        Num,
    }

    private KeyState[] keyStates = new KeyState[numActions];
    private List<KeyCode>[] keyCodes = new List<KeyCode>[numActions];

    public bool IsPressed(PlayerAction action)
    {
        return (int)keyStates[(int)action] < (int)KeyState.OnRelease;
    }

    public bool OnPress(PlayerAction action)
    {
        //if (action == PlayerAction.Jump)
            //Debug.Log("CheckPress");
        return keyStates[(int)action] == KeyState.OnPress;
    }

    public bool OnRelease(PlayerAction action)
    {
        return keyStates[(int)action] == KeyState.OnRelease;
    }

    /*private int SafeGetActionID(PlayerAction action)
    {
        int i = (int)action;
        if (i >= (int)PlayerAction.Num || i < 0)
            throw new System.ArgumentOutOfRangeException("PlayerAction", i, "Invalid Action Given");
        return i;
    }*/

    //Pass in an action to check for an input for
    /*public bool IsActionPressed(PlayerAction action)
    {
        //If the action doesn't exist, return false
        //if (!playerKeys.ContainsKey(action))
        //    return false;

        //If it does exist, check every key mapped to said action
        foreach (KeyCode key in playerKeys[action])
        {
            //If the key is pressed, return true
            if (Input.GetKey(key))
                return true;
        }

        return false;
    }*/
}
