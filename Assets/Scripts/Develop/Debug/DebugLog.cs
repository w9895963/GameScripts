using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DebugF
{
    private static string logLine = "";
    private static int index = 0;
    private static float? currTime;

    public static void LogLine(this System.Object logObj, string name = null, bool lineBreak = false)
    {
        if (currTime == null)
        {
            currTime = Time.time;
        }
        else
        {
            if (currTime != Time.time)
            {
                LogOut();
            }
        }
        logLine += $"{"{"} {index} {name} < {logObj.ToString()} > {"}"}  ";
        index++;

        if (lineBreak)
        {
            LogOut();
        }

        static void LogOut()
        {
            Debug.Log(logLine);
            index = 0;
            logLine = "";
            currTime = null;
        }

    }
    public static void LogLine(this System.Object logObj, bool lineBreak = false)
    {
        LogLine(logObj, null, lineBreak);
    }
    public static void LogLine(this System.Object logObj)
    {
        LogLine(logObj, null, false);
    }


    public static void Log(this System.Object logObj, string name = null, Object context = null)
    {
        Debug.Log(name + " : " + logObj, context);
    }
    public static void Log(this System.Object logObj, Object context = null)
    {
        Debug.Log(logObj, context);
    }
    public static void Log(this System.Object logObj)
    {
        Debug.Log(logObj);
    }


}
