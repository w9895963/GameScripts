using System;
using System.Collections;
using System.Collections.Generic;
using BasicEvent;
using UnityEngine;



namespace CMDBundle
{
    namespace Controller
    {
        public class RotateController : Controller
        {
            GameObject obj;
            private float startAngle;



            private void onStart(OnPointerDrag.DragDate d)
            {
                startAngle = gameObject.GetRotate1DLo();
            }

            private void onEnd(OnPointerDrag.DragDate d)
            {
                onFinalUpdate?.Invoke(new string[1] { gameObject.GetRotate1DLo().ToString() });
            }

            private void onDrag(OnPointerDrag.DragDate d)
            {
                var center = gameObject.GetPosition2d();
                float endAngle = Vector2.right.SignedAngle(d.position - center);
                gameObject.SetRotate(endAngle);
                obj.SetRotate(endAngle);
                var list = obj.GetAllChild();


                obj.GetAllChildAndSelf().ForEach((o) =>
                {
                    ; DateF.AddDate<Date.GameObject.Rotate, float>(o, o.GetRotate1D());
                });
            }


            public override void Setup()
            {
                BasicEvent.OnPointerDrag.Add(gameObject, onDrag, onEnd, onStart);
                var allParams = cl.paramaters;
                if (allParams.IsEmpty()) return;

                gameObject.SetRotate(float.Parse(allParams[0]));


                obj = base.cl.GameObject;
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
