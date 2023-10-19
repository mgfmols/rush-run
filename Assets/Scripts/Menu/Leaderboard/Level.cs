using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Level
{
    Farm
}

public abstract class LevelDict
{
    public static Dictionary<Level, string> SaveDataDictionary = new Dictionary<Level, string>()
    {
        {
            Level.Farm, "farmLeaderboard"
        }
    };
}
