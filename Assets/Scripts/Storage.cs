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

    public bool TryGetStoredItemSO(out ItemSO itemSO)
    {
        throw new System.NotImplementedException();
    }
}
