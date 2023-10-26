using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CollectibleCounter : MonoBehaviour
{

    private int collectCount = 0;

    public TextMeshProUGUI numberDisplay;

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
