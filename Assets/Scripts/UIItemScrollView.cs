using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIItemScrollView : MonoBehaviour
{
    [SerializeField] private InfiniteScroll _infiniteScroll; 
    [SerializeField] private int _stopIndex; 
    [Header("Event Listener")]

    [SerializeField] private ItemDataSOsEventChannel _onAllRequiredItemSelectedEvent;
    [SerializeField] private ItemDataSOsEventChannel _onSetupItemDatasEvent;

    void OnEnable()
    {
        _onAllRequiredItemSelectedEvent.OnEventRaised += OnAllRequiredItemSelectedReceived;
        _onSetupItemDatasEvent.OnEventRaised += OnSetupItemDatasReceived;
    }

    void OnDisable()
    {
        
        _onSetupItemDatasEvent.OnEventRaised -= OnAllRequiredItemSelectedReceived;
        _onSetupItemDatasEvent.OnEventRaised -= OnSetupItemDatasReceived;
    }

    private void OnSetupItemDatasReceived(List<ItemDataSO> list)
    {
        _infiniteScroll.Initialize(list);
    }

    private void OnAllRequiredItemSelectedReceived(List<ItemDataSO> list)
    {
        _infiniteScroll.SetDecreaseSpeed(list, _stopIndex);
    }
}
