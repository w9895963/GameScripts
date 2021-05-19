using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LayerF
{
    public static int AddMask(int currMask, Layer layer)
    {
        int v = 1 << (int)layer;
        return currMask | v;
    }
    public static int RemoveMask(int currMask, Layer layer)
    {
        int v = 1 << (int)layer;
        return currMask ^ v;
    }
}

public enum Layer
{
    StaticSolid = 8,
    Player = 9,
    Indicator = 30,
    TempLayer = 31
}
