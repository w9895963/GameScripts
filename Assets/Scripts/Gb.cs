using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gb : MonoBehaviour {
    public M_PlayerManager mainCharactor = null;
    public static M_PlayerManager MainCharactor = null;

    private void Reset () {
        mainCharactor = mainCharactor?mainCharactor : FindObjectOfType<M_PlayerManager> ();
        MainCharactor = mainCharactor;
    }


}