using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Global {
    namespace Visible {
        public static class VisibleUtility {
            private const string DialoguoTipPrefabPath = "Animation/Dialogue/DialoguoTip";
            private static GameObject dialoguoTip;
            private static GameObject DialoguoTip => dialoguoTip?dialoguoTip : dialoguoTip = Resources.Load (
                DialoguoTipPrefabPath, typeof (GameObject)) as GameObject;



            public static GameObject ShowDialoguoTip (GameObject target) {
                IDialogueTarget dialogueTarget = target.GetComponent<IDialogueTarget> ();
                Vector2 dialoguoTipPosition = new Vector2 (0.5f, 0.9f);
                if (dialogueTarget != null) {
                    dialoguoTipPosition = dialogueTarget.DialoguoTipPosition;
                }
                GameObject obj = GameObject.Instantiate (DialoguoTip, target.transform);
                obj.transform.localPosition = dialoguoTipPosition;
                return obj;

            }

            public static void ShowNormalDialoguo (GameObject speaker, string v) { }

            public static void ShowOptionalDialoguo (List<string> contents) { }




        }

    
    }
}