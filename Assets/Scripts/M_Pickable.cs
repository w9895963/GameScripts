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
    private Component inputEvent;
    private C_ColliderEvent grabZoneEvent;
    private bool indicateMode = false;
    private EventTrigger backpackButtonEvent;
    public C_StateMachine<State> state;
    private bool passNextTest;
    private EventTrigger canvasBackGroundInput;
    private State lastState;

    private void Awake () {
        state = new C_StateMachine<State> ();




        state.AddEnterListener (() => {
            GrabZoneEventSetup (true);
        }, State.onground);
        state.AddEnterListener (() => {
            GrabZoneEventSetup (false);
        }, State.animateToPack);



        state.AddEnterListener (() => {
            AnimateToPack ();
        }, State.animateToPack);




        state.AddListener (State.ongroundCloesed, () => {
            InputEventSetup (true);
        }, () => {
            InputEventSetup (false);
        });




        state.AddListener (State.inpack, () => {
            if (backpackButtonEvent == null) {
                backpackButtonEvent = Gb.BackpackButton.Ex_AddInputToTrigger (
                    EventTriggerType.PointerClick, (d) => {
                        state.ChangeState (State.inhand);
                    });
            }
        }, () => {
            backpackButtonEvent.Destroy ();
        });

        state.AddListener (State.inpack, () => {
            Gb.Backpack.PutinStorage (gameObject);
            gameObject.Ex_Hide ();
        }, () => {
            Gb.Backpack.PutoutStorage (0);
            gameObject.Ex_Show ();
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


        state.AddListener (State.fixedOnWall,
            () => {
                I_Grab m_Grab = GetComponent<I_Grab> ();
                if (m_Grab) m_Grab.enabled = true;
            }, () => {
                I_Grab m_Grab = GetComponent<I_Grab> ();
                if (m_Grab) m_Grab.enabled = false;
            });


        state.AddListenerToAll (() => {
            objectState = state.CurrentState;
        });



        state.SetState (State.onground);
        state.InvokeEnterEvent (State.onground);
    }

    private void Start () {


    }


    private void OnDisable () {
        lastState = state.CurrentState;
        state.ChangeState (State.disable);
    }
    private void OnEnable () {
        if (state.IsCurrent (State.disable)) {
            state.ChangeState (lastState);
        }
    }

    private void Update () {
        if (indicateMode) {
            transform.Set2dPosition (Pointer.current.position.ReadValue ().ScreenToWold ());
        }
    }


#if UNITY_EDITOR
    private void OnValidate () {
        UnityEditor.EditorApplication.delayCall += () => {
            state?.ChangeState (objectState);
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
                    state.ChangeState (State.animateToPack);
                }
            };


            if (inputEvent == null) {
                inputEvent = this.Ex_AddInputToTriggerOnece (clickBox, EventTriggerType.PointerClick, onclick);
                inputEventEnable = true;
            }


        } else {
            inputEvent.Destroy ();
            inputEventEnable = false;
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
                grabZoneEvent = Gb.MainCharactor.GrabBox.Ex_AddCollierEvent (null, enter, exit);
            }

        } else {
            grabZoneEventEnable = false;
            grabZoneEvent.Destroy ();
        }

    }

    public void IndicateModeSetup (bool enabled) {
        indicateMode = enabled;
        if (enabled) {


            Gb.CanvasBackGround.enabled = true;
            if (canvasBackGroundInput == null) {
                canvasBackGroundInput = this.Ex_AddInputToTriggerOnece (Gb.CanvasBackGround.gameObject,
                    EventTriggerType.PointerClick, (d) => {
                        ////////////////////////////////
                        Gb.CanvasBackGround.enabled = false;
                        M_FixedTo fixComp = GetComponent<M_FixedTo> ();
                        if (fixComp.TryToConnect ()) {
                            state.ChangeState (State.fixedOnWall);
                        } else {
                            state.ChangeState (State.onground);
                        }
                    });
            }

        } else {
            canvasBackGroundInput.Destroy ();
        }

    }

    private void AnimateToPack () {
        RectTransform rectTransform = Gb.BackpackButton.GetComponent<RectTransform> ();
        Vector2 targetPosition = Gb.MainCharactor.transform.position;
        gameObject.Ex_Moveto (targetPosition, 0.5f);
        float scale = gameObject.transform.localScale.x;
        this.Ex_AnimateFloat (scale, 0, 0.5f,
            onAnimate: (f) => {
                gameObject.transform.localScale = new Vector3 (f, f, 1);
            }, onAnimateEnd: (f) => {
                gameObject.transform.localScale = new Vector3 (scale, scale, 1);
                state.ChangeState (State.inpack);
            });
    }


    public enum State {
        onground,
        ongroundCloesed,
        animateToPack,
        inpack,
        inhand,
        fixedOnWall,
        disable
    }

}