using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class M_CamManager : MonoBehaviour {
    public MoveToProp moveSet = new MoveToProp ();
    public FollowClass followSet = new FollowClass ();
    public Zoom zoomSet = new Zoom ();

    private void Awake () {
        float size = GetComponent<Camera> ().orthographicSize;
    }


    private void LateUpdate () {
        if (followSet.enable) {
            transform.Set2dPosition ((Vector2) followSet.target.transform.position);
        }
    }



    //*Editor
#if UNITY_EDITOR
    private void OnValidate () {
        if (EditorApplication.isPlaying) {
            if (moveSet.test.move == true) {
                moveSet.test.move = false;
                EditorApplication.delayCall += () => {
                    moveSet.target = moveSet.test.target;
                    gameObject.Ex_Moveto (moveSet.target, moveSet.time, moveCurve : moveSet.moveCurve);
                };
            }


            if (followSet.test.testMove == true) {
                followSet.test.testMove = false;
                EditorApplication.delayCall += () => {
                    Follow (followSet.test.target);
                };
            }


            if (zoomSet.test.testZoom) {
                zoomSet.test.testZoom = false;
                EditorApplication.delayCall += () => {
                    ZoomTo (zoomSet.test.size);
                };

            }
        }



        if (zoomSet.size != GetComponent<Camera> ().orthographicSize) {
            GetComponent<Camera> ().orthographicSize = zoomSet.size;
        }
    }

    private void Reset () {
        zoomSet.size = GetComponent<Camera> ().orthographicSize;
    }
#endif



    //*Public
    public void MoveTo (GameObject target) {
        gameObject.Ex_Moveto (target.transform.position, moveSet.time, moveCurve : moveSet.moveCurve);
    }
    public void Follow (GameObject target) {
        UnityAction callback = () => {
            followSet.enable = true;
            followSet.target = target;
        };
        gameObject.Ex_Moveto (target.transform.position, moveSet.time, callback, moveSet.moveCurve);
    }
    public void StopFollow () {
        followSet.enable = false;
    }
    public void ZoomTo (float size) {
        Camera cam = GetComponent<Camera> ();
        float oriSize = cam.orthographicSize;

        UnityAction<C_AnimateData.Data> onProcess = (d) => {
            cam.orthographicSize = d.floatValue;
        };
        UnityAction<C_AnimateData.Data> onend = (d) => {
            zoomSet.size = d.floatValue;
        };
        gameObject.Ex_AnimateFloat (oriSize, size, zoomSet.animationTime, zoomSet.curve, onProcess, onend);
    }




    //*Property
    [System.Serializable]
    public class MoveToProp {
        public float time = 0;
        public AnimationCurve moveCurve = Fn.Curve.ZeroOneCurve;
        [ReadOnly] public Vector2 target = default;
        public Test test = new Test ();
        [System.Serializable]
        public class Test {
            public bool move = false;
            public Vector2 target = default;
        }
    }

    [System.Serializable]
    public class FollowClass {
        public bool enable = false;
        public GameObject target;
        public Test test = new Test ();
        [System.Serializable]
        public class Test {
            public bool testMove = false;
            public GameObject target;
        }
    }

    [System.Serializable]
    public class Zoom {
        public float size;
        public Test test = new Test ();
        public float animationTime = 0;
        public AnimationCurve curve = Fn.Curve.ZeroOneCurve;
        [System.Serializable]
        public class Test {
            public bool testZoom = false;
            public float size = 0;
        }
    }
}