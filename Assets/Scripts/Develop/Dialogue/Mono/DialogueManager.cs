using System.Collections;
using System.Collections.Generic;
using Global;
using Global.Dialogue;
using Global.Physic;
using Global.Visible;
using UnityEngine;

public class DialogueManager : MonoBehaviour {

    void Start () {
        var dialoguoTargets = DialogueUtility.dialoguoTargets;
        DialoguoTipTrigger trigger = GameObject.FindObjectOfType<DialoguoTipTrigger> ();
        PhysicUtility.AddTriggerAction (trigger.gameObject, onEnter, onExist);

        void onEnter (Collider2D other) {
            IDialogueTarget dialogueTarget = other.GetComponent<IDialogueTarget> ();
            bool isDialoguoTarget = false;
            if (dialogueTarget != null) {
                if (dialogueTarget.EnabaleDialoguo) {
                    isDialoguoTarget = true;
                    dialoguoTargets.Add (other.gameObject);
                }
            }

            if (isDialoguoTarget) {
                dialoguoTargets.Sort ((o) => (o.GetPosition2d () - Find.Player.GetPosition2d ()).magnitude);
                GameObject focusObj = dialoguoTargets[0];
                DialoguoTipAnimation tip = focusObj.GetComponentInChildren<DialoguoTipAnimation> ();
                if (tip == null) {
                    VisibleUtility.ShowDialoguoTip (focusObj);
                }

            }




        }
        void onExist (Collider2D other) {
            dialoguoTargets.Remove (other.gameObject);

            DialoguoTipAnimation tip = other.GetComponentInChildren<DialoguoTipAnimation> ();
            if (tip != null) {
                other.gameObject.GetComponentInChildren<DialoguoTipAnimation> ().DestroyObject ();
            }



        }

        InputUtility.InteractInput.performed += (d) => {
            if (DialogueUtility.FocuseTarget != null) {
                GameObject focuseTarget = DialogueUtility.FocuseTarget;
                IDialogueTarget target = focuseTarget.GetComponent<IDialogueTarget> ();
                DialoguoGroup dialoguo = target.DialoguoGroup;
                if (dialoguo != null) {
                    dialoguo.StartConversation ();
                }
            }
        };

    }




}