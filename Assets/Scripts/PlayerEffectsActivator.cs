using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffectsActivator : MonoBehaviour
{
    public GameObject SlingEffect;
    public Material ForegroundShader;
    public Camera MainCamera;
    public float ShakeScale;

    private void OnSling(Vector2 targetDir)
    {
        // create gust particles
        GameObject effect = Instantiate(SlingEffect, transform.position, transform.rotation, transform);
        effect.transform.up = targetDir;
        ParticleSystem system = effect.GetComponent<ParticleSystem>();
        // rotation direction
        ParticleSystem.RotationBySpeedModule rBS = system.rotationBySpeed;
        float xDir = Mathf.Sign(targetDir.x);
        rBS.zMultiplier = 5.0f * xDir;
        // sprite flip
        float flipX = Mathf.Ceil(xDir * 0.5f);
        ParticleSystemRenderer renderer = effect.GetComponent<ParticleSystemRenderer>();
        renderer.flip = new Vector3(flipX, 0.0f, 0.0f);
        // effect self destructs on its own
    }

    private void Update()
    {
        ForegroundShader.SetVector("_PlayerPos", transform.position);

        // screen wobble when fast
        float speed = GetComponent<PlayerController>().CurrentVelocity.magnitude;
        if (speed > 14.5f)
        {
            float xNoise = (Mathf.PerlinNoise(transform.position.x, Time.time) - 0.5f) * 2.0f;
            float yNoise = (Mathf.PerlinNoise(Time.time, transform.position.y) - 0.5f);
            MainCamera.GetComponent<PlayerCamera>().ShakeOffset = new Vector2(xNoise, yNoise) * ShakeScale;
        }
        else
        {
            MainCamera.GetComponent<PlayerCamera>().ShakeOffset = Vector2.zero;
        }
    }
}
