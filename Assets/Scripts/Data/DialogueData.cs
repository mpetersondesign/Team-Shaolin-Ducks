using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
    File: DialogueData.cs 
    Author: Matthew McFarland
    Date: 11/22/2022

    Data for a set of dialogue lines.
**/

[CreateAssetMenu]
public class DialogueData : ScriptableObject
{
    public CharacterData Speaker;
    public List<DialogueLine> Lines = new List<DialogueLine>();
}

[System.Serializable]
public class DialogueLine
{
    public string Text;
}

[System.Serializable]
public class OptionLine : DialogueLine
{
    public List<string> Options = new List<string>();
}
