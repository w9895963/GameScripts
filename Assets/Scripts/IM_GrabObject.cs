using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class IM_GrabObject : IC_Base {
    public Collider2D clickBox;
    public I_Grab.Setting grabSetting = new I_Grab.Setting ();

    private void OnEnable () {
        NormalState ();




        void NormalState () {
            I_Input inputComp = null;
            I_Input inputComp2 = null;
            I_Grab grabcomp = null;

            inputComp = I_Input.CreateComp.Onece (gameObject, clickBox);
            inputComp.AddEventOnInput (ObjectClick);
            void ObjectClick () {
                inputComp2 = I_Input.CreateComp.GblobleOnece (gameObject);
                inputComp2.AddEventOnInput (ClickOnDrag);
                grabcomp = I_Grab.CreateComp (gameObject);
                grabcomp.variable = grabSetting;
                grabcomp.enabled = true;
            }
            void ClickOnDrag () {
                inputComp.Destroy ();
                inputComp2.Destroy ();
                grabcomp.Destroy ();
                NormalStateEnd ();
            }
        }
        void NormalStateEnd () {
            if (true) {
                NormalState ();
            } else {

            }
        };



    }
    private void OnDisable () {

    }


}