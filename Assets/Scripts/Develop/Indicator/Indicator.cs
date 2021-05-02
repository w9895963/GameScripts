using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class Indicator : MonoBehaviour
{

    private void Awake()
    {
        AllIndicator.Add(this);
    }
    private void OnDestroy()
    {
        AllIndicator.Remove(this);
    }

    private void OnEnable()
    {
        SetRenderEnabled(this);
    }




    #region Global
    public static bool Show = false;
    public static List<Indicator> AllIndicator = new List<Indicator>();
    private static void SetRenderEnabled(Indicator x)
    {
        Renderer render = x.GetComponent<Renderer>();
        if (render != null)
        {
            render.enabled = Show;
        }
        SpriteShapeController spriteShapeController = x.GetComponent<SpriteShapeController>();
        if (spriteShapeController != null)
        {
            spriteShapeController.enabled = Show;
        }
    }
    public static void ToggleIndicator(bool? enabled = null)
    {
        Show = enabled == null ? !Show : (bool)enabled;

        AllIndicator.ForEach((x) =>
        {
            SetRenderEnabled(x);

        });

    }





    #endregion
    // * Region Global End---------------------------------- 
}
