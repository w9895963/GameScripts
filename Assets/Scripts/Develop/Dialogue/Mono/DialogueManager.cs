using System.Collections;
using System.Collections.Generic;
using Global;
using Global.Dialogue;
using Global.Physic;
using Global.Visible;
using UnityEngine;

public class DialogueManager : MonoBehaviour {

    void Start () {
        DialogueUtility.UpdateDialoguoGroups ();



        var dialoguoTargets = DialogueUtility.dialoguoTargets;
        DialoguoTipTrigger trigger = GameObject.FindObjectOfType<DialoguoTipTrigger> ();
        PhysicUtility.AddTriggerAction (trigger.gameObject, onEnter, onExist);


        void onEnter (Collider2D other) {
            List<DialoguoGroup> dialoguos = DialogueUtility.GetDialoguoGroups (other.gameObject);

            if (dialoguos.Count > 0) {
                dialoguoTargets.Add (other.gameObject);
                GameObject focusObj = DialogueUtility.FocuseTarget;
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
            GameObject fucos = DialogueUtility.FocuseTarget;
            if (fucos != null) {
                List<DialoguoGroup> groups = DialogueUtility.GetDialoguoGroups (fucos);
                if (groups.NotEmpty ()) {
                    groups[0].StartDialoguo ();
                }
            }
        };

    }




}