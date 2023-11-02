using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//using DataBase;
using Managers;
using Tools;

using Data;
using System;
using System.Linq;

/// <summary>
/// Game starts here.
/// Control Scenes
/// Other Managers (only them), do not control data directly! Only levelList and dict
/// </summary>
public /*sealed */class GameManager : MonoBehaviour
{
    #region thread safe Singleton and readonly
    private static readonly object lockObject = new object();
    private static GameManager instance = null;
    
    private GameManager() 
    {
        levelThemes = (LevelThemeName[])Enum.GetValues(typeof(LevelThemeName));
        levelTypes = (LevelTypeName[])Enum.GetValues(typeof(LevelTypeName));
    }
    public static GameManager Instance
    {
        get
        {
            lock (lockObject)
            {   
                if (instance == null)
                {
                    instance = new GameManager();
                }
                return instance;
            }

        }
    }
    #endregion

    #region readonly
    private readonly LevelThemeName[] levelThemes;
    private readonly LevelTypeName[] levelTypes;
    #endregion

    [SerializeField] private MenuManager my_MenuManager;
    [SerializeField] private LevelConstructorManager my_LevelConstructorManager;

    //TASK // Set to Props with Get; private set
    //TASK // Or Set as readonly I want to protect levels!
    // Values
    private static List<Level> _levels;                         // All levels list using Screen Resolution
    public static Dictionary<LevelThemeName, Dictionary<LevelTypeName, List<Level>>> levelsDict { get; private set; }  

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        LoadGameData();
        SubscribeOnLevelCompleted();
    }
    private void SubscribeOnLevelCompleted()
    {
        if (my_LevelConstructorManager != null)
        {
            my_LevelConstructorManager.LevelCompleted += OnLevelCompleted;
        }
        else
        {
            Debug.LogError("GameManager couldn't subscribe on levelConstructorManager");
        }
    }
    private void Start()
    {
        CreateMainMenu();
        CreateThemeMenu();

        // TASK // May be not in Start()
        //CreateTypesSubMenu();
        //CreateLevel();
        //CreateNextLevel();

        //int[] firstlevel = { 0, 0, 0 };
        //CreateLevel(firstlevel);

        //int[] levelId = { 0,0,0};
        // TASK // Should know, what level you should create
        //LevelFactory.CreateLevel(levelId);


        // TESTS
        OpenFirstTypeInTheme(LevelThemeName.SimpleFigures);
        //OpenFirstTypeInTheme(LevelThemeName.Numerals);
        //OpenFirstTypeInTheme(LevelThemeName.TagGame);
        //OpenFirstTypeInTheme(LevelThemeName.Symbols);
        //OpenFirstTypeInTheme(LevelThemeName.EnglishAlphabet);
        //OpenFirstTypeInTheme(LevelThemeName.ArithmeticOperations);       
        //OpenFirstTypeInTheme(LevelThemeName.RomanNumerals);

    }
    private void LoadGameData()
    {
        _levels = LevelFactory.SetUpLevels();
        levelsDict = LevelFactory.ConverLevelListToDictionary(_levels);
        // PrepareLevelsList();
        // LoadPlayerData();
        // LoadOptionsData();                                       // Sound, Music and etc.
        Debug.Log("StopHere");
    }

    private void CreateMainMenu()
    {
        // TASK // Add MenuManager method
    }
    private void CreateThemeMenu()
    {
        my_MenuManager.CreateThemeButtons(levelsDict);
    }

    public void CreateLevel(Level level)
    {
        my_MenuManager.StartNewLevel(true);
        my_LevelConstructorManager.CreateLevel(level);
    }
    public void DestroyLevel()
    {
        my_LevelConstructorManager.DestroyLevel();
    }

    public void OpenTheme(LevelThemeName themeName)
    {
        if (!levelsDict.TryGetValue(themeName, out var levelTypes))
        {
            Debug.LogError($"There's no such theme name '{themeName}'");
            return;
        }

        bool isOpenedLevelExists = false;
        foreach (var levelType in levelTypes.Values)
        {
            bool isOpenedLevelType = false;
            foreach (var level in levelType)
            {
                if (level.LevelDictionary != null && !string.IsNullOrEmpty(level.LevelDictionary.GetData()))
                {
                    level.OpenLevel();
                    isOpenedLevelExists = true;
                    isOpenedLevelType = true;
                }
                else
                {
                    Debug.LogError("Unable to open level: missing or empty LevelDictionary.");
                    break;
                }
            }

            if (!isOpenedLevelType)
            {
                break;
            }
        }

        if (isOpenedLevelExists)
        {
            my_MenuManager.UpdateThemeButtonsAvailability(levelsDict);
        }
        else
        {
            Debug.Log("No levels available for opening in this theme.");
        }
    }
    public void CloseTheme(LevelThemeName themeName)
    {
        if (!levelsDict.TryGetValue(themeName, out var levelTypes))
        {
            Debug.LogError($"Theme '{themeName}' not found in levels dictionary.");
            return;
        }

        foreach (var levelType in levelTypes.Values)
        {
            foreach (var level in levelType)
            {
                if (level.LevelDictionary != null && !string.IsNullOrEmpty(level.LevelDictionary.GetData()))
                {
                    level.CloseLevel();
                }
            }
        }
        my_MenuManager.UpdateThemeButtonsAvailability(levelsDict);
    }

    public void OpenFirstTypeInTheme(LevelThemeName themeName)
    {
        if (!levelsDict.TryGetValue(themeName, out var levelTypes))
        {
            Debug.LogError($"There's no such theme name '{themeName}'");
            return;
        }       

        if (levelTypes.TryGetValue(this.levelTypes[0], out var firstTypeLevels))
        {
            bool isOpenedLevelExists = false;

            foreach (var level in firstTypeLevels)
            {
                if (level.LevelDictionary != null && !string.IsNullOrEmpty(level.LevelDictionary.GetData()))
                {
                    level.OpenLevel();
                    isOpenedLevelExists = true;
                }
                else
                {
                    Debug.LogError("Unable to open level: missing or empty LevelDictionary.");
                    break;
                }
            }

            if (isOpenedLevelExists)
            {
                my_MenuManager.UpdateThemeButtonsAvailability(levelsDict);
            }
            else
            {
                Debug.Log("No levels available for opening in the first type of this theme.");
            }
        }
        else
        {
            Debug.Log($"No levels of the first type available for theme '{themeName}'.");
        }
    }

    // TASK // Realize
    private void OpenType(LevelTypeName typeName)
    {
        //TASK // Open Level Type in current theme
        //TASK // add status, to know which theme is opened
    }
    private void CloseType(LevelTypeName typeName) { }

    private void OnLevelCompleted(Level completedLevel)
    {
        // TASK // Check if lastLevelInType or in Theme
        if (IsLastLevelInType(completedLevel))
        {
            if (IsLastLevelInTheme(completedLevel))
            {
                DestroyLevel();
                OpenNextTheme(completedLevel);
                my_MenuManager.GoBack();
                my_MenuManager.GoBack();
                my_MenuManager.UpdateThemeButtonsAvailability(levelsDict);
            }
            else
            {
                DestroyLevel();
                OpenNextType(completedLevel);
                my_MenuManager.GoBack();
                my_MenuManager.UpdateTypeButtons(completedLevel.ThemeName);
            }
        }
        else
        {
            Level nextLevel = GetNextLevelNumber(completedLevel);
            DestroyLevel();
            CreateLevel(nextLevel);
        }
    }
        //Level nextLevel = GetNextLevelNumber(completedLevel);
        // TASK // Add lose condition late
        //if (nextLevel == null)
        //{
        //    //TASK // Check if it was last level in this TYPE? IF YES - go to THEME MENU
        //    if (true)
        //    {

        //    }

        //    DestroyLevel();
        //    OpenNextLevelType(completedLevel); //TASK // set next level type
            
        //    my_MenuManager.GoBack();
        //    my_MenuManager.UpdateTypeButtons(completedLevel.ThemeName);
            
        //    // go to type menu, save, etc
        //    // TASK // Add win/lose condition later
        //    // TASK // Add adv here later
        //}
        //else
        //{
        //    // know next level here
        //    DestroyLevel();
        //    CreateLevel(nextLevel);
        //}
    //}
    private bool IsLastLevelInType(Level level)                     // not best check
    {
        int currentTheme = level.LevelId[0];
        int currentType = level.LevelId[1];

        Level nextLevelInType = _levels
            .FirstOrDefault(nextlevel =>
            nextlevel.LevelId[0] == currentTheme &&
            nextlevel.LevelId[1] == currentType &&
            nextlevel.LevelId[2] == level.LevelId[2] + 1);

        return (nextLevelInType == null);
    }
    private bool IsLastLevelInTheme(Level level)                    // not best check
    {
        int currentTheme = level.LevelId[0];
        int currentLevel = level.LevelId[2];

        Level nextLevelInTheme = _levels
        .FirstOrDefault(nextlevel =>
            nextlevel.LevelId[0] == currentTheme &&
            nextlevel.LevelId[1] == level.LevelId[1] + 1 &&
            nextlevel.LevelId[2] == currentLevel);

        return (nextLevelInTheme == null);
    }
    private void OpenNextType(Level previosLevel)
    {
        LevelTypeName currentType = previosLevel.LevelType.Name;
        int currentTypeIndex = Array.IndexOf(levelTypes, currentType);

        // TASK // Add check if it last type ever
        LevelTypeName nextType = levelTypes[currentTypeIndex + 1];
        Debug.Log($"<color=green>Opening next Type: {nextType}</color>");

        if (levelsDict.TryGetValue(previosLevel.ThemeName, out var themeLevels) && themeLevels.ContainsKey(nextType))
        {
            var levelsToOpen = themeLevels[nextType];
            foreach (var level in levelsToOpen)
            {
                level.OpenLevel();
            }
        }
    }
    // IMPORTANT // IF IT IS LAST THEME IN GAME??
    private void OpenNextTheme(Level previosLevel)
    {
        LevelThemeName currentTheme = previosLevel.ThemeName;
        int currentThemeIndex = Array.IndexOf(levelThemes, previosLevel.LevelType.Name);

        // TASK // Add check if it last theme ever
        LevelThemeName nextTheme = levelThemes[currentThemeIndex + 1];
        Debug.Log($"<color=green>Opening next Theme: {nextTheme}</color>");

        //if (currentThemeIndex >= 0 && currentThemeIndex + 1 < themeNames.Length)
        //{
        //    LevelThemeName nextTheme = themeNames[currentThemeIndex + 1];

        if (levelsDict.TryGetValue(nextTheme, out var themeLevels) && themeLevels.ContainsKey(levelTypes[0]))
        {
            var levelsToOpen = themeLevels[levelTypes[0]];
            foreach (var level in levelsToOpen)
            {
                level.OpenLevel();
            }
        }      
    }


    /// <summary>
    ///  Retrun ONLY levels in choosed type
    /// </summary>
    private Level GetNextLevelNumber(Level currentLevel)
    {
        int currentTheme = currentLevel.LevelId[0];
        int currentType = currentLevel.LevelId[1];

        Level nextLevel = _levels
            .FirstOrDefault(level =>
            level.LevelId[0] == currentTheme &&
            level.LevelId[1] == currentType &&
            level.LevelId[2] == currentLevel.LevelId[2] + 1);

        return nextLevel;
    }
    private void OpenNextLevelType(Level currentLevel)
    {
        LevelTypeName currentType = currentLevel.LevelType.Name;
        int currentIndex = Array.IndexOf(levelTypes, currentType);

        // If that was last level in levelTypes
        if (currentIndex < 0 || currentIndex + 1 >= levelTypes.Length) { return; }

        LevelTypeName nextType = levelTypes[currentIndex + 1];

        // ���� ������� levelsDict �������� ������� ���, ��������� ��� ������ ����� ����
        if (levelsDict.TryGetValue(currentLevel.ThemeName, out var themeLevels) && themeLevels.ContainsKey(nextType))
        {
            var levelsToOpen = themeLevels[nextType];
            foreach (var level in levelsToOpen)
            {
                level.OpenLevel();
            }
        }
    }
    //private void CheckLastLevelInTheme(Level currentLevel)
    //{

    //}
    //public Level CreateLevel(int[] levelId)
    //{
    //    return new Level(levelId);

    //    //if (levelId.Length != 3)
    //    //{
    //    //    throw new ArgumentException("LevelId should contain exactly 3 integers.");
    //    //}

    //    //int theme = levelId[0];
    //    //int type = levelId[1];
    //    //int number = levelId[2];

    //    //// TASK // CHECK // Don't see Number check.. If it will be a value bigger than in Dictionary? Check
    //    //if (_levelsDict.Values.Any(themeDict => themeDict.ContainsKey((LevelTypeName)type)) &&
    //    //    _levelsDict.TryGetValue((LevelThemeName)theme, out var themeDict) &&
    //    //    themeDict.ContainsKey((LevelTypeName)type))
    //    //{
    //    //    return new Level(levelId);
    //    //}

    //    //throw new ArgumentException("Invalid levelId.");
    //}

    // QUESTION // Is it a good solution to use GameManager as one object to talk to others?
    //public void CreateLevel(int[] levelId, 
    //    Dictionary<LevelThemeName, Dictionary<LevelTypeName, LevelNumber>> levelsDict)
    //{
    //    LevelFactory.CreateLevel(levelId, levelsDict);
    //}
}
