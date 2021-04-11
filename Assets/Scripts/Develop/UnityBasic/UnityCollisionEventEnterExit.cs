using System.Collections;
using System.Collections.Generic;
using Global;
using UnityEngine;

public class UnityCollisionEventEnterExit : MonoBehaviour
{
    public UnityEventPort unityEventPort;
    UnityEventPort.CallbackData callbackData;
    public List<UnityEventPort.Event> eventList;//show in inspector
    private void Awake()
    {
        unityEventPort = new UnityEventPort();
        callbackData = new UnityEventPort.CallbackData(gameObject);
        eventList = unityEventPort.eventList;
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        callbackData.collisionData = other;
        unityEventPort.RunEventAction(UnityEventPort.EventType.OnCollisionEnter2D, callbackData);
    }
    private void OnCollisionExit2D(Collision2D other)
    {
        callbackData.collisionData = other;
        unityEventPort.RunEventAction(UnityEventPort.EventType.OnCollisionExit2D, callbackData);
    }
}
