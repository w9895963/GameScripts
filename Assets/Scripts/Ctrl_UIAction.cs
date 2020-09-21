using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ctrl_UIAction : MonoBehaviour {
    public void _ReverseGravity () {
        FindObjectOfType<M_PlayerManager> ().ReverseGravity ();
    }

    public void _Scence_Load (string name) {
        SceneManager.LoadScene (name);
    }
    public void _Scence_Load (int index) {
        if (SceneManager.GetActiveScene ().buildIndex != index) {
            SceneManager.LoadScene (index);
        }
    }



}