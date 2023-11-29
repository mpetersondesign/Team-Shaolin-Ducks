using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeHandler : MonoBehaviour
{
    public static FadeHandler Current;

    public GameObject fadePanel;

    [SerializeField]
    private float fadeTime = 5f;

    [SerializeField]
    private float timer = 0f;

    [SerializeField]
    private AnimationCurve fadeCurve;

    [SerializeField]
    private bool fadeIn = true;

    [SerializeField]
    private bool fadeOut = false;

    [SerializeField]
    private string nextScene;

    private Color fadeColor;

    // Sets Current on Awake
    public void Awake()
    {
        if (Current == null)
        {
            Current = this;
        }
        else
        {
            Destroy(this);
        }
        fadePanel.SetActive(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        timer = 0f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (fadeIn)
        {
            if(timer > fadeTime)
            {
                fadePanel.SetActive(false);
                fadeIn = false;
            }
            else
            {
                timer += Time.deltaTime;
                fadeColor = fadePanel.GetComponent<Image>().color;
                fadeColor.a = fadeCurve.Evaluate(timer/fadeTime);
                fadePanel.GetComponent<Image>().color = fadeColor;
            }
        }
        else if (fadeOut)
        {
            if (timer < 0)
            {
                if(nextScene == "Exit")
                {
                    Application.Quit();
                }
                else
                {
                    SceneManager.LoadScene(nextScene);
                }
            }
            else
            {
                timer -= Time.deltaTime;
                fadeColor = fadePanel.GetComponent<Image>().color;
                fadeColor.a = fadeCurve.Evaluate(timer / fadeTime);
                fadePanel.GetComponent<Image>().color = fadeColor;
            }
        }
    }

    // Triggers a fade out
    public void FadeOut(string nextScene)
    {
        this.nextScene = nextScene;
        fadeOut = true;
        fadePanel.SetActive(true);
        timer = fadeTime;
    }
}
