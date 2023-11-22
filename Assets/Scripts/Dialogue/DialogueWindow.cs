using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

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
    public GameObject optionsPanel;

    public Image characterPortrait;
    public TextMeshProUGUI characterNameDisplay;
    public TextMeshProUGUI dialogueDisplay;
    [Range(0,3)]
    public int textDelay = 1;
    public bool scrollOverSpace = false;
    private int textDelayTimer = 0;

    public TextMeshProUGUI option1Display;
    public TextMeshProUGUI option2Display;
    public TextMeshProUGUI option3Display;

    [SerializeField]
    private AudioCue audioCue;

    private Interactible interactedActor;
    private DialogueData currentDialogue;

    [SerializeField]
    private int currentLine = 0;

    private int numChar;
    private int currentChar = 0;
    private string currentDisplayText;
    private bool scrollingText = false;
    private int audioCounter = 0;

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

    public void DisplayDialogue(Interactible actor, DialogueData data)
    {
        if (dialoguePanel.activeSelf)
        {
            return;
        }
        interactedActor = actor;
        dialoguePanel.SetActive(true);
        FindObjectOfType<PlayerController>().RB.velocity = Vector2.zero;
        FindObjectOfType<PlayerController>().CanMove = false;
        currentDialogue = data;
        currentLine = 0;
        UpdateText();
    }

    public void UpdateText()
    {
        audioCue.PlayAudioCue(2);
        numChar = FindNumChar(currentDialogue.Lines[currentLine].Text);
        SetUpDisplayText();
        characterPortrait.sprite = currentDialogue.Lines[currentLine].Speaker.portrait;
        characterNameDisplay.text = currentDialogue.Lines[currentLine].Speaker.characterName;
        dialogueDisplay.text = currentDisplayText;
        CheckOptions();
        scrollingText = true;
    }

    public int FindNumChar(string text)
    {
        int num = 0;
        bool inField = false;
        char[] textChars = text.ToCharArray();
        for (int i = 0; i < text.Length; i++)
        {
            if(textChars[i] == '<')
            {
                inField = true;
            }

            if (!inField)
            {
                num++;
            }

            if(textChars[i] == '>')
            {
                inField = false;
            }
        }
        return num;
    }

    public void AdvanceText()
    {
        if (dialoguePanel.activeSelf)
        {
            if (currentDialogue.Lines[currentLine].NextLine >= 0)
            {
                currentLine = currentDialogue.Lines[currentLine].NextLine;
            }
            else
            {
                currentLine++;
            }

            if (currentLine < currentDialogue.Lines.Count)
            {
                UpdateText();
            }
            else
            {
                currentLine = 0;
                dialoguePanel.SetActive(false);
                optionsPanel.SetActive(false);
                FindObjectOfType<PlayerController>().CanMove = true;
                // Let Player Move Again

                audioCue.PlayAudioCue(3); // close dialogue audio
            }
        }
    }

    public void AdvanceText(int nextLine)
    {
        if (dialoguePanel.activeSelf)
        {
            currentLine = nextLine;
            
            if (currentLine < currentDialogue.Lines.Count && currentLine >= 0)
            {
                UpdateText();
            }
            else
            {
                currentLine = 0;
                dialoguePanel.SetActive(false);
                optionsPanel.SetActive(false);
                FindObjectOfType<PlayerController>().CanMove = true;
                // Let Player Move Again
            }
        }
    }

    public void SelectOption(int option)
    {
        interactedActor.DialogueEvent(currentDialogue.Lines[currentLine].Options[option].Output);
        AdvanceText(currentDialogue.Lines[currentLine].Options[option].NextLine);
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

    public void CheckOptions()
    {
        if(currentDialogue.Lines[currentLine].Dialogue_Type == DialogueType.options)
        {
            optionsPanel.SetActive(true);
            option1Display.text = currentDialogue.Lines[currentLine].Options[0].OptionLabel;
            option2Display.text = currentDialogue.Lines[currentLine].Options[1].OptionLabel;
            option3Display.text = currentDialogue.Lines[currentLine].Options[2].OptionLabel;
        }
        else
        {
            optionsPanel.SetActive(false);
        }
    }


    private void FixedUpdate()
    {
        if (scrollingText)
        {
            if(textDelayTimer >= textDelay)
            {
                TextScrollUpdate();
                dialogueDisplay.text = currentDisplayText;
                textDelayTimer = 0;
            }
            else
            {
                textDelayTimer++;
            }
            if (audioCounter % 2 == 0 && audioCounter != 0)
                audioCue.PlayAudioCue();
            if (audioCounter >= 5)
            {
                audioCounter = 0;
                audioCue.PlayAudioCue(1);
            }
            audioCounter += 1;
        }
        
    }

    public void TextScrollUpdate()
    {
        if (currentChar < currentDialogue.Lines[currentLine].Text.Length)
        {
            currentDisplayText = GetSubstring(currentDialogue.Lines[currentLine].Text);
        }
        else
        {
            scrollingText = false;
        }
    }

    public string GetSubstring(string text)
    {
        if (scrollOverSpace == true && text.ToCharArray()[currentChar] == ' ')
            ++currentChar;
        if (text.ToCharArray()[currentChar] == '<')
        {
            do
            {
                ++currentChar;
            } while (text.ToCharArray()[currentChar] != '>'
                     || text.ToCharArray()[currentChar + 1] == '<'); // added this so that multiple 
                                                                     // modifiers didnt incur a long pause
        }
        return text.Substring(0, 1 + currentChar++);
    }

}