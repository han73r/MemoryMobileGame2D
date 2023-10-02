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
    #region Singleton                                                   // thread safe Singleton
    private static readonly object lockObject = new object();
    private static GameManager instance = null;
    private GameManager() { }
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

    [SerializeField] private MenuManager my_menuManager;

    //TASK // Set to Props with Get; private set
    //TASK // Or Set as readonly I want to protect levels!
    // Values
    private static List<Level> _levels;                         // All levels list using Screen Resolution
    public static Dictionary<LevelThemeName, Dictionary<LevelTypeName, List<Level>>> levelsDict { get; private set; }
    //private static int _maxLevel;
    //private static int[] _currentLevel;

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
    }


    private void Start()
    {
        CreateMainMenu();
        CreateThemeMenu();

        // TASK // May be not in Start()
        //CreateTypesSubMenu();
        //CreateLevel();
        //CreateNextLevel();

        int[] firstlevel = { 0, 0, 0 };
        CreateLevel(firstlevel);

        //int[] levelId = { 0,0,0};
        // TASK // Should know, what level you should create
        //LevelFactory.CreateLevel(levelId);
    }

    // Open.. 
    public void OpenTheme(LevelThemeName themeName)
    {
        //TASK // Open Theme and first Type that have dictionary!
        // if cant - show message about it
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
                    level.OpenLevel();
                }
            }
        }
        my_menuManager.UpdateThemeButtonsAvailability(levelsDict);
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
        my_menuManager.UpdateThemeButtonsAvailability(levelsDict);
    }


    private void OpenTheme(LevelTypeName typeName)
    {
        //TASK // Open Level Type in current theme
        //TASK // add status, to know which theme is opened
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
        my_menuManager.CreateThemeButtons(levelsDict);
    }

    public void CreateNextLevel()
    {
        // if exists!
        // adding level to currentLevel or some other system
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
    public void Test()
    {

    }

}
