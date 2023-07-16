using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System;

public class EventController : MonoBehaviour
{
    private static EventController _instance;

    public static EventController Instance
    {
        get
        {
            return _instance;
        }
    }


    public UnityAction SceneReloaded;
    public UnityAction ActivateClocks;
    public UnityAction OpenAlarm;
    public UnityAction ResetAlarm;
    public UnityAction Message;
    public event Action<TimeSpan, bool> SetAlarm;
    public UnityAction GetRequest;
    // public event Action<string> AlarmArrowChange;

    public void OnAlarmArrowChange(string gameObj)
    {

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
        SceneReloaded += ReloadScene;
    }

    private void OnDisable()
    {
        SceneReloaded -= ReloadScene;
    }
    
    public void OnSceneReloaded()
    {
        SceneReloaded?.Invoke();
    }

    public void OnActivateClocks()
    {
        ActivateClocks?.Invoke();
    }

    public void OnGetRequest()
    {
        GetRequest?.Invoke();
    }

    private void ReloadScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
        Debug.Log("Scene Reloaded");
    }

    public void OnOpenAlarm()
    {
        OnResetAlarm();

        EventController.Instance.OpenAlarm?.Invoke();
    }

    public void OnMessage()
    {
        EventController.Instance.Message?.Invoke();
    }

    public void OnResetAlarm()
    {
        EventController.Instance.ResetAlarm?.Invoke();
    }

    public void OnSetAlarm(TimeSpan ts, bool isAnalog)
    {
        EventController.Instance.SetAlarm?.Invoke(ts, isAnalog);
    }
}
