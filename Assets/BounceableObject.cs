using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceableObject : MonoBehaviour
{
    [Range(0, 1)]
    public float HorizontalDampening = 0.5f;
    public ParticleSystem DestroyParticles;
    public ParticleSystem EnemyCorpseParticles;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            var PC = collision.gameObject.GetComponent<PlayerController>();
            if (PC.IsSlung == true)
            {
                PC.RB.velocity = new Vector2(PC.RB.velocity.x * (1 - HorizontalDampening), 0f);
                PC.RB.AddForce(Vector2.up * 15f, ForceMode2D.Impulse);
                if (DestroyParticles != null)
                    Instantiate(DestroyParticles, transform.position, Quaternion.identity);
                if (EnemyCorpseParticles != null)
                    Instantiate(EnemyCorpseParticles, transform.position, Quaternion.identity);
                Destroy(this.gameObject);
            }

        }
    }
}
