using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Global {
    public static class ObjectManager {
        private static GameObject player;

        public static GameObject Player => player?player : player = Function.FindObjectWithInterfaces<IPlayer> ();
    }
}