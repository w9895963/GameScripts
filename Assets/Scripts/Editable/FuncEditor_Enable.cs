using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace EditableBundle
{
    namespace Func
    {
        public static class Editor_Enable
        {
            public static void EnableEditor()
            {
                EditableBundle.ShareDate.Editor.SetActive(true);
            }
            public static void DisableEditor()
            {
                EditableBundle.ShareDate.Editor.SetActive(false);
            }
        }
    }

}
