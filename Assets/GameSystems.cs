using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameSystems : MonoBehaviour
{
    public GameObject HUD;
    public TextMeshProUGUI TimerText;
    public TextMeshProUGUI AreaText;
    public TextMeshProUGUI BestTimeText;
    public TextMeshProUGUI JasonBestText;
    public float JasonBestTime;
    public float MatthewBestTime;
    public float Timer;
    public bool TimerEnabled;

    private void Start()
    {
        AreaChange("Hub Area");
    }

    public void StartTimer(GameLevel level)
    {
        Timer = 0f;
        TimerEnabled = true;
        TimerText.gameObject.SetActive(true);
        BestTimeText.gameObject.SetActive(true);
        switch(level)
        {
            case GameLevel.Jason:
                {
                    if (JasonBestTime > 0f)
                        BestTimeText.text = JasonBestText.text;
                    return;
                }
        }
    }

    public void Update()
    {
        if (TimerEnabled)
        {
            Timer += Time.deltaTime;
            TimerText.text = $"{ReturnTimer(Timer)}";
        }
    }

    public void AreaChange(string areaName)
    {
        AreaText.text = areaName;
        AreaText.GetComponent<Animator>().Play("AreaFadeOut", 0, 0.0f);
    }
    public void EndTimer(GameLevel level)
    {
        TimerEnabled = false;
        TimerText.gameObject.SetActive(false);
        BestTimeText.gameObject.SetActive(false);

        switch (level)
        {
            case GameLevel.Jason:
                {
                    if (Timer < JasonBestTime || JasonBestTime == 0)
                    { 
                        JasonBestTime = Timer;
                        JasonBestText.text = $"Best Time: {ReturnTimer(JasonBestTime)}";
                    }

                    return;
                }
        }
    }

    public string ReturnTimer(float timer)
    {
        // Calculate minutes and seconds from the timer
        int minutes = (int)Timer / 60;
        int seconds = (int)Timer % 60;

        // Format and display the time
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
