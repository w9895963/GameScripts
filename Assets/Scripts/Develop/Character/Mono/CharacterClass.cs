using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Global {
    namespace Character {
        public class CharacterClass : MonoBehaviour, ICharacter {
            public string id;

            public string ID => id;
        }
    }

}