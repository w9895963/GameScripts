using System.Collections;
using System.Collections.Generic;
using Global;
using UnityEngine;

public class UnityTriggerEventEnter : MonoBehaviour
{
    public UnityEventPort unityEventPort;
    UnityEventPort.CallbackData callbackData;
    private void Awake()
    {
        unityEventPort = new UnityEventPort();
        callbackData = new UnityEventPort.CallbackData(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        callbackData.colliderData = other;
        unityEventPort.RunEventAction(UnityEventPort.EventType.OnTriggerEnter2D, callbackData);
    }
}
