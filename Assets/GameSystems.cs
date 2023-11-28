using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class GameSystems : MonoBehaviour
{
    public GameObject HUD;
    public GameObject OrbReadout;
    public TextMeshProUGUI TimerText;
    public TextMeshProUGUI AreaText;
    public TextMeshProUGUI BestTimeText;
    public TextMeshProUGUI JasonBestTimeText;
    public TextMeshProUGUI JasonBestOrbsText;
    public TextMeshProUGUI OrbsCollectedText;
    public float JasonBestTime;
    public float JasonBestOrbs;
    public float MatthewBestTime;
    public float Timer;
    public bool TimerEnabled;
    public int CurrentOrbs;

    private void Start()
    {
        AreaChange("Hub Area");
    }

    public void OrbCollect()
    {
        CurrentOrbs++;
    }

    public void StartTimer(GameLevel level)
    {
        CurrentOrbs = 0;
        Timer = 0f;
        TimerEnabled = true;
        TimerText.gameObject.SetActive(true);
        BestTimeText.gameObject.SetActive(true);
        OrbReadout.SetActive(true);
        switch(level)
        {
            case GameLevel.Jason:
                {
                    if (JasonBestTime > 0f)
                        BestTimeText.text = JasonBestTimeText.text;
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
            OrbsCollectedText.text = $"X{CurrentOrbs}";

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
        OrbReadout.SetActive(false);
        switch (level)
        {
            case GameLevel.Jason:
                {
                    if (Timer < JasonBestTime || JasonBestTime == 0)
                    { 
                        JasonBestTime = Timer;
                        JasonBestTimeText.text = $"Best Time: {ReturnTimer(JasonBestTime)}";
                    }

                    if (CurrentOrbs > JasonBestOrbs)
                        JasonBestOrbsText.text = $"Orbs Collected: {CurrentOrbs}";

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
