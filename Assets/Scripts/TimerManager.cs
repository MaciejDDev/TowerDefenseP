using System;
using TMPro;
using UnityEngine;

public class TimerManager : MonoBehaviour 
{
    [Header("Timer Text")]
    [SerializeField] TMP_Text _timerText;

    float _timerTime;

    public float TimerTime => _timerTime;

    public void Init()
    {
        _timerTime = 0;
    }

    private void Update()
    {
        if (!GameManager.Instance.IsGamePaused)
            UpdateTimerText();
    }



    private void UpdateTimerText()
    {
        _timerTime += Time.deltaTime;
        TimeSpan timeSpan = TimeSpan.FromSeconds(_timerTime);
        _timerText.text = string.Format("{0:00}:{1:00}",(int) timeSpan.TotalMinutes,(int) timeSpan.Seconds);
    }


}
