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
        public int Number { get; private set; }                     // set playground size     
        public LevelThemeName ThemeName { get; private set; }
        public LevelType LevelType { get; private set; }
        public LevelNumber LevelNumber { get; private set; }        // set playground size  3rd int
        public LevelDictionary LevelDictionary { get; private set; }
        public DynamicData DynamicData { get; private set; }
        public void OpenLevel()
        {
            DynamicData.OpenLevel();
            //if (CanOpenLevel()) DynamicData.OpenLevel();
            //else Debug.LogError("Unable to open level: missing or empty LevelDictionary."); // double check?
        }
        public void CloseLevel()
        {
            DynamicData.CloseLevel();
        }
        //private bool CanOpenLevel()
        //{
        //    return LevelDictionary != null && !string.IsNullOrEmpty(LevelDictionary.GetData());
        //}
        // QUESTION // Can we Leave only one constructor 
        // Level Constructor to create level List
        public Level(int[] levelId, LevelNumber levelNumber)
        {
            if (levelId.Length != LevelIdLenth)
            {
                throw new ArgumentException($"LevelId should contain exactly {LevelIdLenth} integers.");
            }                   // Check
            foreach (int idPart in levelId)
            {
                if (idPart < MinIdValue)
                {
                    throw new ArgumentException($"Each element in LevelId should be equal or bigger than {MinIdValue}.");
                }
            }                       // Check

            LevelId = levelId;
            Theme = levelId[0];
            Type = levelId[1];
            Number = levelId[2];
            ThemeName = ((LevelThemeName)Theme);

            //if (Theme <= (Enum.GetValues(typeof(LevelThemeName)).Length))
            //{
            //    ThemeName = ((LevelThemeName)Theme);
            //}
            //// TASK // But here we can change something
            //else
            //{
            //    throw new ArgumentException("Invalid Theme value for ThemeName.");
            //}

            //if (Type <= (Enum.GetValues(typeof(LevelTypeName)).Length))
            //{
            //    TypeName = ((LevelTypeName)Type).ToString();
            //}
            //// TASK // But here we can change something
            //else
            //{
            //    throw new ArgumentException("Invalid Theme value for ThemeName.");
            //}

            LevelType = new LevelType((LevelTypeName)Type);
            LevelNumber = levelNumber;

            switch ((LevelThemeName)Theme)
            {
                case LevelThemeName.SimpleFigures:
                    break;
                case LevelThemeName.Numerals:
                    LevelDictionary = new Numerals();
                    break;
                case LevelThemeName.TagGame:
                    break;
                case LevelThemeName.Symbols:
                    break;
                case LevelThemeName.ArithmeticOperations:
                    break;
                case LevelThemeName.RomanNumerals:
                    break;
                case LevelThemeName.EnglishAlphabet:
                    LevelDictionary = new EnglishAlphabet();
                    break;
                case LevelThemeName.ChineseCharacters:
                    break;
                case LevelThemeName.WordParts:
                    break;
                case LevelThemeName.Pharse:
                    break;
                case LevelThemeName.SameMeaning:
                    break;
                case LevelThemeName.Emoji:
                    break;
                case LevelThemeName.EmojiParts:
                    break;
                case LevelThemeName.WordsAndPics:
                    break;
                case LevelThemeName.Colors:
                    break;
                case LevelThemeName.ColorShades:
                    break;
                case LevelThemeName.FastLowPolyPics:
                    break;
                case LevelThemeName.Puzzles:
                    break;
                case LevelThemeName.Pictures:
                    break;
                case LevelThemeName.BonusCraft:
                    break;
                case LevelThemeName.BonusStylus:
                    break;
                default:
                    throw new ArgumentException("Invalid Level Theme");
            }

            DynamicData = new DynamicData(levelId);
        }
        // Level Constructor 2 (for reach max level)
        public Level(int[] levelId)
        {
            LevelId = levelId;
            Theme = levelId[0];                                         // may be too much but it is a bit easy than levelid[3]
            Type = levelId[1];                                          // but you can delete it if you want
            Number = levelId[2];
            ThemeName = ((LevelThemeName)Theme);

            // TASK // What if value is bigger?
            //if (Theme <= (Enum.GetValues(typeof(LevelThemeName)).Length))
            //{
            //    ThemeName = ((LevelThemeName)Theme);
            //}
            //else
            //{
            //    throw new ArgumentException("Invalid Theme value for ThemeName.");
            //}
            //if (Type <= (Enum.GetValues(typeof(LevelTypeName)).Length))
            //{
            //    TypeName = ((LevelTypeName)Type).ToString();
            //}
            //else
            //{
            //    throw new ArgumentException("Invalid Theme value for ThemeName.");
            //}

            LevelType = new LevelType((LevelTypeName)Type);
            LevelNumber = new LevelNumber(levelId[2]);

            switch ((LevelThemeName)Theme)
            {
                case LevelThemeName.SimpleFigures:
                    break;
                case LevelThemeName.Numerals:
                    LevelDictionary = new Numerals();
                    break;
                case LevelThemeName.TagGame:
                    break;
                case LevelThemeName.Symbols:
                    break;
                case LevelThemeName.ArithmeticOperations:
                    break;
                case LevelThemeName.RomanNumerals:
                    break;
                case LevelThemeName.EnglishAlphabet:
                    LevelDictionary = new EnglishAlphabet();
                    break;
                case LevelThemeName.ChineseCharacters:
                    break;
                case LevelThemeName.WordParts:
                    break;
                case LevelThemeName.Pharse:
                    break;
                case LevelThemeName.SameMeaning:
                    break;
                case LevelThemeName.Emoji:
                    break;
                case LevelThemeName.EmojiParts:
                    break;
                case LevelThemeName.WordsAndPics:
                    break;
                case LevelThemeName.Colors:
                    break;
                case LevelThemeName.ColorShades:
                    break;
                case LevelThemeName.FastLowPolyPics:
                    break;
                case LevelThemeName.Puzzles:
                    break;
                case LevelThemeName.Pictures:
                    break;
                case LevelThemeName.BonusCraft:
                    break;
                case LevelThemeName.BonusStylus:
                    break;
                default:
                    throw new ArgumentException("Invalid Level Theme");
            }
        }
    }

    // TASK // How to avoid mistake in public LevelType GetLevelType(LevelTypeName levelType)?
    // TASK // Read about abstract and how to work with
    /// <summary>
    /// Create and set Bools via TypeName (use only enum)
    /// </summary>
    public class LevelType
    {
        public LevelTypeName Name { get; private set; }             
        public bool useTimer { get; internal set; }                 // even if bool is false timer used as hidden
        public bool useDynamicTimer { get; internal set; }          // count to zero
        public bool faceDownCards { get; internal set; }            // player start earn points here
        public bool burn { get; internal set; }                     // faceDown + burn only
        public bool opensItSelf { get; internal set; }              // faceDown + openItSelf only
        public LevelType(LevelTypeName typeName)
        {
            Name = typeName;
            switch (typeName)
            {
                case LevelTypeName.Speed:
                case LevelTypeName.MemorySpeed:
                case LevelTypeName.BurnMemory:
                case LevelTypeName.SelfOpenedMemory:
                    useTimer = true;
                    break;
                default:
                    useTimer = false;
                    break;
            }                                   // useTimer          
            switch (typeName)
            {
                case LevelTypeName.DynamicSpeed:
                case LevelTypeName.MemoryDynamicSpeed:
                case LevelTypeName.BurnMemory:
                case LevelTypeName.SelfOpenedMemory:
                    useDynamicTimer = true;
                    break;
                default:
                    useDynamicTimer = false;
                    break;
            }                                   // useDynamicTimer           
            switch (typeName)
            {
                case LevelTypeName.MemorySimple:
                case LevelTypeName.MemorySpeed:
                case LevelTypeName.MemoryDynamicSpeed:
                case LevelTypeName.BurnMemory:
                case LevelTypeName.SelfOpenedMemory:
                    faceDownCards = true;
                    break;
                default:
                    faceDownCards = false;
                    break;
            }                                   // faceDownCards
            switch (typeName)
            {
                case LevelTypeName.BurnMemory:
                case LevelTypeName.SelfOpenedMemory:
                    burn = true;
                    break;
                default:
                    burn = false;
                    break;
            }                                   // burn
            switch (typeName)
            {
                case LevelTypeName.SelfOpenedMemory:
                    opensItSelf = true;
                    break;
                default:
                    opensItSelf = false;
                    break;
            }                                   // opensItSelf
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
    public class DynamicData
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
        public bool IsOpened { get; private set; } = false;
        public int[] PlayerScore { get; internal set; }             // keeps point data about each played session
        public int BestPlayerScore { get; private set; }            // values for dynamic difficulty
        public int MiddlePlayerScore { get; private set; }
        public int GoalForPlayerScore { get; internal set; }        // set dinamic difficulty from previous values
        public float[] PlayerTime { get; private set; }             // keeps data how fast win condition for player
        public float BestPlayerTime { get; private set; }           // only for win condition, where faster is better
        public float MiddlePlayerTime { get; private set; }         // only win condition
        public float GoalForPlayerTime { get; internal set; }       // set dinamic difficulty from previous values

        public DynamicData(int[] levelId)
        {
            
            //IsOpened = (levelId[0] == 0 && levelId[1] == 0 && levelId[2] == 0);
        }

        public void OpenLevel()
        {
            IsOpened = true;
        }

        public void CloseLevel()
        {
            IsOpened = false;
        }
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

