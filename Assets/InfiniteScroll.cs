using System;
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

    [SerializeField] private RectTransform _itemPrefab;

    private bool _isPositiveDrag;
    private bool _isDecreaseVelocity;

    private Transform _targetSlowChildTransform;
    private Vector3 _targetOriginPosition;

    void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        _scrollRect.movementType = ScrollRect.MovementType.Unrestricted;
        _scrollRect.onValueChanged.AddListener(OnViewScroll);
    }

    void Update()
    {


        if (Input.GetKeyDown(KeyCode.Space))
        {
            _isDecreaseVelocity = true;
            
            for (int i = 0; i < 3; i++)
            {
                int lastItemIndex = _isPositiveDrag ? 0 : _scrollRect.content.childCount - 1;
                var curItem = _scrollRect.content.GetChild(lastItemIndex).GetComponent<RectTransform>();
                var newItem = CreateNewItemAtLast(curItem);

                if (i == 1)
                {
                    _targetSlowChildTransform = newItem;
                    _targetOriginPosition = _targetSlowChildTransform.position;
                }
            }
            // Debug.Log(_targetSlowChildTransform.name);
            
            // for (int i = 0; i < _scrollRect.content.childCount; i++)
            // {
            //     Debug.Log($"Pool {_scrollRect.content.GetChild(i)}");
            // }
        }

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
            newPos.y = lastItem.position.y - (curItem.rect.height * 1.5f) + _itemSpacing;
        }
        else
        {
            newPos.y = lastItem.position.y + (curItem.rect.height * 1.5f) - _itemSpacing;
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

    private Transform CreateNewItemAtLast(Transform currentItem)
    {
        RectTransform newItem = Instantiate(_itemPrefab);
        Vector2 newPos = currentItem.position;
        newPos.y = currentItem.position.y + (newItem.rect.height * 1.5f) - _itemSpacing;

        newItem.SetParent(_scrollRect.content);
        newItem.position = newPos;
        newItem.GetComponent<Image>().color = Color.red;
        newItem.SetAsLastSibling();

        return newItem;
    }
}
