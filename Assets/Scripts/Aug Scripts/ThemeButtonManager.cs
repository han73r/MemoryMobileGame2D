using UnityEngine;
using UnityEngine.UI;
using System;
using Data;
using TMPro;

public class ThemeButtonManager : MonoBehaviour
{
    [SerializeField] private GameObject _themeButtonPrefab;
    [SerializeField] private Transform _themeButtonContainer;
    //[SerializeField] private ScrollRect _scrollRect;

    public void CreateThemeButtons()
    {
        foreach (var theme in EnumHelper.GetValues<LevelThemeName>())
        {
            LevelThemeName tempTheme = theme;
            GameObject buttonObject = Instantiate(_themeButtonPrefab, _themeButtonContainer);
            Button button = buttonObject.GetComponent<Button>();
            TextMeshProUGUI buttonText = buttonObject.GetComponentInChildren<TextMeshProUGUI>();

            buttonText.text = theme.ToString();

            button.onClick.AddListener(() => OnThemeButtonClicked(tempTheme));
        }
    }

    private void OnThemeButtonClicked(LevelThemeName selectedTheme)
    {
        Debug.Log("Selected Theme: " + selectedTheme.ToString());
    }
}
