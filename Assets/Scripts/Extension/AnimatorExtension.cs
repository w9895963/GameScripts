using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Animator_Extension
{
    public static void PlayIfExist(this Animator ani, string name)
    {
        RuntimeAnimatorController control = ani.runtimeAnimatorController;
        if (control == null) { return; }
        AnimationClip[] clips = control.animationClips;
        if (clips.Length == 0) { return; }

        bool animationExist = clips.Any((clip) => clip.name == name);
        if (animationExist)
        {
            ani.Play(name);
        }
    }

}
