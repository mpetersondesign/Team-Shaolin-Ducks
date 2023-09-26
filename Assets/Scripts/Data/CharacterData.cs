using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
    File: CharacterData.cs 
    Author: Matthew McFarland
    Date: 11/22/2022

    Data for each character.
**/

[CreateAssetMenu]
public class CharacterData : ScriptableObject
{
    public string characterName = "DefaultName";
    public Sprite portrait;
}
