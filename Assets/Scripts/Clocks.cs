using UnityEngine;
using TMPro;
using System;

public class Clocks : MonoBehaviour
{
    [SerializeField] private TMP_Text _digitClocks;
    [SerializeField] private RectTransform _secondsArrow;
    [SerializeField] private RectTransform _minutesArrow;
    [SerializeField] private RectTransform _hoursArrow;

    [SerializeField] private float checkTimeSeconds; // 3600f

    void Update()
    {
        TimeSpan timeSpan = TimeController.Instance.CurrentTime;
        DigitsUpdate(timeSpan);
        OriginUpdate(timeSpan);
        CheckSecondsFromStart();
        CheckAlarmTime();
        
    }

    private void CheckSecondsFromStart()
    {
        if (TimeController.Instance.SecondsFromClockStart > checkTimeSeconds)
            EventController.Instance.OnGetRequest();
    }

    private void CheckAlarmTime()
    {
        // Debug.Log((TimeController.Instance.AlarmTime - timeSpan).TotalSeconds.ToString() + 
        // "\n" + 
        // ((TimeController.Instance.AlarmTime - timeSpan).TotalSeconds > 1).ToString());
        if (TimeController.Instance.HasAlarm)
        {
            TimeSpan timeSpan;
            if (TimeController.Instance.IsAnalog)
                timeSpan = TimeController.Instance.CurrentTime12;
            else
                timeSpan = TimeController.Instance.CurrentTime;
            TimeSpan alarmTimeSpan = TimeController.Instance.AlarmTime;
            if (alarmTimeSpan <= timeSpan
            && timeSpan <= alarmTimeSpan.Add(TimeSpan.FromSeconds(1.0)))
            {
                Debug.Log("Alarm");
                EventController.Instance.OnMessage();
                EventController.Instance.OnResetAlarm();
            }
        }
    }

    private void OriginUpdate(in TimeSpan timeSpan)
    {
        float seconds = (float)timeSpan.Seconds;
        float minutes = (float)timeSpan.Minutes;
        float hours = (float)timeSpan.Hours;
        float secondsArrowValue = -seconds * 6f;
        float minutesArrowValue = -minutes * 6f;
        float hoursArrowValue = -((hours % 12) * 30f + minutes * 0.5f);
        _secondsArrow.localRotation = Quaternion.Euler(0, 0, secondsArrowValue);
        _minutesArrow.localRotation = Quaternion.Euler(0, 0, minutesArrowValue);
        _hoursArrow.localRotation = Quaternion.Euler(0, 0, hoursArrowValue);
    }

    private void DigitsUpdate(in TimeSpan timeSpan)
    {
        _digitClocks.text = timeSpan.ToString(@"hh\:mm\:ss");
    }
}
