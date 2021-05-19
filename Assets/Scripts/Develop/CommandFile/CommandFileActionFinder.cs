using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace CommandFileBundle
{
    public static class ActionFinder
    {
        static Dictionary<string, Action<CommandLine>> actDict = new Dictionary<string, Action<CommandLine>>()
        {
            {"预设",Action.PrefabCreate.Act},
            {"粒子起始大小范围",Action.Particle_ParticleStartSizeRange.Act},
            {"粒子贴图",Action.Particle_TextureReplace.Act},
            {"图层",Action.RenderSortingLayer.Act},
            {"贴图",Action.SpritRenderLoadSprite.Act},
            {"添加刚体层",Action.ColliderBoxAdd.Act},
            {"显示辅助图形",Action.ShowIndicator.Act},
        };



        public static Action<CommandLine> FindAction(string CommandTitle)
        {
            Action<CommandLine> act = null;
            actDict.TryGetValue(CommandTitle, out act);
            return act;
        }
    }

}
