using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableBlock : MonoBehaviour
{
    public GameObject BreakEffect;

    public void Activate()
    {
        Instantiate(BreakEffect, transform.position, Quaternion.identity);
        Destroy(transform.parent.gameObject);
    }
}
