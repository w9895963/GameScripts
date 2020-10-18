using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class InterfaceHolder : MonoBehaviour {

}


namespace Global {
    public static class Extension_InterfaceHolder {
        public static List<T> FindAllInterfaces<T> (this Function fn) where T : class {
            InterfaceHolder[] holders = GameObject.FindObjectsOfType<InterfaceHolder> ();
            return holders.ToList ().Select ((x) => x as T).Distinct ().ToList ();
        }
    }
}