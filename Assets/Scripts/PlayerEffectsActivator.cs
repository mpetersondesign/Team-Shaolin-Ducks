using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffectsActivator : MonoBehaviour
{
    public GameObject SlingEffect;
    public GameObject WallSlideEffect;
    public GameObject DashEffect;
    public Material ForegroundShader;
    public Camera MainCamera;
    public float ShakeScale;
    public float ForegroundMaskSize;
    public float ForegroundExpansionTime;

    private ParticleSystem wallSlideParticles;
    private ParticleSystem dashParticles;
    private bool wallSlideActive = false;
    private bool dashActive = false;
    private float foregroundUnscaledInterp = 0.0f;

    private void Awake()
    {
        ForegroundShader.SetFloat("_MaskSize", 0.0f);
        wallSlideParticles = WallSlideEffect.GetComponent<ParticleSystem>();
        dashParticles = DashEffect.GetComponent<ParticleSystem>();
    }

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
        if (speed > 13.5f)
        {
            float xNoise = (Mathf.PerlinNoise(transform.position.x, Time.time * 0.5f) - 0.5f) * 2.0f;
            float yNoise = (Mathf.PerlinNoise(Time.time * 0.5f, transform.position.y) - 0.5f);
            MainCamera.GetComponent<PlayerCamera>().ShakeOffset = new Vector2(xNoise, yNoise) * ShakeScale;
        }
        else
        {
            MainCamera.GetComponent<PlayerCamera>().ShakeOffset = Vector2.zero;
        }
    }

    public void StartWallSlideEffects(int direction)
    {
        if (!wallSlideActive)
        {
            ParticleSystem.ShapeModule shape = wallSlideParticles.shape;
            shape.position = new Vector3(direction * 0.5f, 0.0f);
            ParticleSystem.EmissionModule emission = wallSlideParticles.emission;
            emission.enabled = true;
            ParticleSystem.RotationBySpeedModule rotation = wallSlideParticles.rotationBySpeed;
            rotation.zMultiplier = direction;

            wallSlideActive = true;
        }
    }

    public void StopWallSlideEffects()
    {
        ParticleSystem.EmissionModule emission = wallSlideParticles.emission;
        emission.enabled = false;
        wallSlideActive = false;
    }

    public void StartDashEffects()
    {
        if (!wallSlideActive)
        {
            ParticleSystem.ShapeModule shape = dashParticles.shape;
            ParticleSystem.EmissionModule emission = dashParticles.emission;
            emission.enabled = true;
            ParticleSystem.RotationBySpeedModule rotation = dashParticles.rotationBySpeed;

            dashActive = true;
        }
    }

    public void StopDashEffects()
    {
        ParticleSystem.EmissionModule emission = dashParticles.emission;
        emission.enabled = false;
        dashActive = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "ForegroundTrigger")
        {
            StopCoroutine("RetractForegroundView");
            StartCoroutine("ExpandForegroundView");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "ForegroundTrigger")
        {
            StopCoroutine("ExpandForegroundView");
            StartCoroutine("RetractForegroundView");
        }
    }

    private IEnumerator ExpandForegroundView()
    {
        while (foregroundUnscaledInterp <= ForegroundExpansionTime)
        {
            float interp = foregroundUnscaledInterp / ForegroundExpansionTime;
            ForegroundShader.SetFloat("_MaskSize", Mathf.Lerp(0.0f, ForegroundMaskSize, interp));
            foregroundUnscaledInterp += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator RetractForegroundView()
    {
        while (foregroundUnscaledInterp >= 0.0f)
        {
            float interp = foregroundUnscaledInterp / ForegroundExpansionTime;
            ForegroundShader.SetFloat("_MaskSize", Mathf.Lerp(0.0f, ForegroundMaskSize, interp));
            foregroundUnscaledInterp -= Time.deltaTime;
            yield return null;
        }
    }
}
