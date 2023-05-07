using UnityEngine;

public class Storage : PlaceableObject, IItemContainer
{
    public Vector3 Position => transform.position;

    private int storedItemAmount;
    private ItemSO storedItemSO;

    public override void InvokeOnPlaced(Node placedOnNode)
    {
        base.InvokeOnPlaced(placedOnNode);
    }

    public void Drop(Item droppedItem)
    {
        storedItemSO = droppedItem.ItemSO;
        storedItemAmount++;
    }

    public bool HasItem()
    {
        return storedItemSO != null;
    }

    public ItemSO GetStoredItemSO()
    {
        storedItemAmount--;
        return storedItemSO;
    }
}
