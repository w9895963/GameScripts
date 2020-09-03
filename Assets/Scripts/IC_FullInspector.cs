using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IC_FullInspector : IC_Base {
    public new void EmptyBehavior () {
        base.EmptyBehavior ();
        Behaviours = behaviour;
    }

    [SerializeField] private new Data Data = new Data ();
    [ContextMenuItem ("清空", "EmptyBehavior")]
    [SerializeField] private new Behaviours Behaviours = new Behaviours ();

    public void OnValidate () {
        data = Data;
        behaviour = Behaviours;
    }

    public void UnpdateInspector () {
        Data = data;
        Behaviours = behaviour;
    }


}