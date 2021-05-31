using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EditableBundle.DateType;
using System.Linq;

namespace EditableBundle
{
    namespace EditDateGenerator
    {
        public class SpriteDateGen : SingleGen
        {
            PrefabBundle.Prefab[] allowList = new PrefabBundle.Prefab[]{
                PrefabI.SceneLayer
            };
            public override EditDate EditDateGen(GameObject gameObject)
            {
                bool v = allowList.ToList().Exists((x) => x.IsPrefabOf(gameObject));
                if (!v) return null;
                return new Editable_Sprite();
            }
        }
    }

    namespace DateType
    {
        public class Editable_Sprite : EditDate
        {
            public override BuildUiConfig BuildUiConfig => new BuildUiConfig()
            {
                title = "贴图属性",
                paramNames = new[] { "贴图", "法线贴图" },
            };


            public override System.Object[] GetDate()
            {
                SpriteRenderer ren = gameObject.GetComponent<SpriteRenderer>();
                Texture2D texture = ren.sprite.texture;
                Texture nor = ren.sharedMaterial.GetTexture("_NormalMap");
                System.Object[] re = new System.Object[] { texture, nor };
                return re;
            }

            public override void ApplayDate(System.Object[] dates)
            {
                gameObject.SetPositionLo((float?)dates[0], (float?)dates[1]);
            }


        }
    }
}
