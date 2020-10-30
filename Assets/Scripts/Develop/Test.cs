using Global;
using UnityEngine;

public class Test : MonoBehaviour {
    public Object log;

    private void Start () {
        GameObject topLayer = InputUtility.TopLayer;


    }

    private void OnValidate () {
        if (log != null) Debug.Log (log);

    }

    private void Reset () {

    }


    private void LateUpdate () { }
}