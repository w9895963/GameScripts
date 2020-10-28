using System.Collections;
using System.Collections.Generic;
using Global;
using UnityEngine;

public class InputManager : MonoBehaviour {
    private void OnEnable () {
        InputUtility.EnableAll ();
    }
    private void OnDisable () {
        InputUtility.DisableAll ();
    }
}