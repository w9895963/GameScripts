using System.Collections;
using System.Collections.Generic;
using Global;
using Global.Skill;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkillHolder : MonoBehaviour {

    void OnEnable () {
        GameObject.FindObjectsOfType<Obstacle> ().ForEach ((obj) => {
            InputUtility.AddPointerEvent (obj.gameObject, EventTriggerType.PointerClick, (d) => {
                
            });
        });

    }
    void OnDisable () {

    }

}