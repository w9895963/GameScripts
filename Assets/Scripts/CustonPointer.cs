using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Global;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CustonPointer : MonoBehaviour {
    public float mouseSensity = 1f;
    public float scrollSensity = 1f;
    public Input inputSetting = new Input ();
    [System.Serializable] public class Input {
        public InputAction mouseDelta;
        public InputAction mousePress;
        public InputAction mouseScrollWheel;
        public InputAction touchPosition;
        public InputAction touchPress;
    }

    public Vector2 cursorPosition;
    public GameObject overObject;
    public List<GameObject> overobjects = new List<GameObject> ();

    private PointerEventData pointerData = new PointerEventData (EventSystem.current);



    private void Awake () {
        cursorPosition = new Vector2 (Screen.width, Screen.height) / 2;

        inputSetting.mouseDelta.performed += (d) => {
            Vector2 currP;
            CalcCursorPositionViaMouse (out currP);
            SetCursorPosition (currP);


            List<GameObject> exitObjs;
            List<GameObject> enterObjs;
            FindOverObjs (out exitObjs, out enterObjs);

            CallEnterAction (enterObjs, pointerData);
            CallExitAction (exitObjs, pointerData);

        };

        inputSetting.mousePress.performed += (d) => {
            if (overObject) {
                CallActionOnceUpward<IPointerClickHandler> (overObject, (x) => {
                    x.OnPointerClick (pointerData);
                });
            };
        };


        inputSetting.mouseScrollWheel.performed += (d) => {
            if (overObject) {
                pointerData.scrollDelta = d.ReadValue<Vector2> () / 100f * scrollSensity;
                CallActionOnceUpward<IScrollHandler> (overObject, (x) => {
                    x.OnScroll (pointerData);
                });

            };
        };




        /*    //*Touch Input
           inputSetting.touchPress.performed += (d) => {
               Vector2 p = inputSetting.touchPosition.ReadValue<Vector2> ();
               SetCursorPosition (p);

               List<GameObject> exitObjs;
               List<GameObject> enterObjs;
               CalcOverObjs (out exitObjs, out enterObjs);

               CallEnterAction (enterObjs, pointerData);
               CallExitAction (exitObjs, pointerData);

               if (overObject) {
                   CallClickAction (overObject, pointerData);
               };
           }; */


    }

    private void OnEnable () {
        inputSetting.mouseDelta.Enable ();
        inputSetting.mousePress.Enable ();
        inputSetting.mouseScrollWheel.Enable ();
        inputSetting.touchPress.Enable ();
        inputSetting.touchPosition.Enable ();
    }
    private void OnDisable () {
        inputSetting.mouseDelta.Disable ();
        inputSetting.mousePress.Disable ();
        inputSetting.mouseScrollWheel.Disable ();
        inputSetting.touchPress.Disable ();
        inputSetting.touchPosition.Disable ();
    }

    void Update () {

    }

    private void FindOverObjs (out List<GameObject> exitObjs, out List<GameObject> enterObjs) {
        PointerEventData pointData = pointerData;

        List<RaycastResult> result = new List<RaycastResult> ();

        EventSystem.current.RaycastAll (pointData, result);


        GameObject lastObj = overObject;
        GameObject currObj = (result.Count > 0) ? result[0].gameObject : null;
        List<GameObject> lastObjs = overobjects;
        List<GameObject> currObjs = currObj.GetParentsAndSelf ();
        exitObjs = lastObjs.Except (currObjs).ToList ();
        enterObjs = currObjs.Except (lastObjs).ToList ();




        overObject = currObj;
        overobjects = currObjs;
    }

    private static void CallAction<T> (List<GameObject> gameObjects, UnityAction<T> action) {
        gameObjects.ForEach ((x) => {
            x.GetComponents<T> ().ForEach ((y) => {
                action (y);
            });
        });

    }
    private static void CallActionOnceUpward<T> (GameObject gameObject, UnityAction<T> action) {
        T comp = gameObject.GetComponent<T> ();
        if (comp == null) {
            comp = gameObject.GetComponentInParent<T> ();
        }
        if (comp != null) {
            action (comp);
        }
    }

    private static void CallEnterAction (List<GameObject> gameObjects, PointerEventData data) {
        gameObjects.ForEach ((x) => {
            x.GetComponents<IPointerEnterHandler> ().ForEach ((y) => {
                y.OnPointerEnter (data);
            });
        });
    }
    private static void CallExitAction (List<GameObject> gameObjects, PointerEventData data) {
        gameObjects.ForEach ((x) => {
            x.GetComponents<IPointerExitHandler> ().ForEach ((y) => {
                y.OnPointerExit (data);
            });
        });
    }

    private static void CallClickAction (GameObject gameObjects, PointerEventData data) {
        gameObjects.GetComponents<IPointerClickHandler> ().ForEach ((y) => {
            y.OnPointerClick (data);
        });
    }


    private void CalcCursorPositionViaMouse (out Vector2 currP) {
        Vector2 lastP = cursorPosition;
        Vector2 mouseDelta = inputSetting.mouseDelta.ReadValue<Vector2> ();
        float sensitivity1 = mouseSensity;

        if (mouseDelta.x != 0 | mouseDelta.y != 0) {
            Vector2 p = lastP;

            p += mouseDelta * sensitivity1;
            p = new Vector2 (p.x.Clamp (0, Screen.width), p.y.Clamp (0, Screen.height));

            currP = p;
        } else {
            currP = lastP;
        }
    }

    private void SetCursorPosition (Vector2 p) {
        GlobalObject.Cursor.SetPosition (p);
        pointerData.position = p;
        cursorPosition = p;
    }
}