using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UIItemSelected : MonoBehaviour
{
    [SerializeField] private List<UIItemSelectedEntry> _uIItemSelectedEntries;

    [SerializeField] private List<UIItemEntry> _uIItemSelectingEntries;
    [SerializeField] private int _totalSelectedItem;

    [Header("Event Listener")]
    [SerializeField] private ItemDataSOsEventChannel _onSetupItemDatasEvent;

    [SerializeField] private ItemDataSOEventChannel _onSelectItemDataEvent;
    [SerializeField] private ItemDataSOsEventChannel _onAllRequiredItemSelected;


    private List<ItemDataSO> _selectedItem;

    void OnEnable()
    {
        _onSetupItemDatasEvent.OnEventRaised += Initialize;
        _onSelectItemDataEvent.OnEventRaised += OnItemDataSelectedReceived;
    }

    void OnDisable()
    {
        _onSetupItemDatasEvent.OnEventRaised -= Initialize;
        _onSelectItemDataEvent.OnEventRaised -= OnItemDataSelectedReceived;
    }


    public void Initialize(List<ItemDataSO> itemDatas)
    {
        RemoveAllItemSelectingEntry();
        _selectedItem = new();
        int loopCount = Mathf.Min(itemDatas.Count, _uIItemSelectedEntries.Count);
        for (int i = 0; i < loopCount; i++)
        {
            ItemDataSO currentItemData = itemDatas[i];
            UIItemSelectedEntry currentUIItemSelectedEntry = _uIItemSelectedEntries[i];

            currentUIItemSelectedEntry.Initialize(currentItemData);
        }
    }

    private void OnItemDataSelectedReceived(ItemDataSO itemDataSO)
    {
        if (_selectedItem.Count >= _totalSelectedItem)
        {
            return;
        }

        if (_selectedItem.Count < _uIItemSelectingEntries.Count)
        {
            var itemSelectingSlot = _uIItemSelectingEntries[_selectedItem.Count];
            itemSelectingSlot.Initialize(itemDataSO);
            itemSelectingSlot.gameObject.SetActive(true);
        }

        _selectedItem.Add(itemDataSO);
        if (_selectedItem.Count >= _totalSelectedItem)
        {
            _onAllRequiredItemSelected.RaiseEvent(_selectedItem);
        }
    }

    private void RemoveAllItemSelectingEntry()
    {
        foreach (var item in _uIItemSelectingEntries)
        {
            item.gameObject.SetActive(false);
        }
    }
}
