using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffectsActivator : MonoBehaviour
{
    public GameObject SlingEffect;
    public Material ForegroundShader;

    private void OnSling(Vector2 targetDir)
    {
        // create and destroy the object 3 seconds later
        GameObject effect = Instantiate(SlingEffect, transform.position, transform.rotation);
        effect.transform.up = targetDir;
        ParticleSystem.RotationBySpeedModule rBS = effect.GetComponent<ParticleSystem>().rotationBySpeed;
        ParticleSystem.MinMaxCurve zCurve = rBS.z;
        zCurve.constant *= Mathf.Sign(Vector2.Dot(Vector2.right, targetDir));
        Destroy(effect, 3.0f);
    }

    private void Update()
    {
        ForegroundShader.SetVector("_PlayerPos", transform.position);
    }
}
