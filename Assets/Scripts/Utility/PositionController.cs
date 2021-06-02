using System;
using System.Collections;
using System.Collections.Generic;
using BasicEvent;
using UnityEngine;
using UnityEngine.Events;




namespace Utility
{
    public class PositionController : MonoBehaviour
    {
        public GameObject parent;
        public Action onDrag;
        public Action onDragStart;
        public Action onDragEnd;
        private Vector2 startP;

        private void Start()
        {
            parent = gameObject.GetParent();
            BasicEvent.OnPointerDrag.Add(gameObject, OnDrag, OnEnd, OnStart);
        }

        private void OnStart(OnPointerDrag.DragDate d)
        {
            startP = parent.GetPosition2d();
            onDragStart?.Invoke();
        }

        private void OnEnd(OnPointerDrag.DragDate d)
        {
            onDragEnd?.Invoke();
        }

        private void OnDrag(OnPointerDrag.DragDate d)
        {
            Vector2 vector2 = d.position - d.beginPosition;
            parent.SetPosition(startP + vector2);
            onDrag?.Invoke();
        }
    }
}