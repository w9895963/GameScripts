using System.Collections;
using System.Collections.Generic;
using Global.Dialogue;
using UnityEngine;

namespace Global {
    public interface IDialogueTarget {
        bool EnabaleDialoguo { get; }
        DialoguoGroup DialoguoGroup { get; }
        Vector2 DialoguoTipPosition { get; }
    }
}