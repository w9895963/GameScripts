using System.Collections;
using System.Collections.Generic;
using Global;
using Global.Dialogue;
using UnityEngine;

public class DialogueTarget : MonoBehaviour, IDialogueTarget {
    public DialoguoGroup dialoguoGroup = new DialoguoGroup ();
    public Vector2 dialoguoTipPosition;
    public bool enableDialoguo = true;

    public Vector2 DialoguoTipPosition => dialoguoTipPosition;

    public bool EnabaleDialoguo => enableDialoguo;


    public DialoguoGroup DialoguoGroup => dialoguoGroup;
}