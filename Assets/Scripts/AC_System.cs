using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AC_System : MonoBehaviour {
    public void _ReloadScence () {
        SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
    }
}