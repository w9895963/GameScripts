using System;
using System.Collections;
using System.Collections.Generic;
using BasicEvent;
using UnityEngine;



namespace CommandFileBundle
{
    namespace Controller
    {
        public class RotateController : Controller
        {
            private float startAngle;



            private void onStart(OnPointerDrag.DragDate d)
            {
                startAngle = gameObject.GetRotate1D();
            }

            private void onEnd(OnPointerDrag.DragDate d)
            {
                onFinalUpdate?.Invoke(new string[1] { gameObject.GetRotate1D().ToString() });
            }

            private void onDrag(OnPointerDrag.DragDate d)
            {
                var center = gameObject.GetPosition2d();
                float endAngle = Vector2.right.SignedAngle(d.position - center);
                gameObject.SetRotate(endAngle);
                onUpdate?.Invoke(new string[1] { gameObject.GetRotate1D().ToString() });
            }


            public override void Setup(string[] allParams)
            {
                BasicEvent.OnPointerDrag.Add(gameObject, onDrag, onEnd, onStart);
                if (allParams.IsEmpty()) return;
                gameObject.SetRotate(float.Parse(allParams[0]));


                GameObject obj = base.cl.GameObject;
                // gameObject.SetParent(obj.GetParent());


                gameObject.SetPosition(obj.GetPosition2d());

                DateF.AddAction<Date.GameObject.Position, Vector2>(obj, (d) =>
                {
                    gameObject.SetPosition(d);
                });


            }


        }
    }
}
