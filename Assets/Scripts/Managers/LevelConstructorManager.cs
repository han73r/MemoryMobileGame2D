using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Data;
using TMPro;
using UnityEngine.UI;
using Tools;

namespace Managers
{
    /// <summary>
    /// Create from GM command and data from LevelDataBase
    /// Static because only one level can be created at once
    /// </summary>
    internal class LevelConstructorManager : MonoBehaviour
    {
        #region thread safe Singleton
        private static readonly object lockObject = new object();
        private static LevelConstructorManager instance = null;
        private LevelConstructorManager() { }
        public static LevelConstructorManager Instance
        {
            get
            {
                lock (lockObject)
                {
                    if (instance == null)
                    {
                        instance = new LevelConstructorManager();
                    }
                    return instance;
                }

            }
        }
        #endregion

        [SerializeField] private GameObject _grid;
        [SerializeField] private GameObject _gridRowPrefab;
        [SerializeField] private GameObject _iconPrefab;
        

        [Header("Lists")]
        [SerializeField] private List<GameObject> _myTextFieldsGOList;
        [SerializeField] private List<GameObject> _myRowGOList;
        [SerializeField] private List<TextMeshProUGUI> _myTextsList;
        [SerializeField] private List<Button> _myButtonsList;

        [Header("Dynamic size of game table")]
        //[SerializeField] private int _row = 5;
        //[SerializeField] private int _coloumn = 5;
        [SerializeField] private int _matchesFound;
        [SerializeField] private TextMeshProUGUI _levelNumberText;
        [SerializeField] private TextMeshProUGUI _pointsNumberText;


        public event LevelCompletedEventHandler LevelCompleted;
        public delegate void LevelCompletedEventHandler(Level completedLevel);

        private int _maxMatches;                                    // count to end current level
        private int _levelNumber = 1;                               // start level
        private List<char> _gameList;                               // double any input values
        private bool _findingMatch;
        private Level currentLevel;       
        private int _pointsNumber = 0;                              // start points
        private GameObject _lastClickedGO;

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
        }

        internal void CreateLevel(Level level)
        {
            CreateTextFieldsFromPrefabs(level);
            // TASK // Create timer and etc
        }

        internal void DestroyLevel()
        {
            Tool.RemoveChildObjects(_grid.transform);
            _myRowGOList.Clear();
            _myTextFieldsGOList.Clear();
            _myTextsList.Clear();
            _myButtonsList.Clear();
        }

        private void CreateTextFieldsFromPrefabs(Level level)
        {
            _myTextFieldsGOList = new();
            _myTextsList = new();
            _myButtonsList = new();
            _myRowGOList = new();
            currentLevel = level;

            int rows = level.LevelNumber.Rows;
            int columns = level.LevelNumber.Columns;
            var levelDictionary = level.LevelDictionary.GetData();

            _maxMatches = rows * columns / 2;

            // TASK // Now it is a char but will be other options!
            // Create dictionary for game
            var characterPairs = CreateCharDictionary(levelDictionary,rows,columns);

            int currentIndex = 0;

            for (int i = 0; i < rows; i++)
            {
                var rowGO = Instantiate(_gridRowPrefab, _grid.transform);
                _myRowGOList.Add(rowGO);

                for (int j = 0; j < columns; j++)
                {
                    var iconGO = Instantiate(_iconPrefab, rowGO.transform);
                    _myTextFieldsGOList.Add(iconGO);
                    _myTextsList.Add(iconGO.GetComponent<TextMeshProUGUI>());

                    char currentChar = characterPairs[currentIndex];
                    currentIndex++;

                    _myTextsList[_myTextsList.Count - 1].text = currentChar.ToString();

                    // add buttons to list
                    var btn = iconGO.GetComponent<Button>();
                    btn.onClick.AddListener(delegate { IconClicked(iconGO); });
                    _myButtonsList.Add(btn);
                }
            }
        }

        // TASK // Should be <T>
        private List<char> CreateCharDictionary(string stringDictionary, int rows, int columns)
        {
            List<char> characterPairs = new List<char>();

            for (int i = 0; i < rows * columns / 2; i++)
            {
                int randomIndex = Random.Range(0, stringDictionary.Length);
                char randomChar = stringDictionary[randomIndex];
                characterPairs.Add(randomChar);
                characterPairs.Add(randomChar);
            }

            Tool.ShuffleList(characterPairs);
            return characterPairs;
        }

        #region Player Input Commands
        //TASK? // Переделать в ID? Чтобы не передавать целый GO, а одинаковым объектам одинаковый ID его быстрее проверить
        // TASK // Add points counter
        // TASK // Add UI updater
        private void IconClicked(GameObject clickedGO)
        {
            if (_lastClickedGO == null)
            {
                // 
                _lastClickedGO = clickedGO;
                clickedGO.SetActive(false);
            }
            else
            {
                // Compare
                TextMeshProUGUI selectedText = _lastClickedGO.GetComponent<TextMeshProUGUI>();
                TextMeshProUGUI clickedText = clickedGO.GetComponent<TextMeshProUGUI>();

                if (selectedText.text == clickedText.text)
                {
                    _lastClickedGO.SetActive(false);
                    clickedGO.SetActive(false);
                    _maxMatches--;
                }
                else
                {
                    _lastClickedGO.SetActive(true);
                    clickedGO.SetActive(true);
                }
                _lastClickedGO = null;
            }

            if (_maxMatches <= 0)
            {
                Debug.Log($"Level {currentLevel.LevelNumber} completed");
                HandleLevelCompletion(currentLevel);                
            }
        }

        private void HandleLevelCompletion(Level completedLevel)
        {
            LevelCompleted?.Invoke(completedLevel);
        }

        #endregion
    }
}
