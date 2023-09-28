using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
    File: Interactible.cs 
    Author: Matthew McFarland
    Date: 12/1/2022

    Lets the player interact with the object
**/

public class Interactible : MonoBehaviour
{
    public DialogueData interactionData;
    public GameObject talkDisplay;
    [SerializeField]
    private bool seePlayer;
    private PlayerInputs playerInputs;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            seePlayer = true;
            playerInputs = collision.GetComponent<PlayerInputs>();
            talkDisplay.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            seePlayer = false;
            playerInputs = null;
            talkDisplay.SetActive(false);
        }
    }

    private void Update()
    {
        if (seePlayer)
        {
            
            if (playerInputs.OnPress(PlayerInputs.PlayerAction.Interact))
            {
                if (DialogueWindow.Current.dialoguePanel.activeSelf)
                {
                    DialogueWindow.Current.AdvanceText();
                }
                else
                {
                    DialogueWindow.Current.DisplayDialogue(interactionData);
                }
            }
        }
    }
}
