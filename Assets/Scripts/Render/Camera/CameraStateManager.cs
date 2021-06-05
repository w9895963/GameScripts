using System.Collections;
using System.Collections.Generic;
using UnityEngine;




namespace StateM
{
    namespace Comp
    {
        public class CameraStateManager : MonoBehaviour, IStateManager
        {
            private StateManager stateManager = new StateManager();

            public StateManager StateManager => stateManager;

            private void Awake()
            {

            }

        }
    }

    interface IStateManager
    {
        StateManager StateManager { get; }
    }



    public class StateManager
    {
        public List<State> aloneStates = new List<State>();

    }


    public class State
    {

    }

    public static class Func {
        public static void Add () {
        }
    
    }
}
