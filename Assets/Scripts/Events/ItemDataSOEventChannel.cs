using UnityEngine;

[CreateAssetMenu(menuName = "Pachinko/ItemDataSOEventChannel")]
public class ItemDataSOEventChannel : ScriptableObject
{
    public System.Action<ItemDataSO> OnEventRaised;
    public void RaiseEvent(ItemDataSO itemDataSO)
    {
        OnEventRaised?.Invoke(itemDataSO);
    }
}