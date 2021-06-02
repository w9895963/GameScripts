using System;
using System.Collections;
using System.Collections.Generic;
using BasicEvent;
using UnityEngine;



namespace UIBundle
{
    namespace Component
    {
        public class DragWidth : MonoBehaviour
        {
            public GameObject target;
            private RectTransform trans;
            private float originWidth;

            void Start()
            {
                BasicEvent.OnPointerDrag.Add(gameObject, onDrag, onEnd, onStart);
            }

            private void onDrag(OnPointerDrag.DragDate d)
            {
                Vector2 s = trans.sizeDelta;
                Vector2 del = d.screenPosition - d.beginScreenPosition;
                float x = del.x / trans.lossyScale.x + originWidth;
                trans.sizeDelta = new Vector2(x.ClampMin(0), s.y);

            }

            private void onEnd(OnPointerDrag.DragDate d)
            {
            }

            private void onStart(OnPointerDrag.DragDate d)
            {
                trans = target.GetComponent<RectTransform>();
                originWidth = trans.sizeDelta.x;
            }
        }
    }
}



