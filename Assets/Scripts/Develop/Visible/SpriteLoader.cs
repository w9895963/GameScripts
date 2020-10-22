using System.Collections;
using System.Collections.Generic;
using Global.Visible;
using UnityEngine;

public class SpriteLoader : MonoBehaviour {
    public List<SpriteIterm> list = new List<SpriteIterm> ();

    private void Awake () {
        var spriteProfiles = Resources.LoadAll<SpriteResouceProfile> ("Visible");
        foreach (var prof in spriteProfiles) {
            SpriteIterm sp = prof.setting.ToSpriteIterm ();
            sp.Load ();
            list.Add (sp);
        }
    }
}