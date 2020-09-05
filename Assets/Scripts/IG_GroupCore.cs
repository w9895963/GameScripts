using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IG_GroupCore : IC_Base {
    [HideInInspector] public Property property = new Property ();
    [System.Serializable] public class Property {
        public string currentState;
        public List<string> allowStates = new List<string> ();
        public List<IC_Base> setOn = new List<IC_Base> ();
        public List<IC_Base> setOff = new List<IC_Base> ();
        public List<IG_GroupCore> group = new List<IG_GroupCore> ();
    }



    public void OnEnable () {
        property.setOn.Distinct ();
        Fn._.OrderRun (() => {
            property.setOn.ForEach ((x) => {

                if (x) x.enabled = true;
            });
        });


    }

    public void OnDisable () {
        property.setOff.Distinct ();
        Fn._.OrderRun (() => {
            property.setOff.ForEach ((x) => {
                if (x) x.enabled = false;
            });
        });

    }


    public void OnValidate () {
        UpdateGroup (property.group);
    }


    public void UpdateGroup (List<IG_GroupCore> group) {
        group.RemoveAll ((x) => x == null);
        group.AddNotHas (this);
        var allReferMenbers = group.SelectMany ((x) => x.property.group).Distinct ().ToList ();
        allReferMenbers.Remove (null);
        bool findAll = (allReferMenbers.Count == this.property.group.Count);

        group.ForEach ((menber) => {
            menber.property.group = allReferMenbers;
        });
        if (!findAll) {
            UpdateGroup (group);
        }
        PropertyChanged (property.currentState);
    }
    public void Changestate (string stateName) {
        Fn._.OrderRun (() => {
            if (!string.IsNullOrWhiteSpace (stateName)) {
                property.group.ForEach ((x) => {
                    x.property.currentState = stateName;
                    if (x.property.allowStates.Contains (stateName)) {
                        x.enabled = true;
                    } else {
                        x.enabled = false;
                    }
                });
            }
        });



        PropertyChanged (property.currentState);

    }

    public virtual void PropertyChanged (string current) {

    }
}