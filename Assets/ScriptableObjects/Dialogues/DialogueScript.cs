using System;
using NaughtyAttributes;
using UnityEngine;


[Serializable]
public struct Dialogue
{
    public string name;
    public string text;
    public GameObject camera; //(?)

    public bool hasItems;
    [HideIf("hasItems")]
    public bool hasOptions;

    [ShowIf("hasOptions")]
    public Option[] options;
}

[Serializable]
public struct Option
{
    public string text;

    public DialogueScript nextDialogue;
    public string answerIFNoDialogue;
}

[CreateAssetMenu(fileName = "DialogueScript", menuName = "Scriptable Objects/Dialogue")]
public class DialogueScript : ScriptableObject
{
    public Dialogue[] dialogues;
}
