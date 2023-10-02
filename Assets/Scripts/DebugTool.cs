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

    private List<string> commandHistory = new List<string>();
    private int _currentHistoryIndex = -1;
    private bool _isConsoleVisible = false;

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

    //private void OnInputFieldValueChanged(string newValue)
    //{
    //    if (Input.GetKeyDown(KeyCode.UpArrow))
    //    {
    //        ShowPreviousCommand();
    //    }
    //    else if (Input.GetKeyDown(KeyCode.DownArrow))
    //    {
    //        ShowNextCommand();
    //    }
    //    // TASK // may use for autoadding command
    //}

    private void OnInputFieldEndEdit(string inputText)
    {
        ClearConsole();
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            // history
            commandHistory.Add(inputText);
            _currentHistoryIndex = -1;

            string[] inputParts = inputText.Split(' ');

            if (inputParts.Length > 0)
            {
                string command = inputParts[0].ToLower();

                switch (command)
                {
                    case "help":
                        DisplayHelp();
                        break;
                    //TASK // add open level
                    // TASK // add close theme and level
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
        LogToConsole("'help'                     \t\t\t- Display list of commands");
        LogToConsole("'open [themeName]'    \t- Open a theme by name");
        LogToConsole("'close [themeName]'    \t- Close a theme by name");
        LogToConsole("'theme'                    \t\t- List of avaliable themes");
    }

    private void OpenTheme(string themeName)
    {
        if (Enum.TryParse(themeName, true, out LevelThemeName theme))
        {
            GameManager.Instance.OpenTheme(theme);
            Debug.Log($"Opening theme: {theme}");
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
    private void LogToConsole(string message)
    {
        _consoleText.text += message + "\n";
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
    public enum ConsoleCommand
    {
        Help,
        OpenTheme,
        CloseTheme
    }

    //private void ShowPreviousCommand()
    //{
    //    if (commandHistory.Count > 0)
    //    {
    //        currentHistoryIndex = Mathf.Clamp(currentHistoryIndex + 1, 0, commandHistory.Count - 1);
    //        _inputField.text = commandHistory[currentHistoryIndex];
    //    }
    //}

    //private void ShowNextCommand()
    //{
    //    if (commandHistory.Count > 0)
    //    {
    //        currentHistoryIndex = Mathf.Clamp(currentHistoryIndex - 1, -1, commandHistory.Count - 1);
    //        if (currentHistoryIndex == -1)
    //        {
    //            _inputField.text = "";
    //        }
    //        else
    //        {
    //            _inputField.text = commandHistory[currentHistoryIndex];
    //        }
    //    }
    //}

}
