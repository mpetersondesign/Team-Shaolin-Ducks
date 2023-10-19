using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncePad : MonoBehaviour
{
    public float Power = 5f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            var player = collision.GetComponent<PlayerController>();
            player.IsSlung = true;
            player.RB.velocity = Vector2.zero;
            player.RB.angularVelocity = 0f;
            player.RB.AddForce(transform.up * Power, ForceMode2D.Impulse);
        }
    }
}
