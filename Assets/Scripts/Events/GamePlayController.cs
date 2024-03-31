using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayController : MonoBehaviour
{
    [SerializeField] private List<ItemDataSO> _allItems;

    [Header("Event Listener")]
    [SerializeField] private VoidEventChannel _onGamePlayRestartEvent;

    [Header("Event Raiser")]
    [SerializeField] private ItemDataSOsEventChannel _onSetupItemDatasEvent;

    void OnEnable()
    {
        _onGamePlayRestartEvent.OnEventRaised += Initialize;
    }

    void OnDisable()
    {
        _onGamePlayRestartEvent.OnEventRaised -= Initialize;
    }

    void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        _onSetupItemDatasEvent.RaiseEvent(_allItems);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Initialize();
        }
    }
}
