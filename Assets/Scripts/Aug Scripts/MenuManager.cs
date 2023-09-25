using UnityEngine;
using UnityEngine.UI;
using System;
using Data;
using TMPro;
using System.Collections.Generic;
using Tools;
using System.Linq;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject _themeButtonPrefab;
    [SerializeField] private GameObject _typeButtonPrefab;
    [SerializeField] private Transform _themeButtonContainer;
    [SerializeField] private Transform _typeButtonContainer;

    private MenuLevel currentMenuLevel = MenuLevel.Main;
    //[SerializeField] private ScrollRect _scrollRect;              // TASK // ADD for swiping between Themes

    public void CreateThemeButtons(Dictionary<LevelThemeName, Dictionary<LevelTypeName, List<Level>>> levelsDict)
    {
        currentMenuLevel = MenuLevel.Theme;

        foreach (var theme in levelsDict.Keys)
        {
            LevelThemeName tempTheme = theme;
            GameObject buttonGO = Instantiate(_themeButtonPrefab, _themeButtonContainer);
            Button button = buttonGO.GetComponent<Button>();
            TextMeshProUGUI buttonText = buttonGO.GetComponentInChildren<TextMeshProUGUI>();

            buttonText.text = theme.ToString();

            // At least one level should be IsOpened = true to active the Theme button
            bool isOpenedLevelExists = levelsDict[theme].Any(typeLevels => typeLevels.Value.Any(level => level.DynamicData.IsOpened));

            button.interactable = isOpenedLevelExists;

            button.onClick.AddListener(() => OnThemeButtonClicked(tempTheme));
        }
    }

    private void OnThemeButtonClicked(LevelThemeName selectedTheme)
    {
        Debug.Log("Selected Theme: " + selectedTheme.ToString());
        Tool.RemoveChildObjects(_themeButtonContainer);
        CreateSubmenu(selectedTheme);
    }

    private void CreateSubmenu(LevelThemeName selectedTheme)
    {
        currentMenuLevel = MenuLevel.Type;

        var levelTypesForTheme = GameManager.levelsDict[selectedTheme]; // Get Level Types

        foreach (var levelType in levelTypesForTheme.Keys)
        {
            // At least one level should be IsOpened = true to active the Type button
            bool isOpenedLevelExists = levelTypesForTheme[levelType].Any(level => level.DynamicData.IsOpened);

            GameObject buttonGO = Instantiate(_typeButtonPrefab, _themeButtonContainer);
            Button button = buttonGO.GetComponent<Button>();
            TextMeshProUGUI buttonText = buttonGO.GetComponentInChildren<TextMeshProUGUI>();

            buttonText.text = levelType.ToString();

            button.interactable = isOpenedLevelExists;

            button.onClick.AddListener(() => OnLevelTypeButtonClicked(levelType));
        }
    }

    private void OnLevelTypeButtonClicked(LevelTypeName selectedType)
    {
        Debug.Log("Selected Type: " + selectedType.ToString());
    }

    public void GoBack()
    {
        switch (currentMenuLevel)
        {
            case MenuLevel.Main:
                // TASK // Add Question before quit
                // TASK // Save all before quit or before going to Main!
                // TASK // Add Start Game button here
                Debug.Log("Disable back button OR Quit");
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#endif
                Application.Quit();                                                             
                break;
            case MenuLevel.Theme:
                Tool.RemoveChildObjects(_themeButtonContainer);
                // TASK // Build Main screen
                currentMenuLevel = MenuLevel.Main;
#if UNITY_EDITOR                                                                
                UnityEditor.EditorApplication.isPlaying = false;                                // Temporary
#endif
                Application.Quit();                                                             // Temporary
                break;
            case MenuLevel.Type:
                Tool.RemoveChildObjects(_typeButtonContainer);
                CreateThemeButtons(GameManager.levelsDict);
                currentMenuLevel = MenuLevel.Theme;
                break;
            default:
                break;
        }
    }
}
