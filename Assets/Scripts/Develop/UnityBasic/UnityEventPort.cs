using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace Global
{
    public class UnityEventPort
    {
        public List<Event> eventList = new List<Event>();

        public void AddEventAction(EventType type, int runOrder, UnityAction<CallbackData> action, bool sort = false)
        {
            OrderRun.AddAction(() =>
            {
                if (!eventList.Exists((m) => m.eventType == type))
                {
                    eventList.Add(new Event(type));
                }

                Event evt = eventList.Find((m) => m.eventType == type);


                Event.EventAction ea = new Event.EventAction();
                ea.runOrder = runOrder;
                ea.action = action;
                evt.actionList.Add(ea);

                if (sort)
                {
                    eventList.ForEach((m) =>
                    {
                        m.actionList.Sort((x, y) => x.runOrder.CompareTo(y.runOrder));
                    });
                }
            });


        }
        public void RemoveEventAction(EventType type, UnityAction<CallbackData> action)
        {
            OrderRun.AddAction(() =>
            {
                Event evt = eventList.Find((m) => m.eventType == type);
                if (evt != null)
                {
                    evt.actionList.RemoveAll((x) => x.action == action);
                }
            });

        }


        public void RunEventAction(EventType type, CallbackData data)
        {
            OrderRun.AddAction(() =>
            {
                Event evt = eventList.Find((m) => m.eventType == type);
                if (evt != null)
                {
                    evt.actionList.ForEach((m) =>
                    {
                        m.action(data);
                    });
                }
            });




        }

        [System.Serializable]
        public class Event
        {
            public EventType eventType;
            public List<EventAction> actionList = new List<EventAction>();

            public Event(EventType eventType)
            {
                this.eventType = eventType;
            }
            [System.Serializable]
            public class EventAction
            {
                public int runOrder = 0;
                public UnityAction<CallbackData> action;

            }

        }
        public enum EventType
        {
            FixedUpdate,
            OnDestroy,
            OnCollisionEnter2D,
            OnCollisionStay2D,
            OnCollisionExit2D,
            OnTriggerEnter2D,
            OnTriggerStay2D,
            OnTriggerExit2D
        }

        public class CallbackData
        {
            public GameObject gameObject;
            public Collision2D collisionData;
            public Collider2D colliderData;

            public CallbackData(GameObject gameObject, Collision2D collision2D = null)
            {
                this.gameObject = gameObject;
                this.collisionData = collision2D;
            }
        }



        public static void AddFixedUpdateAction(GameObject gameObject,
                                                int runOrder,
                                                UnityAction<CallbackData> action)
        {
            FixedUpdateEvent fixedUpdateEvent = gameObject.GetComponent<FixedUpdateEvent>();
            if (fixedUpdateEvent == null)
            {
                fixedUpdateEvent = gameObject.AddComponent<FixedUpdateEvent>();
            }
            fixedUpdateEvent.unityEventPort.AddEventAction(EventType.FixedUpdate, runOrder, action);

        }
        public static void RemoveFixedUpdateAction(GameObject gameObject, UnityAction<CallbackData> action)
        {

            FixedUpdateEvent fixedUpdateEvent = gameObject.GetComponent<FixedUpdateEvent>();
            if (fixedUpdateEvent != null)
            {
                fixedUpdateEvent.unityEventPort.RemoveEventAction(EventType.FixedUpdate, action);
            }
        }
        public static void AddOnDestroyAction(GameObject gameObject,
                                               int runOrder,
                                               UnityAction<CallbackData> action)
        {
            UnityEvent_OnDestroy comp = gameObject.GetComponent<UnityEvent_OnDestroy>();
            if (comp == null)
            {
                comp = gameObject.AddComponent<UnityEvent_OnDestroy>();
            }
            comp.unityEventPort.AddEventAction(EventType.OnDestroy, runOrder, action);

        }

        public static void AddCollisionAction(GameObject gameObject,
                                                    int runOrder,
                                                    UnityAction<CallbackData> onCollisionEnter = null,
                                                    UnityAction<CallbackData> onCollisionExit = null)
        {
            if (onCollisionEnter != null | onCollisionExit != null)
            {
                UnityCollisionEventEnterExit comp = gameObject.GetComponent<UnityCollisionEventEnterExit>();
                if (comp == null)
                {
                    comp = gameObject.AddComponent<UnityCollisionEventEnterExit>();
                }
                UnityEventPort port = comp.unityEventPort;
                if (onCollisionEnter != null)
                {
                    port.AddEventAction(EventType.OnCollisionEnter2D, runOrder, onCollisionEnter);
                }
                if (onCollisionExit != null)
                {
                    port.AddEventAction(EventType.OnCollisionExit2D, runOrder, onCollisionExit);
                }
            }

        }
        public static void AddTriggerAction(GameObject gameObject,
                                                    int runOrder,
                                                    UnityAction<CallbackData> onTriggerEnter)
        {
            UnityTriggerEventEnter comp = gameObject.GetComponent<UnityTriggerEventEnter>();
            if (comp == null)
            {
                comp = gameObject.AddComponent<UnityTriggerEventEnter>();
            }
            UnityEventPort port = comp.unityEventPort;
            port.AddEventAction(EventType.OnTriggerEnter2D, runOrder, onTriggerEnter);

        }



    }
}