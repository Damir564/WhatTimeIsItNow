using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    [SerializeField] private GameObject _services;
    [SerializeField] private GameObject _clocks;
    
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
        _clocks.SetActive(true);
        _services.SetActive(false);
    }
}
