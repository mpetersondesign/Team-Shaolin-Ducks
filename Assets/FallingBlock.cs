using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FallingBlock : MonoBehaviour
{
    public float FallDelay = 0.35f;

    private bool Activated = false;

    public void Activate()
    {
        if (!Activated)
        {
            StartCoroutine(nameof(Rumble));
            Invoke("StartFalling", FallDelay);
            Activated = true;
        }
    }

    public void StartFalling()
    {
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        GetComponent<BoxCollider2D>().enabled = false;
    }

    private IEnumerator Rumble()
    {
        float t = 0;
        float originalX = transform.position.x;

        while(t < FallDelay)
        {
            transform.position = new Vector3(Mathf.Sin(50.0f * t) * 0.04f + originalX, transform.position.y, transform.position.z);
            t += Time.deltaTime;
            yield return null;
        }

        transform.position = new Vector3(originalX, transform.position.y, transform.position.z);

        // enable particles
        GetComponent<ParticleSystem>().Play();
    }
}
