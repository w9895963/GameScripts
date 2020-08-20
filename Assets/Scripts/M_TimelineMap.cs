using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

public class M_TimelineMap : MonoBehaviour {
    public PlayableDirector timeline = null;
    public bool enableMapPosition = false;
    public MapPosition mapPosition = new MapPosition ();
    public bool enableMapRotation = false;
    public MapRotation mapRotation;
    public Events[] events;
    private double lastTime;


    void Update () {
        //*Map Position
        if (enableMapPosition) {
            Vector2 position = mapPosition.gameObject.transform.position;
            Vector2 startPoint = mapPosition.positionStart;
            Vector2 endPoint = mapPosition.positionEnd;

            float length = (float) timeline.duration;

            float rate = (position - startPoint).magnitude / (endPoint - startPoint).magnitude;


            timeline.time = rate * length;
        }


        //*Map Rotation
        if (enableMapRotation) {
            float rotate = mapRotation.gameObject.GetComponent<Rigidbody2D> ().rotation;
            float lastRotate = mapRotation.lastRotate;


            timeline.time += (rotate - lastRotate) * mapRotation.angleToSpeed;
            timeline.Evaluate ();
            // timeline.u
            // Debug.Log (timeline.time);


            mapRotation.lastRotate = rotate;
        }


        //*Events
        if (events.Length > 0) {

            foreach (var ev in events) {
                if ((timeline.time - ev.time) * (lastTime - ev.time) <= 0) {
                    ev.onTime.Invoke ();
                }
            }

            lastTime = timeline.time;
        }
    }


    private void OnEnable () {
        lastTime = timeline.time;

        if (enableMapRotation) {
            mapRotation.lastRotate = mapRotation.gameObject.GetComponent<Rigidbody2D> ().rotation;
        }
    }



    //*Property Class
    [System.Serializable]
    public class MapPosition {
        public GameObject gameObject;
        public Vector2 positionStart = default;
        public Vector2 positionEnd = default;
    }

    [System.Serializable]
    public class Events {
        public float time = 1f;
        public UnityEvent onTime;
    }

    [System.Serializable]
    public class MapRotation {
        public GameObject gameObject;
        public float angleToSpeed = 0.01f;
        public float lastRotate;
    }
    private void OnValidate () {
        if (timeline == null) timeline = GetComponent<PlayableDirector> ();
    }


}