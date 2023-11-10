using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

namespace Tools
{
    public class Timer : MonoBehaviour
    {       
        private bool _isRunning;
        private bool _isPaused;

        private float _elapsedTime;
        private float _pausedTime;
        private float _stopTime;
        private float _countdownTime;

        private Coroutine _countupCoroutine;
        private Coroutine _countdownCoroutine;

        public event Action<TimeSpan> TimeUpdated;
        public event Action onCountdownFinished;

        private void Start()
        {
            ResetTimer();
        }
        public void StartCountup()
        {
            if (!_isRunning)
            {
                _isRunning = true;
                _isPaused = false;
                _pausedTime = 0f;
                StartCoroutine(CountupCoroutine());
            }
        }
        public void StartCoutdown(TimeSpan countdownTime, Action onCountdownFinished)
        {
            if (_isRunning) { StopTimerAndReturnStopTime(); }
            _countdownTime = (float)countdownTime.TotalSeconds;
            this.onCountdownFinished = onCountdownFinished;
            _isRunning = true;
            _isPaused = false;
            _elapsedTime = 0f;
            StartCoroutine(CountdownCoroutine());
        }
        
        public void PauseTimer()
        {
            if (_isRunning && !_isPaused)
            {
                _isPaused = true;
                _pausedTime = _elapsedTime;
            }
        }
        public void ResetTimer()
        {
            StopAllCoroutines();
            _elapsedTime = 0f;
            _isRunning = false;
            _isPaused = false;
            _pausedTime = 0f;
        }

        public TimeSpan StopTimerAndReturnStopTime()
        {
            PauseTimer();
            _stopTime = _elapsedTime;
            ResetTimer();
            return TimeSpan.FromSeconds(_stopTime);
        }

        private IEnumerator CountupCoroutine()
        {
            while (_isRunning)
            {
                yield return null;
                if (!_isPaused)
                {
                    _elapsedTime += Time.deltaTime;
                    TimeUpdated?.Invoke(TimeSpan.FromSeconds(_elapsedTime));
                }
            }
        }

        private IEnumerator CountdownCoroutine()
        {
            while (_isRunning)
            {
                yield return null;
                if (!_isPaused)
                {
                    _elapsedTime += Time.deltaTime;

                    if (_elapsedTime >= _countdownTime)
                    {
                        _isRunning = false;
                        onCountdownFinished?.Invoke();
                    }
                }
            }
        }

        //public TimeSpan GetElapsedTime()
        //{
        //    return TimeSpan.FromSeconds(elapsedTime);
        //}

    }
}

