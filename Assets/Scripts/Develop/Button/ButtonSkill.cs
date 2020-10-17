using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSkill : MonoBehaviour, IPointerClickHandler {
    public string skillName;
    public void OnPointerClick (PointerEventData eventData) {
        SkillObject skillObject = Skill.Find (skillName);
        if (skillObject)
            skillObject.Cast ();
    }

}



public static class Skill {
    private static SkillManager skillManager;
    private static List<SkillObject> skills;

    public static List<SkillObject> Skills {
        get {
            if (skills == null) {
                FindAllSkills ();
            }
            return skills;
        }
    }

    public static SkillManager SkillManager {
        get {
            if (skillManager == null) {
                skillManager = GameObject.FindObjectOfType<SkillManager> ();
            }
            return skillManager;
        }
    }

    public static SkillObject Find (string name) {
        SkillObject skill = Skills.Find ((x) => x.name == name);
        if (skill) {
            return skill;
        } else {
            return null;
        }
    }


    public static void FindAllSkills () {
        skills = SkillManager.GetComponentsInChildren<SkillObject> ().ToList ();
    }
}