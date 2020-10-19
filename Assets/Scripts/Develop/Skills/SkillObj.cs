using System.Collections;
using System.Collections.Generic;
using Global;
using Global.Skill;
using UnityEngine;

public class SkillObj : MonoBehaviour {

    public SkillProfile skillProfile = new SkillProfile ();
    [System.Serializable] public class SkillProfile {
        public string skillName;
        public SkillType skillType;
        public ISkillTarget target;

    }




    public void Cast () {
       



    }
}