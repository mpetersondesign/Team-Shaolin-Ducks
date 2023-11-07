using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableBlock : MonoBehaviour
{
    public void Activate()
    {
        Destroy(transform.parent.gameObject);
    }
}
