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
/// Other Managers (only them), do not control data directly!
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


    //TASK // Set to Props with Get; private set
    // Values
    private static List<Level> _levels;
    private static Dictionary<LevelThemeName, Dictionary<LevelTypeName, List<LevelNumber>>> _levelsDict;
    private static int _maxLevel;
    private static int[] _currentLevel;

    // TASK // DDOL and Singleton
    private void Start()
    {
        LoadGameData();
        // TASK // Find better Naming solution
        CreateMenu();
        CreateNextLevel();

        int[] firstlevel = { 0, 0, 0 };
        CreateLevel(firstlevel);

        //int[] levelId = { 0,0,0};
        // TASK // Should know, what level you should create
        //LevelFactory.CreateLevel(levelId);
    }

    private void LoadGameData()
    {
        // Test Create several LevelNumbers to List()
        var levelNumbers = new List<LevelNumber>();
        for (int i = 0; i <= 10; i++)
        {
            levelNumbers.Add(new LevelNumber(i));
        }

        _levels = LevelFactory.SetUpLevels();
        _levelsDict = LevelFactory.ConvertToLevelDataDictionary(_levels);
        // PrepareLevelsList();
        // LoadPlayerData();
        // LoadOptionsData();                                       // Sound, Music and etc.
        Debug.Log("StopHere");
    }

    private void CreateMenu()
    {
        // CreateLevelSelectionUI();
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
