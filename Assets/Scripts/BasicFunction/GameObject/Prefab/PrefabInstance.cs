using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using PrefabBundle;
using PrefabBundle.Compt;
using UnityEngine;

namespace PrefabBundle
{

    namespace Compt
    {
        public class PrefabInstance : MonoBehaviour
        {
            public string folderPath = "Prefab";
            public string fileName;
            public string filePath;




            private void Reset()
            {
                GeneratePath();
            }
            [ContextMenu("GeneratePath")]
            public void GeneratePath()
            {
                fileName = name;
                filePath = $"{folderPath}/{fileName}";
            }
        }
    }








}










