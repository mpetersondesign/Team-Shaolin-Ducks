using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CollectibleCounter : MonoBehaviour
{
    public static CollectibleCounter Current;

    private int collectCount = 0;

    public TextMeshProUGUI numberDisplay;

    private void Awake()
    {
        if(Current == null)
        {
            Current = this;
        }
        else
        {
            Destroy(this);
        }
    }

    void FixedUpdate()
    {
        numberDisplay.text = collectCount.ToString();
    }

    public void Collect(int numCollect)
    {
        collectCount += numCollect;
    }

    public bool Spend(int numSpend)
    {
        if(numSpend > collectCount)
        {
            return false;
        }
        else
        {
            collectCount -= numSpend;
            return true;
        }
    }
}
