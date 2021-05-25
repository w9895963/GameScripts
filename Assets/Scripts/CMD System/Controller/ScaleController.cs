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
            public GameObject body;
            public GameObject handle;
            public Vector2 standartSize = Vector2.one;
            private Vector2 scale;
            private Vector2 localScale;
            private GameObject obj;

            private void onStart(OnPointerDrag.DragDate obj)
            {

            }

            private void onEnd(OnPointerDrag.DragDate d)
            {
                Vector2 vector2 = obj.GetScale2dLo();
                onFinalUpdate?.Invoke(vector2.ToStringArray());
            }

            private void onDrag(OnPointerDrag.DragDate d)
            {
                handle.SetPosition(d.position);
                Vector2 worldCurrScale = handle.GetPosition2dLo().Divide(standartSize);
                obj.SetScale(worldCurrScale);
                body.GetComponent<SpriteRenderer>().size = obj.GetSize() ?? Vector2.one;
            }


            public override void Setup()
            {
                BasicEvent.OnPointerDrag.Add(handle, onDrag, onEnd, onStart);

                obj = base.cl.GameObject;

                standartSize = obj.GetSpriteSize() ?? Vector2.one;
                Vector2 rightTop = obj.GetPosition2d() + (obj.GetSize() ?? Vector2.one) / 2;

                Vector2 sc = obj.transform.lossyScale;
                gameObject.SetPosition(obj.GetPosition2d());
                handle.SetPosition(rightTop);
                body.GetComponent<SpriteRenderer>().size = obj.GetSize() ?? Vector2.one;



                DateF.AddAction<Date.GameObject.Position, Vector2>(obj, (d) =>
                {
                    gameObject.SetPosition(d);
                });
                DateF.AddAction<Date.GameObject.Rotate, float>(obj, (d) =>
                {
                    gameObject.SetRotate(d);
                });
            }

           
        }
    }
}
