using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneFader : MonoBehaviour
{
    public void DestroySelf()
    {
        Destroy(this.gameObject);
    }

    public void FadeIn()
    {
        GetComponent<Animator>().Play("FadeIn");
    }

    public void FadeOut()
    {
        GetComponent<Animator>().Play("FadeOut");
    }
}
