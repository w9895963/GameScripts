using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFunc : MonoBehaviour
{

}

namespace EditableBundle
{
    public static class UIFunc
    {
        public static void EnableEditor()
        {
            EditableBundle.ShareField.Editor.SetActive(true);
        }
        public static void DisableEditor()
        {
            EditableBundle.ShareField.Editor.SetActive(false);
        }
    }
}
