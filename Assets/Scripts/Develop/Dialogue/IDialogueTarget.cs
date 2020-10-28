using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Global {
    public interface IDialogueTarget {
        bool EnaDialoguo { get; }
        Vector2 DialoguoTipPosition { get; }
    }
}