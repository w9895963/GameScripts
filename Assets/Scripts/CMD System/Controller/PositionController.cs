using System.Collections;
using System.Collections.Generic;
using BasicEvent;
using UnityEngine;




namespace CommandFileBundle
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
                onFinalUpdate?.Invoke(cl.GameObject.GetPositionLocal2d().ToStringArray());
            }

            private void onDrag(OnPointerDrag.DragDate d)
            {
                cl.GameObject.SetPosition(d.position);
                gameObject.SetPosition(d.position);
                onUpdate?.Invoke(cl.GameObject.GetPositionLocal2d().ToStringArray());
            }


            public override void Setup(string[] prs)
            {
                BasicEvent.OnPointerDrag.Add(gameObject, onDrag, onEnd, onStart);
                if (prs.IsEmpty()) return;
                cl.commandFile.afterFileExecute += () => gameObject.SetPosition(cl.GameObject.GetPosition2d());

            }
        }
    }
}
