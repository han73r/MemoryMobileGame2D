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

    private static MenuLevel currentMenuLevel = MenuLevel.Main;
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
    public void UpdateThemeButtonsAvailability(Dictionary<LevelThemeName, Dictionary<LevelTypeName, List<Level>>> levelsDict)
    {
        foreach (var theme in levelsDict.Keys)
        {
            bool isOpenedLevelExists = levelsDict[theme].Any(typeLevels => typeLevels.Value.Any(level => level.DynamicData.IsOpened));
            Button themeButton = GetThemeButtonByThemeName(theme);
            themeButton.interactable = isOpenedLevelExists;
        }
    }
    public void StartNewLevel(bool isActive)
    {
        Tool.SetTransformActive(_typeButtonContainer, !isActive);
        if (isActive) { currentMenuLevel = MenuLevel.InGame; }
    }
    private Button GetThemeButtonByThemeName(LevelThemeName themeName)
    {
        foreach (Transform button in _themeButtonContainer)
        {
            Button themeButton = button.GetComponent<Button>();
            TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();

            if (buttonText.text == themeName.ToString())
            {
                return themeButton;
            }
        }
        return null;
    }

    private void CreateTypeButtons(LevelThemeName selectedTheme, bool updateButtons = false)
    {
        if (updateButtons)
        {
            foreach (Transform child in _themeButtonContainer)
            {
                Destroy(child.gameObject);
            }
        }

        currentMenuLevel = MenuLevel.Type;
        var levelTypesForTheme = GameManager.levelsDict[selectedTheme]; // Get Level Types

        foreach (var levelType in levelTypesForTheme.Keys)
        {
            GameObject buttonGO = Instantiate(_typeButtonPrefab, _themeButtonContainer);
            Button button = buttonGO.GetComponent<Button>();
            TextMeshProUGUI buttonText = buttonGO.GetComponentInChildren<TextMeshProUGUI>();

            buttonText.text = levelType.ToString();

            // At least one level should be IsOpened = true to make the Type button interactable
            var openedLevel = levelTypesForTheme[levelType].FirstOrDefault(level => level.DynamicData.IsOpened);

            button.interactable = (openedLevel != null);

            // Capture the openedLevel in a closure and pass it to OnLevelTypeButtonClicked
            if (openedLevel != null)
            {
                Level level = openedLevel;
                button.onClick.AddListener(() =>
                {
                    OnLevelTypeButtonClicked(level);
                });
            }
            else
            {
                // Handle the case when there are no opened levels of this type
            }
        }
    }
    public void UpdateTypeButtons(LevelThemeName selectedTheme)
    {
        CreateTypeButtons(selectedTheme, true);
    }

    private void OnThemeButtonClicked(LevelThemeName selectedTheme)
    {
        Debug.Log("Selected Theme: " + selectedTheme.ToString());
        Tool.RemoveChildObjects(_themeButtonContainer);
        CreateTypeButtons(selectedTheme);
    }
    private void OnLevelTypeButtonClicked(Level level)
    {
        Debug.Log("Selected Type: " + level.LevelType.Name);
        GameManager.Instance.CreateLevel(level);
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
            case MenuLevel.InGame:
                StartNewLevel(false);
                GameManager.Instance.DestroyLevel();
                currentMenuLevel = MenuLevel.Type;
                break;
            default:
                break;
        }
    }

}
