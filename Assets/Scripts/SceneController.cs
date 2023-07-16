using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using UnityEngine.EventSystems;

public class SceneController : MonoBehaviour, IPointerClickHandler, IPointerDownHandler
{
    [SerializeField] private GameObject _services;
    [SerializeField] private GameObject _clocks;
    [SerializeField] private GameObject _alarm;
    [SerializeField] private GameObject _message;

    private bool messageState;
    
    public void OnPointerClick(PointerEventData pointerEventData)
    {
        string tag = pointerEventData.pointerCurrentRaycast.gameObject.tag;
        if (tag.Equals("Clocks"))
            EventController.Instance.OnOpenAlarm();
        else if (tag.Equals("Message"))
            EventController.Instance.OnMessage();
        // Debug.Log(tag + " - obj tag");
        // Debug.Log(pointerEventData.pointerCurrentRaycast.gameObject.layer + " Game Object Clicked!");
    }

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        if (pointerEventData.pointerCurrentRaycast.gameObject.tag.Equals("Alarm"))
            EventController.Instance.OnAlarmArrowChange(pointerEventData.pointerCurrentRaycast.gameObject.name);
        Debug.Log(pointerEventData.pointerCurrentRaycast.gameObject.name);
    }

    private void Start()
    {
        messageState = _message.activeSelf;
        // Debug.Log(messageState + " - is message active?");

        EventController.Instance.ActivateClocks += ActivateClocksScene;
        EventController.Instance.OpenAlarm += ActivateAlarmScene;
        EventController.Instance.Message += ChangeMessageScene;
    }

    private void OnDisable()
    {
        EventController.Instance.ActivateClocks -= ActivateClocksScene;
        EventController.Instance.OpenAlarm -= ActivateAlarmScene;
        EventController.Instance.Message -= ChangeMessageScene;
    }

    private void ActivateClocksScene()
    {
        _clocks.SetActive(true);
        _services.SetActive(false);
        _alarm.SetActive(false);
    }

    private void ActivateAlarmScene()
    {
        _alarm.SetActive(true);
        _services.SetActive(false);
        _clocks.SetActive(false);
    }

    private void ChangeMessageScene()
    {
        messageState = !messageState;
        _message.SetActive(messageState);
    }
}
