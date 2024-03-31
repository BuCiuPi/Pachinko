using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InfiniteScroll : MonoBehaviour
{
    [SerializeField] private ScrollRect _scrollRect;
    [SerializeField] private float _outOfBoundsThreshold;
    [SerializeField] private Vector2 _initScrollRectVelocity;

    [SerializeField] private float _itemSpacing;
    [SerializeField] private Transform _stopPosition;

    [SerializeField] private UIItemEntry _itemPrefab;

    private List<UIItemEntry> _allUIItemEntry = new();

    private bool _isPositiveDrag;
    private bool _isDecreaseVelocity;

    private Transform _targetSlowChildTransform;
    private Vector3 _targetOriginPosition;

    public void Initialize(List<ItemDataSO> items)
    {
        RemoveAllItem();
        ResetParam();
        _scrollRect.movementType = ScrollRect.MovementType.Unrestricted;
        _scrollRect.onValueChanged.AddListener(OnViewScroll);
        CreateItems(items);
    }

    private void CreateItems(List<ItemDataSO> items)
    {
        foreach (var item in items)
        {
            CreateNewItemAtLast(_scrollRect.content.position, item);
        }
    }

    void Update()
    {
        if (_isDecreaseVelocity)
        {
            float targetOriginDistance = _targetOriginPosition.y - _stopPosition.position.y;

            float targetCurrentDistance = _targetSlowChildTransform.position.y - _stopPosition.position.y;
            float velocityIntensity = Mathf.Abs(targetCurrentDistance) / targetOriginDistance;
            Debug.Log(_scrollRect.viewport.position + "|" + _targetSlowChildTransform.position);

            Vector2 scrollViewVelocity = new();
            if (targetCurrentDistance > 0)
            {
                scrollViewVelocity = Vector2.Lerp(Vector2.zero, _initScrollRectVelocity, velocityIntensity);
            }

            Debug.Log(_targetSlowChildTransform.name);
            _scrollRect.velocity = scrollViewVelocity;
        }
        else
        {
            _scrollRect.velocity = _initScrollRectVelocity;
        }
    }

    public void SetDecreaseSpeed(List<ItemDataSO> selectedItems, int stoppedIndex)
    {
        _isDecreaseVelocity = true;
        stoppedIndex = Mathf.Clamp(stoppedIndex, 0, selectedItems.Count - 1);

        for (int i = 0; i < selectedItems.Count; i++)
        {
            int lastItemIndex = _isPositiveDrag ? 0 : _scrollRect.content.childCount - 1;
            var curItem = _scrollRect.content.GetChild(lastItemIndex).GetComponent<RectTransform>();
            var newItem = CreateNewItemAtLast(curItem.position, selectedItems[i]);
            // newItem.GetComponent<Image>().color = Color.red;

            if (i == stoppedIndex)
            {
                _targetSlowChildTransform = newItem;
                _targetOriginPosition = _targetSlowChildTransform.position;
            }
        }
    }


    public void OnViewScroll(Vector2 value)
    {
        _isPositiveDrag = _scrollRect.velocity.y > 0;
        HandleVerticalScroll();
    }

    private void HandleVerticalScroll()
    {
        int curItemIndex = _isPositiveDrag ? _scrollRect.content.childCount - 1 : 0;
        var curItem = _scrollRect.content.GetChild(curItemIndex).GetComponent<RectTransform>();
        if (!ReachedThreshold(curItem))
        {
            return;
        }

        var lastItemIndex = _isPositiveDrag ? 0 : _scrollRect.content.childCount - 1;
        var lastItem = _scrollRect.content.GetChild(lastItemIndex);

        Vector2 newPos = lastItem.position;

        if (_isPositiveDrag)
        {
            newPos.y = lastItem.position.y - curItem.rect.height - _itemSpacing;
        }
        else
        {
            newPos.y = lastItem.position.y + curItem.rect.height + _itemSpacing;
        }

        curItem.position = newPos;
        curItem.SetSiblingIndex(lastItemIndex);
    }

    private bool ReachedThreshold(RectTransform curItem)
    {
        if (_isPositiveDrag)
        {
            return curItem.position.y - curItem.rect.height > transform.position.y + _outOfBoundsThreshold;
        }
        else
        {
            return curItem.position.y + curItem.rect.height < transform.position.y - _outOfBoundsThreshold;
        }
    }

    private Transform CreateNewItemAtLast(Vector2 currentPosition, ItemDataSO itemDataSO)
    {
        UIItemEntry newItem = Instantiate(_itemPrefab, _scrollRect.content);
        newItem.Initialize(itemDataSO);

        RectTransform newItemRectTransform = newItem.GetComponent<RectTransform>();
        Vector2 newPos = currentPosition;
        newPos.y = currentPosition.y + newItemRectTransform.rect.height + _itemSpacing;

        newItemRectTransform.position = newPos;
        newItemRectTransform.SetAsLastSibling();

        _allUIItemEntry.Add(newItem);

        return newItemRectTransform;
    }

    private void ResetParam()
    {
        _isDecreaseVelocity = false;
    }

    private void RemoveAllItem()
    {
        foreach (var item in _allUIItemEntry)
        {
            Destroy(item.gameObject);
        }
        _allUIItemEntry.Clear();
    }
}
