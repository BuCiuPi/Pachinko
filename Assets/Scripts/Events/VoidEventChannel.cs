using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Pachinko/VoidEventChannel")]
public class VoidEventChannel : ScriptableObject
{
    public Action OnEventRaised;
    public void RaiseEvent()
    {
        OnEventRaised?.Invoke();
    }
}