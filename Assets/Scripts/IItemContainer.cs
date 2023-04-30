using UnityEngine;

public interface IItemContainer
{
    Vector3 Position { get; }
    bool TryGetStoredItemSO(out ItemSO itemSO);
}
