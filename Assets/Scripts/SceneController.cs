using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using UnityEngine.EventSystems;

public class SceneController : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject _services;
    [SerializeField] private GameObject _clocks;
    [SerializeField] private GameObject _alarm;
    
    public void OnPointerClick(PointerEventData pointerEventData)
    {
        if (pointerEventData.pointerCurrentRaycast.gameObject.tag.Equals("Clocks"))
            EventController.Instance.OnResetAlarm();
        Debug.Log(pointerEventData.pointerCurrentRaycast.gameObject.layer + " Game Object Clicked!");
    }

    private void Start()
    {
        Debug.Log(EventController.Instance);
        EventController.Instance.ActivateClocks += ActivateClocks;
        EventController.Instance.ResetAlarm += ActivateAlarm;
    }

    private void OnDisable()
    {
        EventController.Instance.ActivateClocks -= ActivateClocks;
        EventController.Instance.ResetAlarm -= ActivateAlarm;
    }

    private void ActivateClocks()
    {
        _clocks.SetActive(true);
        _services.SetActive(false);
        _alarm.SetActive(false);
    }

    private void ActivateAlarm()
    {
        _alarm.SetActive(true);
        _services.SetActive(false);
        _clocks.SetActive(false);
    }
}
