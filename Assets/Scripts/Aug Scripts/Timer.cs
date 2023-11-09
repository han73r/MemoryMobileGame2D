using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

namespace Tools
{
    public class Timer : MonoBehaviour
    {
        private float elapsedTime;
        private bool isRunning;
        private bool isPaused;
        private float pausedTime;
        private float stopTime;

        public event Action<TimeSpan> TimeUpdated;

        private void Start()
        {
            ResetTimer();
        }

        public void StartTimer()
        {
            if (!isRunning)
            {
                isRunning = true;
                isPaused = false;
                pausedTime = 0f;
                StartCoroutine(TimerCoroutine());
            }
        }

        public void PauseTimer()
        {
            if (isRunning && !isPaused)
            {
                isPaused = true;
                pausedTime = elapsedTime;
            }
        }

        private IEnumerator TimerCoroutine()
        {
            while (isRunning)
            {
                yield return null;
                if (!isPaused)
                {
                    elapsedTime += Time.deltaTime;
                    TimeUpdated?.Invoke(TimeSpan.FromSeconds(elapsedTime));
                }
            }
        }

        public void ResetTimer()
        {
            StopAllCoroutines();
            elapsedTime = 0f;
            isRunning = false;
            isPaused = false;
            pausedTime = 0f;
        }

        public TimeSpan StopTimerAndReturnStopTime()
        {
            PauseTimer();
            stopTime = elapsedTime;
            ResetTimer();
            return TimeSpan.FromSeconds(stopTime);
        }

        //public TimeSpan GetElapsedTime()
        //{
        //    return TimeSpan.FromSeconds(elapsedTime);
        //}

    }
}

