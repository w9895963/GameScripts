using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;



namespace Physic
{
    public static class Method
    {
      

    }

    public class PIDcontrol
    {
        public Basic basic = new Basic();
        public class Basic
        {
            public float deltaRate = 0.3f;
            public float changedRate = 45f;
        }
        public Optional optional = new Optional();
        public class Optional
        {
            public bool enableMax = false;
            public float maximum = 60;
        }
        private bool initial = false;
        private Vector2 integrate = default;
        private Vector2 lastError;

        public PIDcontrol() { }
        public PIDcontrol(float deltaRate = 0.3f, float changedRate = 45f)
        {
            basic.deltaRate = deltaRate;
            basic.changedRate = changedRate;
        }


        public Vector2 CalcOutput(Vector2 error)
        {
            var s = basic;
            if (!initial)
            {
                lastError = error;
                initial = true;
            }
            Vector2 output;
            Vector2 delta = error - lastError;
            Vector2 wantDel = -error * s.deltaRate;
            Vector2 valueAdd = (wantDel - delta) * s.changedRate;
            integrate += -valueAdd;
            if (optional.enableMax) integrate = integrate.ClampMax(optional.maximum);

            lastError = error;
            output = integrate;

            float index = Time.time * 20;

            return output;
        }
    }



}
