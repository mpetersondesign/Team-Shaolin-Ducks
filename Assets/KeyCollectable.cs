using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyCollectable : MonoBehaviour
{
    public bool Collected;

    public void Start()
    {
        Collected = false;
    }

    public void Update()
    {
        if(Collected)
        {
            transform.position = Vector3.Lerp(transform.position, FindObjectOfType<PlayerController>().transform.position, 0.01f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && Collected == false)
        {
            GetComponent<AudioCue>().PlayAudioCue();
            Collected = true;
        }
    }
}
