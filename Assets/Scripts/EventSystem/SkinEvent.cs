using UnityEngine.Events;
using UnityEngine;

public class SkinEvent
{
    private readonly static UnityEvent _onNewSkinEquip = new UnityEvent();
    public static UnityEvent OnNewSkinEquip { get => _onNewSkinEquip; }
}
