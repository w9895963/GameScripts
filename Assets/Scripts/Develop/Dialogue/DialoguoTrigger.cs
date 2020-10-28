using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Global;
using Global.GlobalData;
using UnityEngine;

public class DialoguoTrigger : MonoBehaviour {
    public List<Collider2D> targets = new List<Collider2D> ();
    public Collider2D focus;
    private void OnTriggerEnter2D (Collider2D other) {
        IDialogueTarget dialogueTarget = other.GetComponentInChildren<IDialogueTarget> ();
        if (dialogueTarget != null) {
            targets.Add (other);
            targets.Sort ((col) => {
                return (col.gameObject.GetPosition2d () - gameObject.GetPosition2d ()).magnitude;
            });
            focus = targets[0];
        }



    }
    private void OnTriggerExit2D (Collider2D other) {
        targets.Remove (other);
        if (focus == other) {
            focus = null;
        }

        if (targets.Count > 0) {
            focus = targets[0];
        }
    }
}