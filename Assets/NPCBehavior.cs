using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBehavior : MonoBehaviour
{
    public SpriteRenderer NPCSprite;
    public bool reverseFlip = false;

    // Update is called once per frame
    void Update()
    {
        if(FindObjectOfType<PlayerController>() != null)
        {
            var pc = FindObjectOfType<PlayerController>();
            if (pc.transform.position.x > transform.position.x)
                NPCSprite.flipX = reverseFlip;
            else
                NPCSprite.flipX = !reverseFlip;
        }
    }
}
