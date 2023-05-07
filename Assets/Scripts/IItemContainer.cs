using UnityEngine;

public interface IItemContainer
{
    Vector3 Position { get; }
    void Drop(Item droppedItem);
    bool HasItem();

    ItemSO GetStoredItemSO();
}
