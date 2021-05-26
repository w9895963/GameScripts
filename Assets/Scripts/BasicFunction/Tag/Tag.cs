using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TagBundle;



namespace TagBundle
{
    public class TagObj
    {
        private string tagName;

        public TagObj(string tagName)
        {
            this.tagName = tagName;
        }

        public GameObject Object => GameObject.FindGameObjectWithTag(tagName);


    }

}


public static class TagFinder
{
    public static TagObj PostEffect = new TagObj("PostEffect");
    public static TagObj CommandEditor = new TagObj("CommandEditor");
}
