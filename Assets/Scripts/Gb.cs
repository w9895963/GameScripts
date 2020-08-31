using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Gb : MonoBehaviour {
    public M_PlayerManager mainCharactor;
    public static M_PlayerManager MainCharactor;

    public Camera helperCamera;
    public static Camera HelperCamera;

    public Text screenLog;
    public static Text ScreenLog;

    public M_BackPack backpack;
    public static M_BackPack Backpack;


    public Image canvasBackGround;
    public static Image CanvasBackGround;
    public Image canvasTopLayer;
    public static Image CanvasTopLayer;



    public GameObject backpackButton;
    public static GameObject BackpackButton;

    public static C_StateMachine<PlayMode> PlayModeManager = new C_StateMachine<PlayMode> ();



    private void Awake () {
        ApplyData ();
    }

    private void Start () {
        PlayModeManager.SetState (PlayMode.noramal);
        PlayModeManager.InvokeEnterEvent (PlayMode.noramal);
    }

    private void Reset () {
        mainCharactor = GetComponent<M_PlayerManager> ();
        backpack = GetComponent<M_BackPack> ();
    }


    //*Private
    private void ApplyData () {
        HelperCamera = helperCamera ? helperCamera : HelperCamera;
        MainCharactor = mainCharactor ? mainCharactor : MainCharactor;
        ScreenLog = screenLog ? screenLog : ScreenLog;
        Backpack = backpack ? backpack : Backpack;
        CanvasBackGround = canvasBackGround ? canvasBackGround : CanvasBackGround;
        CanvasTopLayer = canvasTopLayer ? canvasTopLayer : CanvasTopLayer;
        BackpackButton = backpackButton ? backpackButton : BackpackButton;
    }

    public enum PlayMode {
        noramal
    }

}