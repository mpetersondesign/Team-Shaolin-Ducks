using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffectsActivator : MonoBehaviour
{
    public GameObject SlingEffect;
    public Material ForegroundShader;

    private void OnSling()
    {
        // create and destroy the object 3 seconds later
        GameObject effect = Instantiate(SlingEffect, transform.position, transform.rotation);
        Destroy(effect, 3.0f);
    }

    private void Update()
    {
        ForegroundShader.SetVector("_PlayerPos", transform.position);
    }
}
