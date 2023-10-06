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

        private int _maxMatches;
        private int _levelNumber = 1;                                // start level
        private List<char> _gameList;                                // double any input values
        private bool _findingMatch;
        private GameObject _lastGoClicked;
        private int _pointsNumber = 0;                               // start points

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

            int rows = level.LevelNumber.Rows;
            int columns = level.LevelNumber.Columns;
            var levelDictionary = level.LevelDictionary.GetData();

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
                }
            }
        }

        //// add buttons to list
        //var btn = iconGO.GetComponent<Button>();
        ////btn.onClick.AddListener(delegate { IconClicked(iconGO); });
        //_myButtonsList.Add(btn);

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
        // TASK? // Переделать в ID? Чтобы не передавать целый GO, а одинаковым объектам одинаковый ID его быстрее проверить
        //private static void IconClicked(GameObject iconGO)
        //{

        //    if (!_findingMatch)
        //    {
        //        iconGO.SetActive(false);
        //        _lastGoClicked = iconGO;

        //        _lastClickedText = iconGO.GetComponent<TextMeshProUGUI>().text;
        //        _findingMatch = true;

        //    }
        //    else if (iconGO.GetComponent<TextMeshProUGUI>().text == _lastGoClicked.GetComponent<TextMeshProUGUI>().text)
        //    {
        //        iconGO.SetActive(false);
        //        _findingMatch = false;
        //        _matchesFound++;
        //        _pointsNumber++;                // add points for right desicion
        //        CheckIfLeveIsCompleted();
        //        UpdateUI();
        //    }
        //    else
        //    {
        //        _lastGoClicked.SetActive(true);
        //        _findingMatch = false;
        //        _pointsNumber -= 3;             // remove points for wrong desicion
        //        UpdateUI();
        //    }
        //}
        #endregion
    }

}
