using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Global;

public class FixedUpdateEvent : MonoBehaviour
{
    public UnityEventPort unityEventPort;
    public List<UnityEventPort.Event> eventList;//show in inspector


    private void Awake()
    {
        unityEventPort = new UnityEventPort();
        eventList = unityEventPort.eventList;

    }

    private void FixedUpdate()
    {
        unityEventPort.RunEventAction(
            UnityEventPort.EventType.FixedUpdate,
            new UnityEventPort.CallbackData(gameObject)
        );
    }



}
