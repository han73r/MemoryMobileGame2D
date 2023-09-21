using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Data
{
    /// <summary>
    /// Keep static and dynamic info about levels
    /// </summary>
    public class LevelsData
    {
        // TASK // CREATE FROM CSV? or JSON file?
        public class Level
        {
            public int Theme { get; private set; }         //QUESTION // What database use?
            public string ThemeName { get; private set; }
            public int Type { get; private set; }           // Type Enum Condition
            public string TypeName { get; private set; }
            public LevelType LevelType { get; private set; }
            public int Number { get; private set; }         // how many cards or field size (rows and coloumns)
            
            //public FieldSize FieldSize { get; set; }
            [SerializeField]
            public DynamicLevelStats dynamicLevelStats { get; private set; }
            public Level(int theme, int type, int number)
            {
                Theme = theme;
                Type = type;
                Number = number;
            }

            // TASK // NO int check! // Start from Zero level!
            public void AddLevelName(Level level)
            {
                ThemeName = ((LevelThemeNames)level.Theme).ToString();
            }

            public void AddLevelTypeName(Level level)
            {
                TypeName = ((LevelTypeName)level.Type).ToString();
            }

            public void AddLevelTypeData(Level level, LevelType LevelType)
            {
                level.LevelType = LevelType;
            }
        }


    }

    public class DynamicLevelStats
    {

    }
}


