using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Global;
using UnityEngine;

public class M_GlobalObject : MonoBehaviour {
    public GlobalObject instanceName = default;


}

public static class Extention_M_Instance {


    public static GameObject FindGlobalObject (this Fn fn, GlobalObject instance) {
        List<GameObject> gameObjects = FindAllInstance ();
        return gameObjects.Find ((x) => x.GetComponent<M_GlobalObject> ().instanceName == instance);
    }


    //* Private Method
    private static List<GameObject> FindAllInstance () {
        return GameObject.FindObjectsOfType<M_GlobalObject> ().ToList ()
            .Select ((x) => x.gameObject).ToList ();
    }
}

namespace Global {
    public enum GlobalObject {
        None,
        BackpackIcon,
        IndicatorCamera,
    }

}