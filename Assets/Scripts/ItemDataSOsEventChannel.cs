using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Pachinko/ItemDataSOsEventChannel")]
public class ItemDataSOsEventChannel : ScriptableObject
{
    public System.Action<List<ItemDataSO>> OnEventRaised;
    public void RaiseEvent(List<ItemDataSO> itemDataSO)
    {
        OnEventRaised?.Invoke(itemDataSO);
    }
}