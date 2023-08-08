using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

// https://gamedevbeginner.com/how-to-make-countdown-timer-in-unity-minutes-seconds/
// Coroutine or Update for timer?
// https://stackoverflow.com/questions/61464452/in-unity-when-should-i-use-coroutines-versus-subtracting-time-deltatime-in-upda
public class Timer : MonoBehaviour
{
    [SerializeField]
    private float _timeRemaining = 60;

    [SerializeField]
    private TextMeshProUGUI _timerText;

    private bool _timerIsRunning = false;
    private void Start()
    {
        _timerIsRunning = true;
    }

    private void Update()
    {
        if (_timerIsRunning)
        {
            if (_timeRemaining > 0)
            {
                _timeRemaining -= Time.deltaTime;
                
            }
            else
            {
                _timeRemaining = 0;
                _timerIsRunning = false;
            }
            DisplayTime(_timeRemaining);
        }
    }

    private void DisplayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        float miliSeconds = (timeToDisplay % 1) * 1000;
        _timerText.text = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds,miliSeconds);
    }
}
