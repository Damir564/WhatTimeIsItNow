using System.Text.RegularExpressions;
using UnityEngine;
using TMPro;
using System;

public class Alarm : MonoBehaviour
{
    [SerializeField] private TMP_InputField _timeInput;

    private int prevLength = -1;
    private TimeSpan _alarmTimeSpan;

    private bool _timeCorrect = false;

    // Start is called before the first frame update
    private void Start()
    {
        EventController.Instance.ResetAlarm += ResetAlarm;

        EventController.Instance.OnResetAlarm();
    }

    private void OnDisable()
    {
        EventController.Instance.ResetAlarm -= ResetAlarm;
    }

    private void ResetAlarm()
    {
        prevLength = -1;
        _alarmTimeSpan = TimeSpan.MinValue;
        _timeInput.text = "";
        _timeCorrect = false;
    }

    public void OnValueChanged(string value)
    {
        int currentLength = _timeInput.text.Length;
        // HandleOnlyDigitsInput(currentLength);
        if (currentLength > prevLength &&
            (currentLength == 2 || currentLength == 5))
            AddColon(true, currentLength);
        else if (currentLength > 0 && _timeInput.text[currentLength - 1] != ':' &&
            (currentLength == 3 || currentLength == 6))
            AddColon(false, currentLength);
        prevLength = _timeInput.text.Length;

        HandleOnlyDigitsInput(prevLength);
        if (prevLength == 8)
            CheckDigits(prevLength);
    }

    private void HandleOnlyDigitsInput(int currentLength)
    {
        if (currentLength == 0)
            return;
        char lastCharacter = _timeInput.text[currentLength - 1];
        // Debug.Log("LastCharacter: " + lastCharacter);
        // Make dependency on length so if ":" added it is get counted

        if (lastCharacter < '0' || lastCharacter > '9')
            DeleteLastChar(currentLength);
        else
            HandleMaxValue(currentLength);
    }

    private void HandleMaxValue(int currentLength)
    {
        int tempValue = 0;
        int tempValue2 = 0;
        tempValue = Int32.Parse(_timeInput.text[currentLength - 1].ToString());
        switch (currentLength)
        {
            case 1:
                if (tempValue > 2)
                    DeleteLastChar(currentLength);
                break;
            case 2:
                tempValue2 = Int32.Parse(_timeInput.text[0].ToString());
                if (tempValue2 * 10 + tempValue > 23)
                    DeleteLastChar(currentLength);
                break;
            case 4:
                if (tempValue > 5)
                    DeleteLastChar(currentLength);
                break;
            case 5:
                tempValue2 = Int32.Parse(_timeInput.text[3].ToString());
                if (tempValue2 * 10 + tempValue > 59)
                    DeleteLastChar(currentLength);
                break;
            case 7:
                if (tempValue > 5)
                    DeleteLastChar(currentLength);
                break;
            case 8:
                tempValue2 = Int32.Parse(_timeInput.text[6].ToString());
                if (tempValue2 * 10 + tempValue > 59)
                    DeleteLastChar(currentLength);
                break;
            default:
                break;
        }
    }

    private void DeleteLastChar(int currentLength)
    {
        if (currentLength == 1)
            _timeInput.text = "";
        else
            _timeInput.text = _timeInput.text.Substring(0, currentLength - 1);
    }

    private void AddColon(bool isLast, int currentLength)
    {
        if (isLast)
            _timeInput.text = _timeInput.text + ":";
        else
            _timeInput.text = _timeInput.text.Substring(0, currentLength - 1)  +
            ":" + _timeInput.text[currentLength - 1];
        _timeInput.stringPosition++;
    }

    private void CheckDigits(int currentLength)
    {
        Regex regex = new Regex(@"\d{2}:\d{2}:\d{2}");
        _timeCorrect = regex.IsMatch(_timeInput.text);
        Debug.Log(_timeCorrect);
        if (!_timeCorrect)
            return;
        _alarmTimeSpan = AlarmSetTime(_timeInput.text);
    }

    public void SetAlarm()
    {
        EventController.Instance.OnSetAlarm(_alarmTimeSpan);
        EventController.Instance.OnActivateClocks();
    }

    private TimeSpan AlarmSetTime(string time)
    {
        return TimeSpan.Parse(time);
    }
}
