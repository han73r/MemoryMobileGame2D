using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Data;
using System;

public class ThemeManager
{
    private static ThemeManager instance;
    private Dictionary<LevelThemeName, Func<LevelDictionary>> dictionaryMappings;

    public ThemeManager()
    {
        dictionaryMappings = new Dictionary<LevelThemeName, Func<LevelDictionary>>()
        {
            { LevelThemeName.SimpleFigures, () => new SimpleFigures() },
            { LevelThemeName.Numerals, () => new Numerals() },
            { LevelThemeName.TagGame, () => new TagGame() },
            { LevelThemeName.Symbols, () => new Symbols() },
            { LevelThemeName.ArithmeticOperations, () => new ArithmeticOperations() },
            { LevelThemeName.RomanNumerals, () => new RomanNumerals() },
            { LevelThemeName.EnglishAlphabet, () => new EnglishAlphabet() }
            //TASK // add other dictionaries
        };
    }
    public static ThemeManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new ThemeManager();
            }
            return instance;
        }
    }

    public LevelDictionary GetDictionaryForTheme(LevelThemeName theme)
    {
        if (dictionaryMappings.TryGetValue(theme, out var dictionaryCreator))
        {
            return dictionaryCreator();
        }
        return null;
    }
}
