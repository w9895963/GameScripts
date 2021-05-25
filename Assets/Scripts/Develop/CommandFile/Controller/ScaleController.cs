using System;
using System.Collections;
using System.Collections.Generic;
using BasicEvent;
using UnityEngine;
namespace CommandFileBundle
{
    namespace Controller
    {
        public class ScaleController : Controller
        {
            private GameObject body;
            private GameObject handle;
            private Vector2 scale;

            private void onStart(OnPointerDrag.DragDate obj)
            {

            }

            private void onEnd(OnPointerDrag.DragDate d)
            {
                onFinalUpdate?.Invoke(scale.ToStringArray());
            }

            private void onDrag(OnPointerDrag.DragDate d)
            {
                handle.SetPosition(d.position);
                Vector2 ps = cl.GameObject.GetParent().GetScale2d();
                Vector2 wantScale = handle.GetPositionLocal2d();
                scale = new Vector2(wantScale.x / ps.x, wantScale.y / ps.y);
                body.SetScale(wantScale);
                onUpdate?.Invoke(scale.ToStringArray());
            }


            public override void Setup(string[] ps)
            {
                body = gameObject.FindChild("缩放控制器身");
                handle = gameObject.GetComponentInChildren<BoxCollider2D>().gameObject;
                BasicEvent.OnPointerDrag.Add(handle, onDrag, onEnd, onStart);
                if (ps.IsEmpty()) return;
                float?[] vs = ps.TryFloat();


                GameObject obj = base.cl.GameObject;
                cl.commandFile.afterFileExecute += () =>
                {
                    Vector2 sc = obj.transform.lossyScale;
                    body.SetScale(sc);
                    handle.SetPositionLocal(sc);
                    gameObject.SetPosition(obj.GetPosition2d());
                };



                // gameObject.SetParent(obj.GetParent());


                DateF.AddAction<Date.GameObject.Position, Vector2>(obj, (d) =>
                {
                    gameObject.SetPosition(d);
                });
            }


        }
    }
}
