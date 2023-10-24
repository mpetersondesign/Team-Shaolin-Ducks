using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/**
    File: DialogueWindow.cs 
    Author: Matthew McFarland
    Date: 11/22/2022

    The scripting for the dialogue window.
**/

public class DialogueWindow : MonoBehaviour
{
    public static DialogueWindow Current;

    public GameObject dialoguePanel;

    public Image characterPortrait;
    public TextMeshProUGUI characterNameDisplay;
    public TextMeshProUGUI dialogueDisplay;
    public int textDelay = 1;
    private int textDelayTimer = 0;

    [SerializeField]
    private DialogueData currentDialogue;
    private int currentLine = 0;

    private int numChar;
    private int currentChar = 0;
    private string currentDisplayText;
    private bool scrollingText = false;

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
    }

    public void DisplayDialogue(DialogueData data)
    {
        if (dialoguePanel.activeSelf)
        {
            return;
        }
        dialoguePanel.SetActive(true);
        FindObjectOfType<PlayerController>().RB.velocity = Vector2.zero;
        FindObjectOfType<PlayerController>().CanMove = false;
        currentDialogue = data;
        currentLine = 0;
        UpdateText();
    }

    public void UpdateText()
    {
        numChar = currentDialogue.Lines[currentLine].Text.Length;
        SetUpDisplayText();
        characterPortrait.sprite = currentDialogue.Speaker.portrait;
        characterNameDisplay.text = currentDialogue.Speaker.characterName;
        dialogueDisplay.text = currentDisplayText;
        scrollingText = true;
    }

    public void AdvanceText()
    {
        if (dialoguePanel.activeSelf)
        {
            currentLine++;
            if (currentLine < currentDialogue.Lines.Count)
            {
                UpdateText();
            }
            else
            {
                currentLine = 0;
                dialoguePanel.SetActive(false);
                FindObjectOfType<PlayerController>().CanMove = true;
                // Let Player Move Again
            }
        }
    }

    public void SetUpDisplayText()
    {
        currentChar = 0;
        currentDisplayText = "";
        for (int i = 0; i < numChar; i++)
        {
            currentDisplayText += " ";
        }
    }

    private void FixedUpdate()
    {
        if (scrollingText)
        {
            if(textDelayTimer % textDelay == 0)
            {
                TextScrollUpdate();
                dialogueDisplay.text = currentDisplayText;
            }
            if(textDelayTimer > textDelay)
            {
                textDelayTimer = 0;
            }
            else
            {
                textDelayTimer++;
            }
        }
        
    }

    public void TextScrollUpdate()
    {
        if (currentChar < numChar)
        {
            currentDisplayText = currentDialogue.Lines[currentLine].Text.Substring(0, currentChar);
            currentChar++;
        }
        else
        {
            scrollingText = false;
        }
    }
}