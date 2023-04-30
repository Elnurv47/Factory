using UnityEngine;
using System.Collections;

public class Mine : PlaceableObject, IItemContainer
{
    private const int PRODUCTION_INTERVAL = 1;

    private int storedItemAmount;

    public Vector3 Position { get => transform.position; }

    [SerializeField] private ItemSO itemSO;

    public override void InvokeOnPlaced(Node placedOnNode)
    {
        base.InvokeOnPlaced(placedOnNode);
        StartCoroutine(ProduceMaterialCoroutine());
    }

    public IEnumerator ProduceMaterialCoroutine()
    {
        while (true)
        {
            storedItemAmount++;
            yield return new WaitForSeconds(PRODUCTION_INTERVAL);
        }
    }

    public bool TryGetStoredItemSO(out ItemSO itemSO)
    {
        if (storedItemAmount > 0)
        {
            itemSO = this.itemSO;
            return true;
        }

        itemSO = null;
        return false;
    }

    public void Drop(Item droppedItem)
    {
        throw new System.NotImplementedException();
    }

    public bool IsAvailable()
    {
        throw new System.NotImplementedException();
    }
}
