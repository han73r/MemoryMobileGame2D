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
    
    #region readonly
    private readonly LevelThemeName[] levelThemes;
    private readonly LevelTypeName[] levelTypes;
    #endregion

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
        OpenFirstTypeInTheme(LevelThemeName.EnglishAlphabet);

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
        Level nextLevel = GetNextLevelNumber(completedLevel);

        // TASK // Add lose condition late
        if (nextLevel == null)
        {
            DestroyLevel();
            OpenNextLevelType(completedLevel); //TASK // set next level type
            
            my_MenuManager.GoBack();
            my_MenuManager.UpdateTypeButtons(completedLevel.ThemeName);
            // go to type menu, save, etc
            // TASK // Open Next Level type
            // TASK // Return to Type menu
            // TASK // Add win/lose condition later
            // TASK // Add adv here later
        }
        else
        {
            // know next level here
            DestroyLevel();
            CreateLevel(nextLevel);
        }
    }
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

        // Если словарь levelsDict содержит текущий тип, открываем все уровни этого типа
        if (levelsDict.TryGetValue(currentLevel.ThemeName, out var themeLevels) && themeLevels.ContainsKey(nextType))
        {
            var levelsToOpen = themeLevels[nextType];
            foreach (var level in levelsToOpen)
            {
                level.OpenLevel();
            }
        }
    }

    public Level CreateLevel(int[] levelId)
    {
        return new Level(levelId);

        //if (levelId.Length != 3)
        //{
        //    throw new ArgumentException("LevelId should contain exactly 3 integers.");
        //}

        //int theme = levelId[0];
        //int type = levelId[1];
        //int number = levelId[2];

        //// TASK // CHECK // Don't see Number check.. If it will be a value bigger than in Dictionary? Check
        //if (_levelsDict.Values.Any(themeDict => themeDict.ContainsKey((LevelTypeName)type)) &&
        //    _levelsDict.TryGetValue((LevelThemeName)theme, out var themeDict) &&
        //    themeDict.ContainsKey((LevelTypeName)type))
        //{
        //    return new Level(levelId);
        //}

        //throw new ArgumentException("Invalid levelId.");
    }

    // QUESTION // Is it a good solution to use GameManager as one object to talk to others?
    public void CreateLevel(int[] levelId, 
        Dictionary<LevelThemeName, Dictionary<LevelTypeName, LevelNumber>> levelsDict)
    {
        LevelFactory.CreateLevel(levelId, levelsDict);
    }
}
