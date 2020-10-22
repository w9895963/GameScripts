using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Global {
    namespace Iterm {
        public class ItemUtility {
            public static class PreMakeIterm {
                public static class Names {
                    public static string folder = "Item";
                    public static string GravityBomb = "GravityBomb";
                }
                private static List<GameObject> all;
                private static List<GameObject> All =>
                    all != null?all : all = Resources.LoadAll<GameObject> (Names.folder).ToList ();


                public static GameObject GravityBomb => All.Find ((x) => x.name == Names.GravityBomb);

            }

        }

    }
}