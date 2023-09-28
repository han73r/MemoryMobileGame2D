using UnityEngine;
using System.Collections.Generic;
using Data;
using System.Linq;

public static class DebugTool
{
    public static void EnableLevelsWithDictionary(Dictionary<LevelThemeName, Dictionary<LevelTypeName, List<Level>>> levelsDict)
    {
        var levelsToEnable = levelsDict.Values
            .SelectMany(themeDict => themeDict.Values.SelectMany(typeDict => typeDict))
            .Where(level => level.LevelDictionary != null);

        foreach (var level in levelsToEnable)
        {
            level.DynamicData.OpenLevel();
        }
    }

    // Console is better // TASK // prepare console
}
