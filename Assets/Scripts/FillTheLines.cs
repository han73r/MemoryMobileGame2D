using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using System;

public class FillTheLines : MonoBehaviour
{
    /*
    private class AnimalEnoji
    {
        List<string> animalEmoji = new()
            {
                "🙈","🙈",
                "🐺","🐺",
                "🦁","🦁",
                "🦄","🦄",
                "🐷","🐷",
                "🐼","🐼",
                "🦇","🦇",
                "🐸","🐸",
            };
    }
    */

    [SerializeField]
    private GameObject _grid;

    [SerializeField]
    private GameObject _gridRow;

    [SerializeField]
    private GameObject _gridRowPrefab;

    [SerializeField]
    private GameObject _iconPrefab;

    /*
    [SerializeField]
    private List<GameObject> _rowGoList;   
    */

    private List<string> _faceEmoji = new()
    {
        "😁",
        "😁",
        "😁",
        "😁",
        "😂",
        "😂",
        "😃",
        "😃",
        "😅",
        "😅",
        "😆",
        "😆",
        "😈",
        "😈",
        "😋",
        "😋",
    };

    [SerializeField]
    private List<GameObject> _myIconsGOList;

    [SerializeField]
    private List<TextMeshProUGUI> _myTextIconsList;


    private void Start()
    {
        //SetUpGame();
        //var myEmojiList = new List<string>();
        CreateIconsFromPrefabs(4,4);
        SetEmoticonsToIconList(_faceEmoji);                                 // input different list types

    }

    private void CreateIconsFromPrefabs(int row, int coloumn)
    {
        _myIconsGOList = new();
        _myTextIconsList = new();

        for (int i = 0; i < row; i++)
        {
            var rowGO = Instantiate(_gridRowPrefab, _grid.transform);
            for (int j = 0; j < coloumn; j++)
            {
                var iconGO = Instantiate(_iconPrefab, rowGO.transform);
                _myIconsGOList.Add(iconGO);
                _myTextIconsList.Add(iconGO.GetComponent<TextMeshProUGUI>());
            }
        }
    }

    private void SetEmoticonsToIconList(List<string> list)
    {
        // TASK // check that list count and textIconList have same numbers
        var random = new System.Random();

        foreach (var icon in _myTextIconsList)
        {
            int index = random.Next(list.Count);
            string nextEmoji = list[index];
            icon.text = nextEmoji;
            list.RemoveAt(index);
        }
    }

    /*
    private void SetUpGame()
    {

        // Find All Text
        _rowGoList = GetAllChilds(_grid);
        _tableIconsList = GetAllTextMeshProUGUI(_rowGoList);


        var random = new System.Random();   // better then static Unity Random
        // Set icons from list to Text and 
        foreach (var text in _tableIconsList)
        {
            int index = random.Next(animalEmoji.Count);
            string nextEmoji = animalEmoji[index];
            text.text = nextEmoji;
            animalEmoji.RemoveAt(index);
        }

        Debug.Log("StopHere");
    }
    */
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
                    // экстравагантным способом скроем уже взятые значения))
                    // сработало но ток на первый ряд)
                    txtsList[j].gameObject.SetActive(false);
                }
            }
        foreach (var txt in txtsList)
        {
            txt.gameObject.SetActive(true);
        }
        return txtsList;

    }

}
