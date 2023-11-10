using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Tools;
using System;

namespace Managers
{
    public class UiManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _debugTimer;

        private Timer timer;

        private void Start()
        {
            GameManager gameManager = FindObjectOfType<GameManager>();
            if (gameManager != null)
            {
                timer = gameManager.GetTimer();
                timer.TimeUpdated += UpdateTimerUI;
            }
            else
            {
                Debug.LogError("GameManager not found!");
            }
        }
        private void OnDestroy()
        {
            if (timer != null)
            {
                timer.TimeUpdated -= UpdateTimerUI;
            }
        }

        private void UpdateTimerUI(TimeSpan time)
        {
            string formattedTime = $"{time.Hours:D2}:{time.Minutes:D2}:{time.Seconds:D2}.{time.Milliseconds:D3}";
            _debugTimer.text = formattedTime;
        }

    }

}
