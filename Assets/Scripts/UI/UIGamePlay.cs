using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGamePlay : MonoBehaviour
{
    [SerializeField] private Button _btnRestart;

    [SerializeField] private VoidEventChannel _onGamePlayRestartEvent;

    void OnEnable()
    {
        _btnRestart.onClick.AddListener(OnButtonRestartClicked);
    }

    void OnDisable()
    {
        _btnRestart.onClick.RemoveAllListeners();
    }

    private void OnButtonRestartClicked()
    {
        _onGamePlayRestartEvent.RaiseEvent();
    }
}
