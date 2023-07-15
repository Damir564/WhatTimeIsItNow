using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System;

public class TimeController : MonoBehaviour
{
    private static readonly string[] URIS = 
    {"https://script.googleusercontent.com/macros/echo?user_content_key=fweK9DPgE7VStiidS_oDiBSJ_hjsIExYzei1ZGK0gdRPZ-um_qu2Q7xtFzs6wRaiGYBrgYBJUSkZ3UFhg519xGw-THB8X7-sm5_BxDlH2jW0nuo2oDemN9CCS2h10ox_1xSncGQajx_ryfhECjZEnJ9GRkcRevgjTvo8Dc32iw_BLJPcPfRdVKhJT5HNzQuXEeN3QFwl2n0M6ZmO-h7C6bwVq0tbM60-YSRgvERRRx91eQMV9hTntRGQmSuaYtHQ&lib=MwxUjRcLr2qLlnVOLh12wSNkqcO1Ikdrk", 
    "http://worldtimeapi.org/api/ip"};

    private int _choosedURI = 0;
    private float _timeFromStartHttpResponded;

    private float _secondsFromClockStart;

    public float SecondsFromClockStart
    {
        get
        {
            return _secondsFromClockStart;
        }
    }

    // [SerializeField] private bool _overrideTime;
    // [SerializeField] private int _inputHours;
    // [SerializeField] private int _inputMinutes;
    // [SerializeField] private int _inputSeconds;

    public int ChoosedURI
    {
        set
        {
            _choosedURI = (value == 0 ? 0 : 1);
            GetRequest();
            EventController.Instance.OnActivateClocks();
        }
    }

    private static TimeController _instance;

    public static TimeController Instance
    {
        get
        {
            return _instance;
        }
    }

    private DateTime _currentTime;

    public DateTime CurrentTime
    {
        get
        {
            _secondsFromClockStart = Time.realtimeSinceStartup - _timeFromStartHttpResponded;
            return _currentTime.AddSeconds(_secondsFromClockStart);
        }
    }

    private bool _hasAlarm = false;

    public bool HasAlarm
    {
        get
        {
            return _hasAlarm;
        }
        set
        {
            _hasAlarm = value;
        }
    }

    private TimeSpan _alarmTime;

    public TimeSpan AlarmTime
    {
        get
        {
            return _alarmTime;
        }
        set
        {
            _alarmTime = value;
        }
    }

    private void ResetAlarm()
    {
        _hasAlarm = false;
        _alarmTime = TimeSpan.MinValue;
    }

    private void SetAlarm(TimeSpan ts)
    {
        _hasAlarm = true;
        _alarmTime = ts;
    }

    public void GetRequest()
    {
        StartCoroutine(GetTimeHttpRequest(URIS[_choosedURI]));
        _timeFromStartHttpResponded = Time.realtimeSinceStartup;
    }

    
    private void Awake()
    {
        if (_instance)
            Destroy(this);
        else
            _instance = this;
    }

    private void Start()
    {
        EventController.Instance.GetRequest += GetRequest;
        EventController.Instance.ResetAlarm += ResetAlarm;
        EventController.Instance.SetAlarm += SetAlarm;
    }

    private void OnDisable()
    {
        EventController.Instance.GetRequest -= GetRequest;
        EventController.Instance.ResetAlarm -= ResetAlarm;
        EventController.Instance.SetAlarm -= SetAlarm;
    }

    IEnumerator GetTimeHttpRequest (string uri)
    {
        UnityWebRequest webRequest = UnityWebRequest.Get(uri);
        yield return webRequest.SendWebRequest();

        if (webRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Error " + webRequest.responseCode);
            EventController.Instance.OnSceneReloaded();
        }
        else
        {
            string jsonText = webRequest.downloadHandler.text;
            if (_choosedURI == 1)
            {
                var data = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonText);
                string dateTimeText = data["datetime"];;
                
                _currentTime = ParseTime(dateTimeText);
            }
            else
            {
                _currentTime = ParseTime(jsonText);
            }
            Debug.Log(_currentTime);
        }
    }

    public static DateTime ParseTime(string datetime)
    {
        string time = Regex.Match(datetime, @"\d{2}:\d{2}:\d{2}").Value;
        return DateTime.Parse(time);
    }


    // private void Update()
    // {
    //     if (_overrideTime)
    //     {
    //         _currentTime = new DateTime(2000, 10, 10, _inputHours, _inputMinutes, _inputSeconds);
    //     }
    // }
}
