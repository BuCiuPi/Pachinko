using UnityEngine;
using UnityEngine.UI;

public class UIItemEntry : MonoBehaviour
{
    [SerializeField] protected Image _itemImage;
    protected ItemDataSO _currentItemData;

    public virtual void Initialize(ItemDataSO itemDataSO)
    {
        this._currentItemData = itemDataSO;
        _itemImage.sprite = itemDataSO.PreviewIcon;
    }
}
