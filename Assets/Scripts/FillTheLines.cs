using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using System;
using System.Linq;
using UnityEngine.UI;

// Emoji pack below, try:
// https://github.com/iBicha/EmojiTexture
public class FillTheLines : MonoBehaviour
{
    #region Variables
    #region SerializeField
    [Header("GO and prefabs")]
    [SerializeField]
    private GameObject _grid;

    [SerializeField]
    private GameObject _gridRowPrefab;

    [SerializeField]
    private GameObject _iconPrefab;

    [Header("Lists")]
    [SerializeField]
    private List<GameObject> _myTextFieldsGOList;

    [SerializeField]
    private List<GameObject> _myRowGOList;

    [SerializeField]
    private List<TextMeshProUGUI> _myTextsList;

    [SerializeField]
    private List<Button> _myButtonsList;

    [Header("Dynamic size of game table")]
    [SerializeField]
    private int _row = 5;

    [SerializeField]
    private int _coloumn = 5;

    [SerializeField]
    private int _matchesFound;

    [SerializeField]
    private TextMeshProUGUI _levelNumberText;

    [SerializeField]
    private TextMeshProUGUI _pointsNumberText;

    #endregion

    #region Readonly
    private readonly string _alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private readonly string _numbers = "0987654321";
    #endregion

    #region Private
    private int _maxMatches;
    private int _levelNumber = 1;       // start level
    private List<char> _gameList;       // double any input values
    private bool _findingMatch;
    private GameObject _lastGoClicked;
    private String _lastClickedText;
    private int _pointsNumber = 0;      // start points
    #endregion
    #endregion

    #region Methods

    #region Start And Restart Game
    private void Start()
    {
        StartGame();
    }

    public void StartGame()
    {
        _matchesFound = 0;
        //_levelNumber = 1;
        //_pointsNumber = 0;
        _maxMatches = _row * _coloumn / 2;

        CheckForRestart(); // TASK // lose (start from #level)/win (somethinf) / next level / cases
        CreateTextFieldsFromPrefabs(_row, _coloumn);
        _gameList = LoadGameList(_alphabet);
        SetValuesToFields(_gameList);
        UpdateUI();
    }

    private void CreateTextFieldsFromPrefabs(int row, int coloumn)
    {
        _myTextFieldsGOList = new();
        _myTextsList = new();
        _myButtonsList = new();
        _myRowGOList = new();

        for (int i = 0; i < row; i++)
        {
            var rowGO = Instantiate(_gridRowPrefab, _grid.transform);
            _myRowGOList.Add(rowGO);

            for (int j = 0; j < coloumn; j++)
            {
                var iconGO = Instantiate(_iconPrefab, rowGO.transform);
                _myTextFieldsGOList.Add(iconGO);
                _myTextsList.Add(iconGO.GetComponent<TextMeshProUGUI>());

                // add buttons to list
                var btn = iconGO.GetComponent<Button>();
                btn.onClick.AddListener(delegate { IconClicked(iconGO); });
                _myButtonsList.Add(btn);
            }
        }
    }

    public List<char> LoadGameList(string inputValue)
    {
        _gameList = new List<char>();

        int x = _row * _coloumn;

        for (int i = 0; i < x;)
        {
            // TASK // ОБЯЗАТЕЛЬНО НУЖНА ПРОВЕРКА НА РАЗМЕР ВХОДЯЩЕГО ЗНАЧЕНИЯ ИНАЧЕ МОЖНО ПОКРУГУ ТУТ БЕГАТЬ)
            char ch = GerRandomChar(inputValue);

            if (!_gameList.Contains(ch))
            {
                _gameList.Add(ch);
                _gameList.Add(ch);
                i += 2;
            }
        }
        return _gameList;
    }

    private static char GerRandomChar(string inputValue)
    {
        var random = new System.Random();
        int index = random.Next(inputValue.Length);
        return inputValue[index];
    }

    private void SetValuesToFields(List<char> charsList)
    {
        // TASK // check that list count and textIconList have same numbers
        var random = new System.Random();

        foreach (var icon in _myTextsList)
        {
            int index = random.Next(charsList.Count);
            string next = charsList[index].ToString();
            icon.text = next;
            charsList.RemoveAt(index);
        }
    }

    private void CheckForRestart()
    {
        // why not in delete rows? for new Restart Methods
        if (_myTextsList.Count > 0)
        {
            DeleteRows();
        }
    }

    private void DeleteRows()
    {
        // Destroy previos ones if it's a restart
        foreach (var rowGO in _myRowGOList)
        {
            Destroy(rowGO);
        }
    }
    #endregion

    #region Unused Methods Delete 
    private List<string> GetRandomValuesFromThatList(List<string> sourceList, int count)
    {
        count /= 2;                                                 // how many pairs do you need, half of total count
        return new List<string>();
    }

    private List<GameObject> GetAllChilds(GameObject go)
    {
        List<GameObject> list = new();
        for (int i = 0; i < go.transform.childCount; i++)
        {
            list.Add(go.transform.GetChild(i).gameObject);
        }
        return list;
    }

    private List<TextMeshProUGUI> GetAllTextMeshProUGUI(List<GameObject> goList)
    {
        List<TextMeshProUGUI> txtsList = new();

        for (int i = 0; i < goList.Count; i++)
        {
            for (int j = 0; i < goList[i].transform.childCount; i++)
            {
                txtsList.Add(goList[j].transform.GetComponentInChildren<TextMeshProUGUI>());
                txtsList[j].gameObject.SetActive(false);
            }
        }
        foreach (var txt in txtsList)
        {
            txt.gameObject.SetActive(true);
        }
        return txtsList;

    }
    #endregion

    #region Player Input Commands
    // TASK? // Переделать в ID? Чтобы не передавать целый GO, а одинаковым объектам одинаковый ID его быстрее проверить
    private void IconClicked(GameObject iconGO)
    {

        if (!_findingMatch)
        {
            iconGO.SetActive(false);
            _lastGoClicked = iconGO;

            _lastClickedText = iconGO.GetComponent<TextMeshProUGUI>().text;
            _findingMatch = true;

        }
        else if (iconGO.GetComponent<TextMeshProUGUI>().text == _lastGoClicked.GetComponent<TextMeshProUGUI>().text)
        {
            iconGO.SetActive(false);
            _findingMatch = false;
            _matchesFound++;           
            _pointsNumber++;                // add points for right desicion
            CheckIfLeveIsCompleted();
            UpdateUI();
        }
        else
        {
            _lastGoClicked.SetActive(true);
            _findingMatch = false;
            _pointsNumber -= 3;             // remove points for wrong desicion
            UpdateUI();
        }
    }
    #endregion

    #region In Game Tools
    private void UpdateUI()
    {
        _pointsNumberText.text = "Points: " + _pointsNumber.ToString();
        _levelNumberText.text = "Level #" + _levelNumber.ToString();
    }

    // TASK // Rename and here we have two tasks
    private void CheckIfLeveIsCompleted()
    {
        if (_matchesFound == _maxMatches)
        {
            _levelNumber++;
           
            _ = _levelNumber % 2 == 0 ? _row++ : _coloumn++;    // _ used when we do not need a result
            
            StartGame();
        }
    }
    #endregion

    #endregion
}
