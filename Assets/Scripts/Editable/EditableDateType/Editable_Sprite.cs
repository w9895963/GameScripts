using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EditableBundle.DateType;
using System.Linq;
using EditableBundle.Comp;
using System;
using System.IO;

namespace EditableBundle
{
   

    namespace DateType
    {
        public class Editable_Sprite : EditDate
        {
            private SpriteRenderer render;

            public override BuildUiConfig UiConfig => new BuildUiConfig()
            {
                title = "贴图属性",
                paramNames = new[] { "贴图", "法线贴图" },
            };

            public override GameObject[] UiObjects
            {
                get
                {
                    List<GameObject> re = new List<GameObject>();
                    re.Add(Func.Editor_ListAllEditableDate.DefaultUiTitleBuildMethod(this));

                    GameObject titleTextObj = PrefabI.UI_EditorItem_TitleTexture.CreateInstance((obj) =>
                    {
                        CompItemTexture com = obj.GetComponent<CompItemTexture>();
                        com.titleText = UiConfig.paramNames[0];
                        render = gameObject.GetComponent<SpriteRenderer>();
                        com.SetTexture(render.sprite.texture);
                        com.AddOnClickAction(SetSpriteAction);
                        re.Add(obj);
                    });


                    GameObject titleTextObj2 = PrefabI.UI_EditorItem_TitleTexture.CreateInstance((obj) =>
                    {
                        CompItemTexture com = obj.GetComponent<CompItemTexture>();
                        com.titleText = UiConfig.paramNames[1];
                        Texture texture = render.sharedMaterial.GetTexture("_NormalMap");
                        if (texture != null) { com.SetTexture(texture); }
                        com.AddOnClickAction(SetNormalAction);
                        re.Add(obj);
                    });


                    return re.ToArray();
                }
            }

            private void SetNormalAction()
            {
                Func.Editor_TextureSelector.ShowFolderTexture((path) =>
                 {
                     Texture2D texture2D = FileF.LoadTexture(path);
                     render.sharedMaterial.SetTexture("_NormalMap", texture2D);
                     EditableF.ShowObjectEditor(gameObject);
                 });
            }

            private void SetSpriteAction()
            {

                Func.Editor_TextureSelector.ShowFolderTexture((path) =>
                {
                    Sprite sprite = FileF.LoadSprite(path);
                    render.sprite = sprite;
                    EditableF.ShowObjectEditor(gameObject);
                });

            }

            public override System.Object[] GetDate()
            {
                SpriteRenderer ren = gameObject.GetComponent<SpriteRenderer>();
                if (ren == null) return null;
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
