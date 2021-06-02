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
        public class SpriteTexture : EditDate
        {
            private SpriteRenderer Render => gameObject.GetComponent<SpriteRenderer>();

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


                    object[] vs = GetDate();
                    Texture colorTex = vs[0] as Texture;
                    Texture normalTex = vs[1] as Texture;
                    PrefabI.UI_EditorItem_TitleTexture.CreateInstance((obj) =>
                    {
                        CompItemTexture com = obj.GetComponent<CompItemTexture>();
                        com.titleText = UiConfig.paramNames[0];
                        com.SetTexture(colorTex);
                        com.AddOnClickAction(ClickAction_ShowAllTexturesAndSelect);
                        re.Add(obj);
                    });


                    PrefabI.UI_EditorItem_TitleTexture.CreateInstance((obj) =>
                    {
                        CompItemTexture com = obj.GetComponent<CompItemTexture>();
                        com.titleText = UiConfig.paramNames[1];
                        if (normalTex != null) { com.SetTexture(normalTex); }
                        com.AddOnClickAction(ClickSetNormalAction);
                        re.Add(obj);
                    });


                    return re.ToArray();
                }
            }



            private void ClickAction_ShowAllTexturesAndSelect()
            {

                Func.Editor_TextureSelector.ShowFolderTexture((path) =>
                {
                    Texture2D colorTex = FileF.LoadTexture(path);
                    object[] dates = new[] { colorTex, null };
                    ApplayDate(dates);
                    EditableF.ShowObjectEditor(gameObject);
                });

            }

            private void ClickSetNormalAction()
            {
                Func.Editor_TextureSelector.ShowFolderTexture((path) =>
                 {
                     Texture2D normal = FileF.LoadTexture(path);

                     object[] dates = new[] { null, normal };
                     ApplayDate(dates);
                     EditableF.ShowObjectEditor(gameObject);
                 });
            }




            public override Type[] DateTypes => new[] { typeof(Texture2D), typeof(Texture2D) };
            public override string[] StringDates
            {
                get
                {
                    object[] vs = GetDate();

                    string texPath = FileBundle.TextureLoader.GetPath(vs[0] as Texture2D);
                    string texPathLo = FileF.GetLocalPathFromDataFolder(texPath);
                    string norPath = FileBundle.TextureLoader.GetPath(vs[1] as Texture2D);
                    string norPathLo = FileF.GetLocalPathFromDataFolder(norPath);

                    string[] re = new[] { texPathLo, norPathLo };
                    return re;
                }
            }
            public override object[] StringDateParse(string[] stringDates)
            {
                object[] re = new object[2];
                string texPathLo = stringDates[0];
                string norPathLo = stringDates[1];
                re[0] = FileF.LoadTextureFromDateFolder(texPathLo);
                re[1] = FileF.LoadTextureFromDateFolder(norPathLo);
                return re;
            }



            public override System.Object[] GetDate()
            {
                SpriteRenderer ren = gameObject.GetComponent<SpriteRenderer>();
                if (ren == null) return null;
                Texture2D texture = ren.sprite.texture;
                Texture nor = ren.sharedMaterial.GetTexture("_NormalMap");

                System.Object[] re = new System.Object[] { texture, nor as Texture2D };
                return re;
            }

            public override void ApplayDate(System.Object[] dates)
            {
                Texture2D tex = dates[0] as Texture2D;
                if (tex != null)
                {
                    Render.sprite = tex.ToSprite(); ;
                }

                Texture2D nor = dates[1] as Texture2D;
                if (nor != null)
                {
                    Render.sharedMaterial.SetTexture("_NormalMap", nor); ;
                }

            }


        }
    }
}
