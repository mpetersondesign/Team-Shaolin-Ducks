using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBehavior : MonoBehaviour
{
    public SpriteRenderer NPCSprite;

    // Update is called once per frame
    void Update()
    {
        if(FindObjectOfType<PlayerController>() != null)
        {
            var pc = FindObjectOfType<PlayerController>();
            if (pc.transform.position.x > transform.position.x)
                NPCSprite.flipX = false;
            else
                NPCSprite.flipX = true;
        }
    }
}
