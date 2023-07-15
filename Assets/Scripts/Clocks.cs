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
        TimeSpan timeSpan = TimeController.Instance.CurrentTime.TimeOfDay;
        DigitsUpdate(timeSpan);
        OriginUpdate(timeSpan);
        if (TimeController.Instance.SecondsFromClockStart > checkTimeSeconds)
            EventController.Instance.OnGetRequest();
        if (TimeController.Instance.HasAlarm && timeSpan == TimeController.Instance.AlarmTime)
            Debug.Log("Alarm");
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
