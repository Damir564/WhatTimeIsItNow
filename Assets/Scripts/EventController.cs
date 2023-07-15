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
    public UnityAction ResetAlarm;
    public event Action<TimeSpan> SetAlarm;
    public UnityAction GetRequest;

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

    public void OnResetAlarm()
    {
        EventController.Instance.ResetAlarm?.Invoke();
    }

    public void OnSetAlarm(TimeSpan ts)
    {
        EventController.Instance.SetAlarm?.Invoke(ts);
    }
}
