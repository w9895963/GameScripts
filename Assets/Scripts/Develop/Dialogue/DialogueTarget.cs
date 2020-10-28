using System.Collections;
using System.Collections.Generic;
using Global;
using UnityEngine;

public class DialogueTarget : MonoBehaviour, IDialogueTarget {
    public Vector2 dialoguoTipPosition;
    public bool enaDialoguo = true;

    public Vector2 DialoguoTipPosition => dialoguoTipPosition;

    public bool EnaDialoguo => enaDialoguo;
}