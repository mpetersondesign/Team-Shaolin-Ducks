using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Collectible : MonoBehaviour
{

    [SerializeField]
    private UnityEvent onCollect;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            onCollect.Invoke();
        }
    }

    public void CollectCount(int numCollect)
    {
        CollectibleCounter.Current.Collect(numCollect);
    }
}
