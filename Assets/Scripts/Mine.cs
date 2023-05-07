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

    public void Drop(Item droppedItem)
    {
        Debug.Log("Mine.Drop()");
    }

    public bool HasItem()
    {
        return storedItemAmount > 0;
    }

    public ItemSO GetStoredItemSO()
    {
        storedItemAmount--;
        return itemSO;
    }

    private void OnMouseDown()
    {
        OnObjectClicked?.Invoke(itemSO, storedItemAmount);
    }
}
