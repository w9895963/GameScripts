using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
namespace Global {
    public static class InputUtility {
        public static class Asset {
            private static InputActionAsset inputActionAsset;
            public static InputActionAsset InputActionAsset =>
                inputActionAsset?inputActionAsset : inputActionAsset = Resources.Load<InputActionAsset> ("Input/InputActions");
        }


        public static InputAction MoveInput => Asset.InputActionAsset.FindAction ("Move");
        public static InputAction JumpInput => Asset.InputActionAsset.FindAction ("Jump");

        public static (EventTrigger, EventTrigger.Entry) AddPointerEvent (GameObject souce,
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
        public static (EventTrigger, EventTrigger.Entry) AddPointerEvent (Component souce,
            EventTriggerType type, UnityAction<BaseEventData> action) {
            return AddPointerEvent (souce, type, action);
        }



    }

}