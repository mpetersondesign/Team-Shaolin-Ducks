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

    private DialogueData currentDialogue;
    private int currentLine = 0;

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

        currentDialogue = data;
        currentLine = 0;
        UpdateText();
    }

    public void UpdateText()
    {
        characterPortrait.sprite = currentDialogue.Speaker.portrait;
        characterNameDisplay.text = currentDialogue.Speaker.characterName;
        dialogueDisplay.text = currentDialogue.Lines[currentLine].Text;
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
                // Let Player Move Again
            }
        }
    }
}