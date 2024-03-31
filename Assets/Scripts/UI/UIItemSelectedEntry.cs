using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIItemSelectedEntry : UIItemEntry
{
    [Header("UIItemSelected Setting")]
    [SerializeField] private Button _btnClick;
    [SerializeField] private Animator _animator;

    [Header("Event Raiser")]
    [SerializeField] private ItemDataSOEventChannel _onItemSelectedEvent;

    void OnEnable()
    {
        _btnClick.onClick.AddListener(OnItemClicked);
    }

    void OnDisable()
    {
        _btnClick.onClick.RemoveAllListeners();
    }

    private void OnItemClicked()
    {
        _onItemSelectedEvent.RaiseEvent(_currentItemData);
    }
}
