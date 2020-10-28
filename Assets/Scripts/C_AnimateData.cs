using System.Collections;
using System.Collections.Generic;
using Global;
using UnityEngine;
using UnityEngine.Events;

public class C_AnimateData : MonoBehaviour {
    [System.Serializable] public class Require {
        public float time = 1;
        public Vector3 valueStart = default;
        public Vector3 valueEnd = default;
    }
    public Require require = new Require ();

    [System.Serializable] public class Optional {
        public bool useUnscaleTime = false;
        public bool useCurve = false;
        public AnimationCurve curve = Curve.ZeroOne;
    }
    public Optional optional = new Optional ();
    [System.Serializable] public class Events {
        public UnityEvent<Vector3> onAnimationEnd = new UnityEvent<Vector3> ();
        public UnityEvent<Vector3> onAnimation = new UnityEvent<Vector3> ();
    }
    public Events events = new Events ();

    [ReadOnly] public float timebegin;
    [ReadOnly] public Vector3 value = default;

    [ReadOnly] public Object createBy = null;
    private bool createInEditor = false;

    private void FixedUpdate () {
        float currTime = (optional.useUnscaleTime) ? Time.unscaledTime : Time.time;
        float timeEnd = timebegin + require.time;
        if (optional.useCurve) currTime = Curve.Evaluate (currTime, timebegin, timeEnd, timebegin, timeEnd, optional.curve);
        float delTime = currTime - timebegin;



        if (delTime < require.time) {
            value = (require.valueEnd - require.valueStart) * (delTime / require.time) + require.valueStart;

            events.onAnimation?.Invoke (value);
        } else {
            value = require.valueEnd;

            events.onAnimation?.Invoke (value);
            events.onAnimationEnd?.Invoke (value);
        }
    }

    private void Awake () {
        enabled = createInEditor;
    }
    private void OnValidate () {
        createInEditor = true;
    }

    private void OnEnable () {
        timebegin = (optional.useUnscaleTime) ? Time.unscaledTime : Time.time;

    }

    public class Profile {
        public C_AnimateData source;


        public class FullVersion {
            private C_AnimateData source;

            public FullVersion (C_AnimateData source) {
                this.source = source;
            }

            public Require require { get => source.require; }

            public Optional optional { get => source.optional; }
            public Events events { get => source.events; }
        }
        public FullVersion useFullVersion { get => new FullVersion (source); }
        public class FloatVersion {
            private C_AnimateData source;

            public FloatVersion (C_AnimateData source) {
                this.source = source;
            }

            public class Require {
                private C_AnimateData source;

                public Require (C_AnimateData source) {
                    this.source = source;
                }

                public float time { set => source.require.time = value; }
                public float valueStart { set => source.require.valueStart.x = value; }
                public float valueEnd { set => source.require.valueEnd.x = value; }

            }
            public Require require { get => new Require (source); }

            public Optional optional { get => source.optional; }
            public class Events {
                private C_AnimateData source;

                public Events (C_AnimateData source) {
                    this.source = source;
                    source.events.onAnimation.AddListener ((v) => {
                        onAnimation.Invoke (v.x);
                    });
                    source.events.onAnimationEnd.AddListener ((v) => {
                        onAnimationEnd.Invoke (v.x);
                    });
                }

                public UnityEvent<float> onAnimation = new UnityEvent<float> ();
                public UnityEvent<float> onAnimationEnd = new UnityEvent<float> ();
            }
            public Events events { get => new Events (source); }
        }
        public FloatVersion useFloatVersion { get => new FloatVersion (source); }


        public Profile (C_AnimateData source) {
            this.source = source;
        }
    }




}

public static class Extension_C_AnimateData {
    public static GameObject AnimateFloat (this Function fn, float floatStart, float floatEnd, float time,
        UnityAction<float> onChanged, bool useUnscaleTime = false, AnimationCurve curve = null
    ) {
        GameObject gameObject = GlobalObject.TempObject.CreateChild ("AnimateData");
        C_AnimateData comp = gameObject.AddComponent<C_AnimateData> ();

        comp.require.valueStart.x = floatStart;
        comp.require.valueEnd.x = floatEnd;
        comp.require.time = time;
        comp.optional.useUnscaleTime = useUnscaleTime;
        if (curve != null) {
            comp.optional.useCurve = true;
            comp.optional.curve = curve;
        }
        comp.events.onAnimation.AddListener ((v) => {
            onChanged (v.x);
        });
        comp.events.onAnimationEnd.AddListener ((v) => {
            gameObject.Destroy ();
        });
        comp.createBy = fn.callBy;



        return gameObject;
    }
    public static GameObject AnimateData (this Function fn, UnityAction<C_AnimateData.Profile> setup) {
        GameObject gameObject = GlobalObject.TempObject.CreateChild ("AnimateData");
        C_AnimateData comp = gameObject.AddComponent<C_AnimateData> ();
        C_AnimateData.Profile setting = new C_AnimateData.Profile (comp);

        setup (setting);
        comp.events.onAnimationEnd.AddListener ((v) => gameObject.Destroy ());


        comp.createBy = fn.callBy;
        comp.enabled = true;
        return gameObject;
    }

}