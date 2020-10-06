using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Global;
using UnityEngine;

public class M_GlobalObject : MonoBehaviour {
    public GlobalObject.Type instanceName = default;


}


namespace Global {

    public static class GlobalObject {
        public static GameObject IndicatorCamera { get => Get (Type.IndicatorCamera); }
        public static GameObject BackpackIcon { get => Get (Type.BackpackIcon); }
        public static GameObject MainCharactor { get => GameObject.FindObjectOfType<M_PlayerManager> ().gameObject; }
        public static GameObject Camera { get => UnityEngine.Camera.main.gameObject; }
        public static GameObject TempObject {
            get {
                GameObject temp = GameObject.Find ("TempGameObject");
                if (temp == null) {
                    temp = new GameObject ("TempGameObject");
                }
                return temp;
            }
        }




        public static GameObject Get (Type type) {
            List<GameObject> gameObjects = GetAll ();
            return gameObjects.Find ((x) => x.GetComponent<M_GlobalObject> ().instanceName == type);
        }


        private static List<GameObject> GetAll () {
            return GameObject.FindObjectsOfType<M_GlobalObject> ().ToList ()
                .Select ((x) => x.gameObject).ToList ();
        }

        public enum Type {
            Null,
            BackpackIcon,
            IndicatorCamera,
        }

    }

}