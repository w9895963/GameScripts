using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static Global.Timer;

public class TimerManager : MonoBehaviour {
    public List<TimerControler> timers = new List<TimerControler> ();

    void Update () {

        List<TimerControler> calls = new List<TimerControler> ();
        timers.ForEach ((timer) => {
            float lastTime;
            if (timer.onRealTime) {
                lastTime = Time.unscaledTime - timer.beginTime;
            } else {
                lastTime = Time.time - timer.beginTime;
            }
            if (lastTime >= timer.lastTime) {
                calls.Add (timer);
            }
        });

        calls.ForEach ((x) => {
            x.onEnd.Invoke ();
            if (x.loop) {
                x.beginTime = x.beginTime + x.lastTime;
            } else {
                timers.Remove (x);
            }

        });
        if (timers.Count == 0) {
            Destroy (gameObject);
        }

    }




}


namespace Global {

    public static class Timer {
        public static TimerManager TimerManager {
            get {
                GameObject tempObject = GlobalObject.TempObject;
                TimerManager timerManager = tempObject.GetComponentInChildren<TimerManager> ();
                if (timerManager == null) {
                    var timerObj = tempObject.AddChild ("TimerManager");
                    timerManager = timerObj.AddComponent<TimerManager> ();
                }
                return timerManager;
            }
        }
        public static TimerControler WaitToCall (float time, UnityAction callback, bool onRealTime = false, Object creator = null) {
            TimerControler timer = new TimerControler ();
            if (onRealTime) {
                timer.beginTime = Time.unscaledTime;
            } else {
                timer.beginTime = Time.time;
            }
            timer.lastTime = time;
            timer.onRealTime = onRealTime;
            timer.creator = creator;
            timer.onEnd.AddListener (callback);

            TimerManager.timers.Add (timer);

            return timer;
        }
        public static TimerControler Loop (float time, UnityAction callback, bool onRealTime = false, Object creator = null) {
            TimerControler timer = WaitToCall (time, callback, onRealTime, creator);
            timer.loop = true;

            return timer;
        }

        [System.Serializable]
        public class TimerControler {
            public float beginTime;
            public float lastTime;
            public bool onRealTime = false;
            public Object creator;
            public bool loop = false;
            public UnityEvent onEnd = new UnityEvent ();

            public void Stop () {
                TimerManager.timers.Remove (this);
            }

        }
    }

}