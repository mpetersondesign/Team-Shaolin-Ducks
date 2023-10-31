using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

/**
    File: DialogueData.cs 
    Author: Matthew McFarland
    Date: 11/22/2022

    Data for a set of dialogue lines.
**/

public enum DialogueType
{
    standard,
    options
}

[CreateAssetMenu]
public class DialogueData : ScriptableObject
{
    public List<DialogueLine> Lines = new List<DialogueLine>();
}

[System.Serializable]
public class DialogueOption
{
    public string OptionLabel;
    public int Output;
}

[System.Serializable]
public class DialogueLine
{
    // Basic Dialogue
    public CharacterData Speaker;
    public string Text;

    // Dialogue Options
    public DialogueType Dialogue_Type = DialogueType.standard;
    public List<DialogueOption> Options = new List<DialogueOption>();

    // Unity Editor stuff to make me happy
#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(DialogueLine))]
    public class DialogueLineDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Rect currPosition = position;
            currPosition.height = EditorGUIUtility.singleLineHeight;
            property.isExpanded = EditorGUI.Foldout(currPosition, property.isExpanded, label);
            if (property.isExpanded)
            {
                // Do things
                currPosition.y += EditorGUIUtility.singleLineHeight;
                SerializedProperty propertySpeaker = property.FindPropertyRelative("Speaker");
                EditorGUI.PropertyField(currPosition, propertySpeaker);

                currPosition.y += EditorGUIUtility.singleLineHeight;
                SerializedProperty propertyText = property.FindPropertyRelative("Text");
                propertyText.stringValue = EditorGUI.TextField(currPosition, propertyText.displayName, propertyText.stringValue);

                currPosition.y += EditorGUIUtility.singleLineHeight;
                SerializedProperty propertyType = property.FindPropertyRelative("Dialogue_Type");
                propertyType.enumValueIndex = (int)(DialogueType)EditorGUI.EnumPopup(currPosition, propertyType.displayName, (DialogueType)propertyType.enumValueIndex);
                if ((DialogueType)propertyType.enumValueIndex == DialogueType.options)
                {
                    // Show Options
                    currPosition.y += EditorGUIUtility.singleLineHeight;
                    SerializedProperty propertyOptions = property.FindPropertyRelative("Options");
                    currPosition.height = (propertyOptions.arraySize * 2 + 1) * EditorGUIUtility.singleLineHeight;
                    EditorGUI.PropertyField(currPosition, propertyOptions);
                }
            }
        }

        private const float EXPAND_HEIGHT = 3.5f;
        private const float EXPAND_EXTRA_HEIGHT = 1f;
        private const float EXPAND_EXPAND_HEIGHT = 2.5f;
        private const float EXPAND_EXPAND_EXTRA_HEIGHT = 1.11f;
        private const float EXPAND_EXPAND_EXPAND_HEIGHT = 2f;


        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float height = 1f;

            if (property.isExpanded)
            {
                height += EXPAND_HEIGHT;

                SerializedProperty propertyType = property.FindPropertyRelative("Dialogue_Type");
                if ((DialogueType)propertyType.enumValueIndex == DialogueType.options)
                {
                    SerializedProperty propertyOptions = property.FindPropertyRelative("Options");

                    height += EXPAND_EXTRA_HEIGHT;

                    if (propertyOptions.isExpanded)
                    {
                        height += EXPAND_EXPAND_HEIGHT;

                        for(int i = 0; i < propertyOptions.arraySize; i++)
                        {
                            height += EXPAND_EXPAND_EXTRA_HEIGHT;
                            if (propertyOptions.GetArrayElementAtIndex(i).isExpanded)
                            {
                                height += EXPAND_EXPAND_EXPAND_HEIGHT;
                            }
                        }
                        if (propertyOptions.arraySize == 1)
                        {
                            //height -= EXPAND_EXPAND_EXTRA_HEIGHT;
                        }
                    }
                }
            }

            return height * EditorGUIUtility.singleLineHeight;
        }
    }
#endif
}
