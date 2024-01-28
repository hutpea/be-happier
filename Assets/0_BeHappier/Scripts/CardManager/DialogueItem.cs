using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[System.Serializable]
public class DialogueItem
{
    public DialogueType dialogueType;
    public string cardState;
    public string dialogueContent;
    [ShowIf("dialogueType", DialogueType.AutomaticEnd)]
    public float timeElapsedBeforeSwitchDialogue = 1F;
}

public enum DialogueType
{
    AutomaticEnd,
    RequireClick
}
