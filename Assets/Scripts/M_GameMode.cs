using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class M_GameMode : MonoBehaviour {

    private GamePlayMode current = GamePlayMode.zero;
    [SerializeField] private GamePlayMode gameModeChanged = GamePlayMode.zero;
    public UnityEvent<GamePlayMode> onGameModeExit = new UnityEvent<GamePlayMode> ();
    public UnityEvent<GamePlayMode> onGameModeEnter = new UnityEvent<GamePlayMode> ();
    public Dictionary<string, GamePlayMode> gameModeDict = new Dictionary<string, GamePlayMode> ();


    private void Awake () { }



    private void Start () {
        MakeDictionary ();
    }




    //*Private
    private void MakeDictionary () {
        foreach (GamePlayMode gm in Enum.GetValues (typeof (GamePlayMode))) {
            if (!gameModeDict.ContainsValue (gm)) {
                gameModeDict.Add (gm.ToString (), gm);
            }
        }
    }




    //*Public
    public GamePlayMode mode { get => current; }
    public void ChangeGameMode (GamePlayMode mode) {
        if (current != mode) {
            onGameModeExit.Invoke (current);
            current = mode;
            onGameModeEnter.Invoke (current);
        }
    }
    public void ChangeGameMode (string gameMode) {
        ChangeGameMode (gameModeDict[gameMode]);
    }



#if UNITY_EDITOR
    private void OnValidate () {
        UnityEditor.EditorApplication.delayCall += () => {
        ChangeGameMode (gameModeChanged);
        };
    }

#endif


}


public enum GamePlayMode {
    zero,
    normal,
    placeItem

}