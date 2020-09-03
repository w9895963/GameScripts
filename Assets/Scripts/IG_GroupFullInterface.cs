using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IG_GroupFullInterface : IG_GroupCore {
    public Property Setting = new Property ();




    public new void OnValidate () {
        property = Setting;
        base.OnValidate ();
    }


}