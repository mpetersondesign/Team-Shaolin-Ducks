using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public PlayerController Player;
    public float LerpAmount;
    private Vector2 DefaultOffset;
    public Vector2 Offset;
    public float LookDownAmount;
    public float LookUpAmount;

    // Start is called before the first frame update
    void Awake()
    {
        Player = FindObjectOfType<PlayerController>();
        DefaultOffset = Offset;
    }

    // Update is called once per frame
    void Update()
    {
        GetInputs();
        CameraMovement();
    }

    private void GetInputs()
    {
        var v_axis = Input.GetAxis("Vertical");
        if (v_axis == -1)
            Offset.y = DefaultOffset.y - LookDownAmount;
        else if (v_axis == 1)
            Offset.y = DefaultOffset.y + LookUpAmount;
        else
            Offset.y = DefaultOffset.y;
    }

    private void CameraMovement()
    {
        Vector3 targetPos = Player.transform.position + (Vector3)Offset;
        targetPos.z = -10;
        transform.position = Vector3.Lerp(transform.position, targetPos, LerpAmount);
    }
}
