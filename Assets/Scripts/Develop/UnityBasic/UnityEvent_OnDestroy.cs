using System.Collections;
using System.Collections.Generic;
using Global;
using UnityEngine;

public class UnityEvent_OnDestroy : MonoBehaviour
{
    public UnityEventPort unityEventPort;
    public List<UnityEventPort.Event> eventList;//show in inspector


    private void Awake()
    {
        unityEventPort = new UnityEventPort();
        eventList = unityEventPort.eventList;

    }


    private void OnDestroy()
    {
        unityEventPort.RunEventAction(
                   UnityEventPort.EventType.OnDestroy,
                   new UnityEventPort.CallbackData(gameObject));
    }
}
