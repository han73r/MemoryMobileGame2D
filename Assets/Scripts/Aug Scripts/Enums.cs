using System;
using System.Linq;

namespace Data
{
    public static class EnumHelper
    {
        public static T[] GetValues<T>() where T : Enum
        {
            return Enum.GetValues(typeof(T)).Cast<T>().ToArray();
        }
    }
    public enum LevelThemeName
    {
        SimpleFigures,
        Numerals,
        TagGame,
        Symbols,
        ArithmeticOperations,
        RomanNumerals,
        EnglishAlphabet,
        ChineseCharacters,
        WordParts,
        Pharse,
        SameMeaning,
        Emoji,
        EmojiParts,
        WordsAndPics,
        Colors,
        ColorShades,
        FastLowPolyPics,
        Puzzles,
        Pictures,
        BonusCraft,
        BonusStylus
    }
    public enum LevelTypeName
    {
        Simple,                     // withoout points
        Speed,                      // add points at the end of the game
        DynamicSpeed,               // add points at the end of the game

        MemorySimple,               // withoout points
        MemorySpeed,                // add points at the end of the game
        MemoryDynamicSpeed,         // add points at the end of the game
        //MemoryPoints,
        //MemorySpeedAndPoitns,

        BurnMemory,                 // add points at the end of the game
        SelfOpenedMemory,           // add points at the end of the game
    }
    public enum BonusLevelTypeData
    {
        Rotated,
        Color,
        Reload
    }
}
