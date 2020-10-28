using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Global;
using TMPro;
using UnityEngine;

namespace Global {
    namespace Dialogue {
        public static class DialogueUtility {
            public static List<GameObject> dialoguoTargets = new List<GameObject> ();
            public static GameObject FocuseTarget => dialoguoTargets.Count > 0 ? dialoguoTargets[0] : null;

            public static void StartConversation (GameObject focuseTarget) {
                throw new NotImplementedException ();
            }
        }

        public static class TMPutility {
            public static void Print (this TMP_Text tmp, float speed, string text = null) {
                float startTime = Time.time;
                int index = 0;
                Timer.TimerControler timerControler = null;
                tmp.ForceMeshUpdate ();
                int total = tmp.textInfo.characterCount;
                if (text != null) {
                    tmp.text = text;
                }

                tmp.OnPreRenderText += SetColor;


                timerControler = Timer.Loop (speed, () => {
                    tmp.ForceMeshUpdate ();

                    index++;
                    if (index >= total) {
                        timerControler.Stop ();
                        tmp.OnPreRenderText -= SetColor;
                    }
                });




                void SetColor (TMP_TextInfo tmpt = null) {
                    float last = Time.time - startTime;
                    TMPutility.SetColor (tmp, 0, index + 1, a : 1);
                    TMPutility.SetColor (tmp, index + 1, total - index, a : 0);
                }



            }
            public static void SetColor (this TMP_Text tmp, int index, int range, float r = -1, float g = -1, float b = -1, float a = -1) {
                if (tmp.textInfo.meshInfo[0].vertexCount == 0) {
                    tmp.ForceMeshUpdate ();
                }

                TMP_CharacterInfo cha = tmp.textInfo.characterInfo[index];
                var chaList = tmp.textInfo.characterInfo.ToList ().GetRange (index, range);
                List<int> verIndexs = chaList.Select ((c) => c.vertexIndex).ToList ();

                List<Color32[]> colors = chaList.Select ((c) => tmp.textInfo.meshInfo[c.materialReferenceIndex].colors32).ToList ();

                for (int i = 0; i < colors.Count; i++) {
                    SetVertexColor (colors[i], verIndexs[i]);
                }
                tmp.UpdateVertexData (TMP_VertexDataUpdateFlags.Colors32);



                void SetVertexColor (Color32[] colorDatas, int beginIndex) {
                    for (int i = 0; i < 4; i++) {
                        if (r != -1) colorDatas[beginIndex + i].r = (byte) (r * 255);
                        if (g != -1) colorDatas[beginIndex + i].g = (byte) (g * 255);
                        if (b != -1) colorDatas[beginIndex + i].b = (byte) (b * 255);
                        if (a != -1) colorDatas[beginIndex + i].a = (byte) (a * 255);
                    }
                }
            }


        }

    }
}