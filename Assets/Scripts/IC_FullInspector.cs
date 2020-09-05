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
        Data = data;
        Behaviours = behaviour;
    }

    public void UnpdateInspector () {
        Debug.Log (000);
        Data = data;
        Data.shareData = data.shareData;
    }



}