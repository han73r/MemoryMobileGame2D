using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Data;
using System;

namespace Tools
{
    // Somebody choose the level and set to Constructor to create it
    public static class LevelFactory
    {
        private static readonly int uiOffsetHeight = 400;          // UI height px
        private static readonly int uiOffsetWidth = 150;           // UI width px
        private static readonly int cardPxSize = 100;              // UI width px
        private static readonly int initialRowCount = 2;
        private static readonly int initialColCount = 2;

        /// <summary>
        /// Create List<Levels>
        /// </summary>
        public static List<Level> SetUpLevels()
        {
            List<Level> levels = new List<Level>();
            List<LevelNumber> levelNumbers = GenerateLevelNumberData();

            foreach (LevelThemeName theme in Enum.GetValues(typeof(LevelThemeName)))
            {
                foreach (LevelTypeName type in Enum.GetValues(typeof(LevelTypeName)))
                {
                    foreach (var levelNumber in levelNumbers)
                    {
                        int[] levelId = { (int)theme, (int)type, levelNumber.Number };
                        Level level = new Level(levelId, levelNumber);
                        levels.Add(level);
                    }
                }
            }
            return levels;
        }
        private static List<LevelNumber> GenerateLevelNumberData()
        {
            // Get max level cards per Row and Col
            int screenWidth = Screen.width - uiOffsetWidth;
            int screenHeight = Screen.height - uiOffsetHeight;
            int maxCardsPerRow = screenWidth / cardPxSize;
            int maxCardsPerColumn = screenHeight / cardPxSize;

            // 1st(0) level values
            int currentRowCount = initialRowCount;
            int currentColCount = initialColCount;
            int currentLevel = 0;

            // TASK // Change to more simplier var
            var data = new List<LevelNumber>();
            while (currentRowCount <= maxCardsPerColumn && currentColCount <= maxCardsPerRow)
            {
                data.Add(new LevelNumber(currentLevel, currentRowCount, currentColCount));
                UpdateRowCountAndColCount(ref currentRowCount, ref currentColCount,
                    maxCardsPerRow, maxCardsPerColumn);
                currentLevel++;
            }
            return data;
        }

        /// <summary>
        ///  Create dictionary to easy work with from list
        /// </summary>
        public static Dictionary<LevelThemeName, Dictionary<LevelTypeName, List<Level>>> ConverLevelListToDictionary(List<Level> levels)
        {
            var dataDictionary = new Dictionary<LevelThemeName, Dictionary<LevelTypeName, List<Level>>>();

            foreach (var level in levels)
            {
                var theme = (LevelThemeName)level.Theme;
                var type = (LevelTypeName)level.Type;

                if (!dataDictionary.ContainsKey(theme))
                {
                    dataDictionary[theme] = new Dictionary<LevelTypeName, List<Level>>();
                }

                if (!dataDictionary[theme].ContainsKey(type))
                {
                    dataDictionary[theme][type] = new List<Level>();
                }

                dataDictionary[theme][type].Add(level);
            }

            return dataDictionary;
        }

        /// <summary>
        /// Increase Row or Col throug screen size rates
        /// </summary>
        private static void UpdateRowCountAndColCount(ref int curRowCount, ref int curColCount,
    int maxCardPerRow, int maxCardPerCol)
        {
            if ((float)curRowCount / curColCount < (float)maxCardPerCol / maxCardPerRow)
            {
                curRowCount++;
            }
            else
            {
                curColCount++;
            }
        }

        public static void CreateLevel(int[] levelId, 
            Dictionary<LevelThemeName, Dictionary<LevelTypeName, LevelNumber>> levelsDict)
        {

        }

        public static void BuildLevel(Level baseLevel)
        {
            // Собирает уровень
        }
    }
}

