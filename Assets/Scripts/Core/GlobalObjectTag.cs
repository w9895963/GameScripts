using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Global;
using UnityEngine;

public class GlobalObjectTag : MonoBehaviour {
    public GlobalObject.Type instanceName = default;


}


namespace Global {

    public static class GlobalObject {
        public static GameObject IndicatorCamera { get => Get (Type.IndicatorCamera); }
        public static GameObject BackpackIcon { get => Get (Type.BackpackIcon); }
        public static GameObject MainCharactor { get => GameObject.FindObjectOfType<CM_MainCharacter> ().gameObject; }
        public static GameObject Cursor => GameObject.FindObjectOfType<M_Cursor> ().gameObject;
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
            return gameObjects.Find ((x) => x.GetComponent<GlobalObjectTag> ().instanceName == type);
        }


        private static List<GameObject> GetAll () {
            return GameObject.FindObjectsOfType<GlobalObjectTag> ().ToList ()
                .Select ((x) => x.gameObject).ToList ();
        }

        public enum Type {
            Null,
            BackpackIcon,
            IndicatorCamera,
        }

    }

}