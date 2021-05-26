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
                trans.sizeDelta = new Vector2(d.screenDelta.x + s.x, s.y);

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



