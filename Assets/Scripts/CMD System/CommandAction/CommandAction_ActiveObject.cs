using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace CommandFileBundle
{
    namespace ActionComponent
    {
        public class CommandAction_ActiveObject : CommandActionHolder
        {

            public override void Action(CommandLine cl)
            {
                cl.GameObject.SetActive(true);

            }




        }
    }

}
