using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Data
{
    // RULE // All data about current level should be here
    // QUESTION // Maybe they shouldnt be abstract as we dont use modification of this classes
    public class Level
    {
        private const int LevelIdLenth = 3;
        private const int MinIdValue = 0;
        public int[] LevelId { get; private set; }                  // Theme+Type+LevelNumber
        public int Theme { get; private set; }                      // choosed dictionary   1st int
        public int Type { get; private set; }                       // set LevelType        2nd int
        public int Number { get; private set; }                   // set playground size     
        public string ThemeName { get; private set; }
        public string LevelTypeName { get; private set; }           // take from enum
        public LevelType LevelType { get; private set; }
        public LevelNumber LevelNumber { get; private set; }        // set playground size  3rd int
        public ILevelDictionary Dictionary { get; private set; }
        public DynamicData DynamicData { get; private set; }

        // Level Constructor
        public Level(int[] levelId, LevelNumber levelNumber)
        {
            if (levelId.Length != LevelIdLenth)
            {
                throw new ArgumentException($"LevelId should contain exactly {LevelIdLenth} integers.");
            }

            foreach (int idPart in levelId)
            {
                if (idPart < MinIdValue)
                {
                    throw new ArgumentException($"Each element in LevelId should be equal or bigger than {MinIdValue}.");
                }
            }
            LevelId = levelId;
            Theme = levelId[0];
            Type = levelId[1];
            LevelNumber = levelNumber;

            if (Theme <= (Enum.GetValues(typeof(LevelThemeName)).Length))
            {
                ThemeName = ((LevelThemeName)Theme).ToString();
            }
            // TASK // But here we can change something
            else
            {
                throw new ArgumentException("Invalid Theme value for ThemeName.");
            }

            if (Type <= (Enum.GetValues(typeof(LevelTypeName)).Length))
            {
                LevelTypeName = ((LevelTypeName)Type).ToString();
            }
            // TASK // But here we can change something
            else
            {
                throw new ArgumentException("Invalid Theme value for ThemeName.");
            }

            // TASK // add LevelType, ILevelDictionary, DynamicData
        }
        // Level Constructor 2 - current
        public Level(int[] levelId)
        {
            LevelId = levelId;
            Theme = levelId[0];                                         // may be too much but it is a bit easy than levelid[3]
            Type = levelId[1];                                          // but you can delete it if you want
            Number = levelId[2];

            // TASK // What if value is bigger?
            if (Theme <= (Enum.GetValues(typeof(LevelThemeName)).Length))
            {
                ThemeName = ((LevelThemeName)Theme).ToString();
            }
            else
            {
                throw new ArgumentException("Invalid Theme value for ThemeName.");
            }
            if (Type <= (Enum.GetValues(typeof(LevelTypeName)).Length))
            {
                LevelTypeName = ((LevelTypeName)Type).ToString();
            }
            else
            {
                throw new ArgumentException("Invalid Theme value for ThemeName.");
            }

            LevelType = new LevelType((LevelTypeName)Type);
            LevelNumber = new LevelNumber(levelId[2]);
            // TASK // Create Dictionary abstract class

        }
    }

    // TASK // How to avoid mistake in public LevelType GetLevelType(LevelTypeName levelType)?
    // TASK // Read about abstract and how to work with
    /// <summary>
    /// Create and set Bools via TypeName (use only enum)
    /// </summary>
    public class LevelType
    {
        public bool useTimer { get; internal set; }                 // even if bool is false timer used as hidden
        public bool useDynamicTimer { get; internal set; }          // count to zero
        public bool faceDownCards { get; internal set; }            // player start earn points here
        public bool burn { get; internal set; }                     // faceDown + burn only
        public bool opensItSelf { get; internal set; }              // faceDown + openItSelf only
        public LevelType(LevelTypeName typeName)
        {
            switch (typeName)
            {
                case LevelTypeName.Simple:
                    useTimer = false;
                    useDynamicTimer = false;
                    faceDownCards = false;
                    burn = false;
                    opensItSelf = false;
                    break;
                case LevelTypeName.Speed:
                    useTimer = true;                                // here
                    useDynamicTimer = false;
                    faceDownCards = false;
                    burn = false;
                    opensItSelf = false;
                    break;
                case LevelTypeName.DynamicSpeed:
                    useTimer = false;
                    useDynamicTimer = true;                         // here
                    faceDownCards = false;
                    burn = false;
                    opensItSelf = false;
                    break;
                case LevelTypeName.MemorySimple:
                    useTimer = false;
                    useDynamicTimer = false;
                    faceDownCards = true;                           // here
                    burn = false;
                    opensItSelf = false;
                    break;
                case LevelTypeName.MemorySpeed:
                    useTimer = true;                                // here
                    useDynamicTimer = false;
                    faceDownCards = true;                           // here
                    burn = false;
                    opensItSelf = false;
                    break;
                case LevelTypeName.MemoryDynamicSpeed:
                    useTimer = false;
                    useDynamicTimer = true;                         // here
                    faceDownCards = true;                           // here
                    burn = false;
                    opensItSelf = false;
                    break;
                case LevelTypeName.BurnMemory:
                    useTimer = false;
                    useDynamicTimer = true;                         // here
                    faceDownCards = true;                           // here
                    burn = true;                                    // here
                    opensItSelf = false;
                    break;
                case LevelTypeName.SelfOpenedMemory:
                    useTimer = false;
                    useDynamicTimer = true;                         // here
                    faceDownCards = true;                           // here
                    burn = true;                                    // here
                    opensItSelf = true;                             // here
                    break;
                default:
                    throw new ArgumentException("Unknown level type", nameof(typeName));
            }

        }
    }

    /// <summary>
    /// Has 16:9 screen rate, Constructor is ready
    /// </summary>
    public class LevelNumber
    {
        // 16:9 rate
        public int Number { get; private set; }
        public int Rows { get; private set; }
        public int Columns { get; private set; }
        public LevelNumber(int levelNumber)
        {
            // base values
            int baseRows = 2;
            int baseColumns = 2;

            //max values
            int maxRows = 16;
            int maxColumns = 9;

            float aimRatio = (float)maxRows / maxColumns;

            Number = levelNumber;
            Rows = baseRows;
            Columns = baseColumns;

            for (int i = 0; i < Number; i++)
            {
                float currentRation = (float)Rows / Columns;

                if (currentRation < aimRatio)
                {
                    Rows++;
                }
                else
                {
                    Columns++;
                }
            }
        }
        public LevelNumber(int number, int rows, int cols)
        {
            Number = number;
            Rows = rows;
            Columns = cols;
        }
    }
    // TASK // Save this data
    [SerializeField]
    public abstract class DynamicData
    {
        private int minStars = 0;
        private int maxStars = 3;
        private int _starsEarned;
        public int StarsEarned
        {
            get => _starsEarned;
            private set
            {
                if (value > maxStars)
                {
                    _starsEarned = maxStars;
                    return;
                }
                else if (value < minStars)
                {

                    _starsEarned = minStars;
                    return;
                }
                else
                {
                    _starsEarned = value;
                }
            }
        }
        public bool IsOpened { get; private set; }

        public int[] PlayerScore { get; internal set; }             // keeps point data about each played session
        public int BestPlayerScore { get; private set; }            // values for dynamic difficulty
        public int MiddlePlayerScore { get; private set; }
        public int GoalForPlayerScore { get; internal set; }        // set dinamic difficulty from previous values

        public float[] PlayerTime { get; private set; }             // keeps data how fast win condition for player
        public float BestPlayerTime { get; private set; }           // only for win condition, where faster is better
        public float MiddlePlayerTime { get; private set; }         // only win condition
        public float GoalForPlayerTime { get; internal set; }       // set dinamic difficulty from previous values
    }

    public static class MiddleResultCounter
    {
        public static int SetMiddlePlayerScore(Level level)
        {
            return default;
        }

    }

    // TASK // Avoid default becase it is a mistake // Take data from same Simple level
    // TASK // Dynamic Data Updater will use that methodsl
    /// <summary>
    /// IMPORTANT: Should set a Goals from "Simple" level from LevelTypeNames to others!
    /// UPDATE: IF current dta is zero take data from same Simple level Number!
    /// </summary>
    public static class ReturnPlayerGoals   // Вроде как интовое выражение должен возвращать а ты ему жоско задаешь что на 
    {
        public static int ReturnGoalForPlayerScore(List<Level> levelList, Level level)
        {
            if (level.DynamicData.BestPlayerScore != default)       // try to take data from Current level
            {
                return level.DynamicData.BestPlayerScore;
            }
            else
            {
                var simpleLevel = levelList
                    .Find(l => l.Theme == level.Theme
                    && l.LevelNumber.Number == level.LevelNumber.Number
                    && l.Type == default);
                return simpleLevel.DynamicData.BestPlayerScore;     // will be default or value anyway
            }
        }

        public static float ReturnGoalForPlayerTime(List<Level> levelList, Level level)
        {
            if (level.DynamicData.BestPlayerTime != default)        // try to take data from Current level
            {
                return level.DynamicData.BestPlayerTime;
            }
            else
            {
                var simpleLevel = levelList
                    .Find(l => l.Theme == level.Theme
                    && l.LevelNumber.Number == level.LevelNumber.Number
                    && l.Type == default);
                return simpleLevel.DynamicData.BestPlayerTime;      // will be default or value anyway
            }
        }
    }

    // TASK // Shoud repeat work with abstract classes

    // public class LevelTypeProvider : LevelType


}

