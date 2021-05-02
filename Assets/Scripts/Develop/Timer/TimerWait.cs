using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace Timer
{
    public class TimerWait
    {
        #region Static
        private const string TimerObjectName = "Timer";
        private static GameObject TimerManager;
        private static List<TimerWait> TimerList = new List<TimerWait>();
        private static void CreateTimerManagerIfNotExist()
        {
            if (TimerManager == null)
            {
                TimerManager = new GameObject(TimerObjectName);
                UnityEventFixedUpdate.AddEvent(TimerManager, OnFixedUpdateAction);
            }

        }
        private static void OnFixedUpdateAction()
        {
            TimerList.ToList().ForEach((timer) =>
            {
                if (timer.startTime < 0)
                {
                    timer.startTime = Time.time;
                }

                float last = Time.time - timer.startTime;
                if (last >= timer.waitTime)
                {
                    timer.callbackAction();
                    RemoveTimer(timer);
                }
            });
        }

        private static void RemoveTimer(TimerWait timer)
        {
            TimerList.Remove(timer);
            RemoveTimerManagerIfNoTimer();
        }

        private static void RemoveTimerManagerIfNoTimer()
        {
            if (TimerList.Count == 0)
            {
                TimerManager.Destroy();
            }
        }


        #endregion

        private float startTime = -1;
        private float waitTime;
        private Action callbackAction;

        public TimerWait(float time, Action callback)
        {

            CreateTimerManagerIfNotExist();
            waitTime = time;
            callbackAction = callback;

            TimerList.Add(this);
        }

        public void Stop()
        {
            RemoveTimer(this);
        }





    }

}


