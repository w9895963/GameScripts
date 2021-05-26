using System.Collections;
using System.Collections.Generic;
using BasicEvent;
using UnityEngine;




namespace CMDBundle
{
    namespace Controller
    {
        public class PositionController : Controller
        {

            private void onStart(OnPointerDrag.DragDate d)
            {

            }

            private void onEnd(OnPointerDrag.DragDate d)
            {
                onFinalUpdate?.Invoke(cl.GameObject.GetPosition2dLo().ToStringArray());
            }

            private void onDrag(OnPointerDrag.DragDate d)
            {
                cl.GameObject.SetPosition(d.position);
                gameObject.SetPosition(d.position);
                onUpdate?.Invoke(cl.GameObject.GetPosition2dLo().ToStringArray());
            }


            public override void Setup()
            {
                BasicEvent.OnPointerDrag.Add(gameObject, onDrag, onEnd, onStart);
                var prs = cl.paramaters;
                if (prs.IsEmpty()) return;

                gameObject.SetPosition(cl.GameObject.GetPosition2d());


            }
        }
    }
}
