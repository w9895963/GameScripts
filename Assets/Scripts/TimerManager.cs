using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static Global.Timer;

public class TimerManager : MonoBehaviour
{
    public List<TimerControler> timers = new List<TimerControler>();

    void Update()
    {

        List<TimerControler> calls = new List<TimerControler>();
        timers.ForEach((timer) =>
        {
            float lastTime;
            if (timer.onRealTime)
            {
                lastTime = Time.unscaledTime - timer.beginTime;
            }
            else
            {
                lastTime = Time.time - timer.beginTime;
            }
            if (lastTime >= timer.LastTime)
            {
                calls.Add(timer);
            }
        });

        calls.ForEach((x) =>
        {
            x.onEnd.Invoke();
            if (x.loop)
            {
                x.beginTime = x.beginTime + x.LastTime;
            }
            else
            {
                timers.Remove(x);
            }

        });
        if (timers.Count == 0)
        {
            Destroy(gameObject);
        }

    }




}


namespace Global
{

    public static class Timer
    {
        public static TimerManager TimerManager
        {
            get
            {
                GameObject tempObject = Global.Find.TempObject;
                TimerManager timerManager = tempObject.GetComponentInChildren<TimerManager>();
                if (timerManager == null)
                {
                    var timerObj = tempObject.CreateChild("TimerManager");
                    timerManager = timerObj.AddComponent<TimerManager>();
                }
                return timerManager;
            }
        }
        public static TimerControler BasicTimer(float time, UnityAction callback, bool onRealTime = false, Object creator = null)
        {
            TimerControler timer = new TimerControler();
            if (onRealTime)
            {
                timer.beginTime = Time.unscaledTime;
            }
            else
            {
                timer.beginTime = Time.time;
            }
            timer.LastTime = time;
            timer.onRealTime = onRealTime;
            timer.creator = creator;
            timer.onEnd.AddListener(callback);

            TimerManager.timers.Add(timer);

            return timer;
        }
        public static TimerControler Loop(float time, UnityAction callback, bool onRealTime = false, Object creator = null)
        {
            TimerControler timer = BasicTimer(time, callback, onRealTime, creator);
            timer.loop = true;

            return timer;
        }
        public static TimerControler DynimicLoop(System.Func<float> timeFunc, UnityAction callback, bool onRealTime = false, Object creator = null)
        {
            TimerControler timer = BasicTimer(0, callback, onRealTime, creator);
            timer.loop = true;
            timer.lastTimeFunc = timeFunc;

            return timer;
        }
        public static void Wait(GameObject gameObject, float time, System.Action callback)
        {
            TimerIns timer = new TimerIns(Time.time);
            UnityAction<UnityEventPort.CallbackData> action = null;
            action = (d) =>
            {
                if (Time.time - timer.timeBegin >= time)
                {
                    callback();
                    UnityEventPort.RemoveAction(gameObject, action);
                }
            };
            UnityEventPort.AddFixedUpdateAction(gameObject, 0, action);

        }



        [System.Serializable]
        public class TimerControler
        {
            public float beginTime;
            public System.Func<float> lastTimeFunc;
            public bool onRealTime = false;
            public Object creator;
            public bool loop = false;
            public UnityEvent onEnd = new UnityEvent();

            public float CurrentTime => (onRealTime) ? Time.unscaledTime : Time.time;

            public float LastTime
            {
                get => lastTimeFunc();
                set => lastTimeFunc = () => value;
            }

            public void Stop()
            {
                TimerManager.timers.Remove(this);
            }

        }

        public class TimerIns
        {
            public float timeBegin;

            public TimerIns(float timeBegin)
            {
                this.timeBegin = timeBegin;
            }
        }
    }

}