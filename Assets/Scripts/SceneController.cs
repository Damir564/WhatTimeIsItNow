using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    [SerializeField] private GameObject _services;
    [SerializeField] private GameObject _clocks;
    [SerializeField] private GameObject _alarm;
    
    private void Start()
    {
        Debug.Log(EventController.Instance);
        EventController.Instance.ActivateClocks += ActivateClocks;
    }

    private void OnDisable()
    {
        EventController.Instance.ActivateClocks -= ActivateClocks;
    }

    private void ActivateClocks()
    {
        _services.SetActive(false);
        _clocks.SetActive(true);
        _alarm.SetActive(false);
    }

    private void ActivateAlarm()
    {
        _services.SetActive(false);
        _clocks.SetActive(false);       
        _alarm.SetActive(true);
    }
}
