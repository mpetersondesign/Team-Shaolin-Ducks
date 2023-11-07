using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingBlock : MonoBehaviour
{
    public float FallDelay = 0.35f;
    public void Activate()
    {
        Invoke("StartFalling", FallDelay);
    }

    public void StartFalling()
    {
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        GetComponent<BoxCollider2D>().enabled = false;
    }
}
