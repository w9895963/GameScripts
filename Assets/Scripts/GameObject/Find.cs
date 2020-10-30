using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Global {
    public static class Find {
        private static GameObject player;
        private static GameObject cursor;
        private static GameObject indicateCamera;
        private static GameObject ui;
        private static GameObject tempObject;


        public static GameObject Player => FindObj<PlayerMnager> (ref player);
        public static PlayerMnager PlayerComp => Player.GetComponent<PlayerMnager> ();


        public static GameObject IndicateCamera => FindObj (ref indicateCamera, Tag.IndicateCamera);
        public static GameObject UI => FindObj (ref ui, Tag.UI);


        public static GameObject TempObject => tempObject?tempObject : tempObject = new GameObject ("TempGameObject");




        private static GameObject FindObj<T> (ref GameObject refField) where T : MonoBehaviour {
            if (refField == null) {
                refField = GameObject.FindObjectOfType<T> ().gameObject;
            }
            return refField;
        }
        private static GameObject FindObj (ref GameObject refField, Tag tag) {
            if (refField == null) {
                refField = GameObject.FindObjectsOfType<FindTag> ()
                    .First ((x) => x.findTag == tag).gameObject;
            }
            return refField;
        }


        public enum Tag {
            None,
            UI,
            IndicateCamera
        }

    }
}