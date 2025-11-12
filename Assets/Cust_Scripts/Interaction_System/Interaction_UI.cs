using System;
using TMPro;
using UnityEngine;

public class Interaction_UI : MonoBehaviour
{
    [SerializeField] private GameObject _UI_Pannel;
    [SerializeField]private TextMeshProUGUI _promptText;
    private Camera _mainCamera;

    private void Start()
    {
        _mainCamera = Camera.main;
        if (_UI_Pannel != null)
        {
            _UI_Pannel.SetActive(false);
        }
    }

    void LateUpdate()
    {
        if (_mainCamera != null)
        {
            transform.LookAt(_mainCamera.transform);
        }
    }

    public bool isDisplayed = false;
    public void SetUp(string promptText)
    {
        if (_promptText != null)
        {
            _promptText.text = promptText;
        }
        if (_UI_Pannel != null)
        {
            _UI_Pannel.SetActive(true);
        }
        isDisplayed = true;
    }
    
    public void Close()
    {
        if (_UI_Pannel != null)
        {
            _UI_Pannel.SetActive(false);
        }
        isDisplayed = false;
    }
}
