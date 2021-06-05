    using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace Utility
{
    public class MoveWithCamera : MonoBehaviour
    {
        public Vector2 originPosition;
        public Vector2 moveRate = new Vector2(0.9f, 0.9f);

        void Start()
        {
            originPosition = transform.position;
            DateF.AddAction<Date.GameObject.PositionLo, Vector2>(gameObject, (d) =>
            {
                originPosition = d;
                Vector2 p = (transform.localToWorldMatrix * d.ToVector3());
                originPosition = p;
            });
        }

        void Update()
        {
            Vector2 camP = Camera.main.transform.position;
            Vector2 P = gameObject.GetPosition2d();
            Vector2 dP = originPosition - camP;
            float x = moveRate.x.Map(0, 1, originPosition.x, camP.x, false);
            float y = moveRate.y.Map(0, 1, originPosition.y, camP.y, false);
            dP.Scale(moveRate);

            Vector2 targetP = camP + dP;
            gameObject.SetPosition(x, y);
        }
    }
}
