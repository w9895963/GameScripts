using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class M_Pickable : MonoBehaviour {
    [SerializeField] private Collider2D clickBox = null;
    public State objectState = State.onground;
    [SerializeField, ReadOnly] private bool grabZoneEventEnable = false;
    [SerializeField, ReadOnly] private bool inputEventEnable = false;
    private EventTrigger inputEvent;
    private C_ColliderEvent grabZoneEvent;
    private bool indicateMode = false;
    private EventTrigger backpackButtonEvent;
    private Dictionary<string, State> enumDict;
    private C_StateMachine state;
    private bool passNextTest;
    private EventTrigger canvasBackGroundInput;

    private void Awake () {

    }

    private void Start () {
        enumDict = Fn._.EnumToDict<State> ();
        state = new C_StateMachine (Fn._.EnumToArray<State> ());


        state.AddEnterListener (() => {
            GrabZoneEventSetup (true);
        }, State.onground, State.ongroundCloesed);

        state.AddEnterListener (() => {
            GrabZoneEventSetup (false);
        }, State.inhand, State.inpack);


        state.AddListener (State.ongroundCloesed, () => {
            InputEventSetup (true);
        }, () => {
            InputEventSetup (false);
        });

        state.AddListener (State.inhand, () => {
            IndicateModeSetup (true);
            SpriteRenderer r = GetComponent<SpriteRenderer> ();
            Color c = r.color;
            r.color = new Color (c.r, c.g, c.b, 0.1f);
        }, () => {
            IndicateModeSetup (false);
            SpriteRenderer r = GetComponent<SpriteRenderer> ();
            Color c = r.color;
            r.color = new Color (c.r, c.g, c.b, 1);
        });


        state.AddListener (State.inpack, () => {
            if (backpackButtonEvent == null) {
                backpackButtonEvent = this.Ex_AddInputToTrigger (Gb.BackpackButton,
                    EventTriggerType.PointerClick, (d) => {
                        ////////////////////////////////
                        state.ChangeState (State.inhand);
                    });
            }
        }, () => {
            backpackButtonEvent.Destroy ();
        });




        state.AddListener (State.inpack, () => {
            Gb.Backpack.PutinStorage (gameObject);
        }, () => {
            Gb.Backpack.PutoutStorage (gameObject);
        });


        state.AddListenerToAll (() => {
            objectState = enumDict[state.CurrentState];
        });



        state.SetState (State.onground.ToString ());
        state.InvokeEnterEvent (State.onground.ToString ());

    }



    private void OnDisable () { }
    private void OnEnable () { }

    private void Update () {
        if (indicateMode) {
            transform.Set2dPosition (Pointer.current.position.ReadValue ().ScreenToWold ());
        }
    }

#if UNITY_EDITOR
    private void OnValidate () {
        UnityEditor.EditorApplication.delayCall += () => {
            state?.ChangeState (objectState.ToString ());
        };
    }
#endif




    //*Public
    public void Pickup () {
        Gb.Backpack.PutinStorage (gameObject);
    }




    //*Private
    public void InputEventSetup (bool enabled) {
        if (enabled) {
            UnityAction<BaseEventData> onclick = (d) => {
                if (Gb.Backpack.IsFull ()) {

                } else {
                    state.ChangeState (State.inpack);
                }
            };


            if (inputEvent == null) {
                inputEvent = this.Ex_AddInputToTriggerOnece (clickBox, EventTriggerType.PointerClick, onclick);
            }
            inputEventEnable = inputEvent != null;


        } else {
            inputEvent.Destroy ();
            inputEventEnable = inputEvent != null;
        }
    }
    public void GrabZoneEventSetup (bool enabled) {
        if (enabled) {
            bool touching = Gb.MainCharactor.GrabBox.IsTouching (clickBox);
            if (touching) {
                if (!passNextTest) {
                    state.ChangeState (State.ongroundCloesed);
                }
                passNextTest = false;

            } else {
                state.ChangeState (State.onground);
            }


            UnityAction<Collider2D> enter = (d) => {
                state.ChangeState (State.ongroundCloesed);
            };
            UnityAction<Collider2D> exit = (d) => {
                passNextTest = true;
                state.ChangeState (State.onground);
            };
            grabZoneEventEnable = true;
            if (grabZoneEvent == null) {
                grabZoneEvent = Gb.MainCharactor.GrabBox.Ex_AddTriggerEvent (gameObject, enter, exit);
            }

        } else {
            grabZoneEventEnable = false;
            grabZoneEvent.Destroy ();
        }

    }

    public void IndicateModeSetup (bool enabled) {
        indicateMode = enabled;
        if (enabled) {
            UnityEngine.Events.UnityAction<PointerData> callback = (d) => {
                IndicateModeSetup (false);
            };
            clickBox.Ex_AddPointerEventOnece (PointerEventType.onClick, callback);


            Gb.CanvasBackGround.enabled = true;
            if (canvasBackGroundInput == null) {
                canvasBackGroundInput = this.Ex_AddInputToTriggerOnece (Gb.CanvasBackGround.gameObject,
                    EventTriggerType.PointerClick, (d) => {
                        ////////////////////////////////
                        Gb.CanvasBackGround.enabled = false;
                        state.ChangeState (State.onground);
                    });
            }

        } else {
            canvasBackGroundInput.Destroy ();
        }

    }


    public enum State {
        onground,
        ongroundCloesed,
        inpack,
        inhand
    }

}