using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;
namespace Global {
    public static class InputUtility {
        private static class Asset {
            private static InputActionAsset inputActionAsset;
            public static InputActionAsset InputActionAsset => inputActionAsset?inputActionAsset : inputActionAsset =
                GameObject.FindObjectOfType<InputSystemUIInputModule> ().actionsAsset;
        }


        public static InputAction MoveInput => Asset.InputActionAsset.FindAction ("Move");
        public static InputAction JumpInput => Asset.InputActionAsset.FindAction ("Jump");
        public static InputAction InteractInput => Asset.InputActionAsset.FindAction ("Interact");
        public static InputAction Pointer => Asset.InputActionAsset.FindAction ("Point");
        
        public static GameObject TopLayer {
            get {
                const string Name = "TopLayer";
                GameObject ui = Find.UI;
                GameObject top = ui.FindChild (Name);
                if (top == null) {
                    top = ui.CreateChild (Name);
                    Image image = top.AddComponent<Image> ();
                    image.color = new Color (0, 0, 0, 0);
                }
                top.transform.SetSiblingIndex (ui.transform.childCount - 1);
                return top;
            }
        }



        public static (EventTrigger, EventTrigger.Entry) AddPointerEvent (GameObject target,
            EventTriggerType type, UnityAction<BaseEventData> action) {
            // * ---------------------------------- 
            EventTrigger trigger = target.GetComponent<EventTrigger> ();
            if (trigger == null) {
                trigger = target.AddComponent<EventTrigger> ();
            }

            EventTrigger.Entry entry = new EventTrigger.Entry ();
            entry.eventID = type;
            entry.callback.AddListener (action);
            trigger.triggers.Add (entry);
            return (trigger, entry);
        }
        public static (EventTrigger, EventTrigger.Entry) AddGlobalPointerEvent (GameObject souce,
            EventTriggerType type, UnityAction<BaseEventData> action) {
            // * ---------------------------------- 


            EventTrigger trigger = souce.GetComponent<EventTrigger> ();
            if (trigger == null) {
                trigger = souce.AddComponent<EventTrigger> ();
            }

            EventTrigger.Entry entry = new EventTrigger.Entry ();
            entry.eventID = type;
            entry.callback.AddListener (action);
            trigger.triggers.Add (entry);
            return (trigger, entry);
        }

        public static void EnableAll () {
            MoveInput.Enable ();
            JumpInput.Enable ();
            InteractInput.Enable ();
        }

        public static void DisableAll () {
            MoveInput.Disable ();
            JumpInput.Disable ();
            InteractInput.Disable ();
        }
    }

}