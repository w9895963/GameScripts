using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectEx
{
    private GameObject gameobject;
    public static GameObjectEx Get(GameObject gameobject)
    {
        GameObjectEx obj = new GameObjectEx();
        obj.gameobject = gameobject;
        return obj;
    }
}
