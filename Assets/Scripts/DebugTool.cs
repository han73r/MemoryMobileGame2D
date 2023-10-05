using UnityEngine;
using Data;
using TMPro;
using System;
using System.Collections.Generic;

public class DebugTool : MonoBehaviour
{
    [SerializeField] private TMP_InputField _inputField;
    [SerializeField] private TextMeshProUGUI _consoleText;
    [SerializeField] private GameObject _consoleGO;

    private bool _isConsoleVisible = false;
    private const int maxCommandWidth = 25;

    void Start()
    {
        //_inputField.onValueChanged.AddListener(OnInputFieldValueChanged);        // for history navigation
        _inputField.onEndEdit.AddListener(OnInputFieldEndEdit);                 // for commands
        ClearConsole();
    }

    private void Update()
    {
        // use (~) key to open cosole
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            ToggleConsoleVisibility();
        }
    }

    private void OnInputFieldEndEdit(string inputText)
    {
        ClearConsole();
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            string[] inputParts = inputText.Split(' ');

            if (inputParts.Length > 0)
            {
                string command = inputParts[0].ToLower();

                switch (command)
                {
                    case "help":
                        DisplayHelp();
                        break;
                    case "open":                        
                        if (inputParts.Length >= 2)
                        {
                            string themeName = inputParts[1].ToLower();
                            OpenTheme(themeName);
                        }
                        else
                        {
                            LogToConsole("Invalid 'open' command format. Usage: open [themeName]");
                        }
                        break;
                    case "close":
                        if (inputParts.Length >= 2)
                        {
                            string themeName = inputParts[1].ToLower();
                            CloseTheme(themeName);
                        }
                        else
                        {
                            LogToConsole("Invalid 'close' command format. Usage: close [themeName]");
                        }
                        break;
                    case "theme":
                        DisplayThemes();
                        break;
                    default:
                        LogToConsole("Type 'help' to see available commands.");
                        break;
                }
                _inputField.text = "";
            }
        }
    }

    private void DisplayHelp()
    {
        LogToConsole("Available commands:");
        LogToConsole("'help'", "Display list of commands");
        LogToConsole("'open [themeName]'", "Open a theme by name");
        LogToConsole("'close [themeName]'", "Close a theme by name");
        LogToConsole("'theme'", "List of available themes");
    }

    private void LogToConsole(string command, string description)
    {
        int paddingWidth = maxCommandWidth - command.Length;
        var padding = new string(' ', paddingWidth);

        string formattedText = $"{command}{padding} - {description}";
        _consoleText.text += formattedText + "\n";
    }
    private void LogToConsole(string message)
    {
        _consoleText.text += message + "\n";
    }
    private void OpenTheme(string themeName)
    {
        if (Enum.TryParse(themeName, true, out LevelThemeName theme))
        {
            Debug.Log($"Opening theme: {theme}");
            GameManager.Instance.OpenTheme(theme);         
        }
        else
        {
            Debug.LogWarning($"Theme '{themeName}' not found.");
        }
    }
    private void CloseTheme(string themeName)
    {
        if (Enum.TryParse(themeName, true, out LevelThemeName theme))
        {
            GameManager.Instance.CloseTheme(theme);
            Debug.Log($"Closing theme: {theme}");
        }
        else
        {
            Debug.LogWarning($"Theme '{themeName}' not found.");
        }
    }
    private void DisplayThemes()
    {
        LogToConsole("Available themes:");
        foreach (LevelThemeName theme in Enum.GetValues(typeof(LevelThemeName)))
        {
            LogToConsole(theme.ToString());
        }
    }

    private void ClearConsole()
    {
        _consoleText.text = "";
        _inputField.text = "";
        _inputField.ActivateInputField();
        _inputField.caretBlinkRate = 1.0f;
    }
    private void ToggleConsoleVisibility()
    {
        if (_consoleGO != null)
        {
            _consoleGO.SetActive(!_isConsoleVisible);
            ClearConsole();
        }

        _isConsoleVisible = !_isConsoleVisible;
    }
}
