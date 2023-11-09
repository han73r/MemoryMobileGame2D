using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Tools;

namespace Managers
{
    public class UiManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _debugTimer;

        //private void Start()
        //{
        //    // Подписываемся на событие TimeUpdated
        //    Timer.TimeUpdated += UpdateTimerUI;
        //}
        //private void OnDestroy()
        //{
        //    // Обязательно отписываемся при уничтожении объекта
        //    Timer.TimeUpdated -= UpdateTimerUI;
        //}

        //private void UpdateTimerUI(TimeSpan time)
        //{
        //    string formattedTime = $"{time.Hours:D2}:{time.Minutes:D2}:{time.Seconds:D2}.{time.Milliseconds:D3}";
        //    _debugTimer.text = formattedTime;
        //}

    }

}
