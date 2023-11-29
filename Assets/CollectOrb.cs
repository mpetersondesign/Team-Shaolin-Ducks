using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectOrb : MonoBehaviour
{
    public ParticleSystem CollectParticles;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            FindObjectOfType<GameSystems>().OrbCollect();
            Instantiate(CollectParticles, transform.position, Quaternion.identity);
            GetComponent<AudioCue>().PlayAudioCue();
            Destroy(this.gameObject);
        }
    }
}
