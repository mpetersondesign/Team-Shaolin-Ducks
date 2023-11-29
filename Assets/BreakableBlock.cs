using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableBlock : MonoBehaviour
{
    public GameObject BreakEffect;

    public void Activate()
    {
        Instantiate(BreakEffect, transform.position, Quaternion.identity);
        GetComponent<AudioCue>().PlayAudioCue();
        Destroy(transform.parent.gameObject);
    }
}
