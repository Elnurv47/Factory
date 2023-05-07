using System;
using UnityEngine;

public class ConveyorBelt : PlaceableObject, IItemContainer
{
    private Item holdItem;

    private Node facingNode;
    private ConveyorBelt facingConveyorBelt;

    private enum State
    {
        NotPlaced,
        TryMoveNextConveyorBelt,
        MovingItem,
        Moved,
    }

    private State state;

    public Vector3 Position => transform.position;

    public override void Awake()
    {
        base.Awake();
        state = State.NotPlaced;
    }

    public override void InvokeOnPlaced(Node placedOnNode)
    {
        base.InvokeOnPlaced(placedOnNode);

        Vector3 facingDirection = Direction.DirectionVector;
        facingNode = GridBuildingSystem.Instance.Grid.GetNeighbourNode(transform.position, facingDirection);
    }

    private void Update()
    {
        switch (state)
        {
            case State.NotPlaced:
                break;
            case State.TryMoveNextConveyorBelt:
                GameObject facingNodeObject = facingNode.GetObject();
                if (facingNodeObject == null) return;

                facingConveyorBelt = facingNodeObject.GetComponent<ConveyorBelt>();
                if (Direction.Type == Direction.OppositeDirection) return;

                if (facingConveyorBelt == null || facingConveyorBelt.HasItem()) return;

                if (holdItem == null) return;

                state = State.MovingItem;
                break;
            case State.MovingItem:

                if (holdItem.IsMoving()) return;

                holdItem.MoveTo(facingConveyorBelt.transform.position, onArrived: () =>
                {
                    state = State.Moved;
                });

                break;
            case State.Moved:
                facingConveyorBelt.Drop(holdItem);
                holdItem.DestroySelf();
                holdItem = null;
                state = State.TryMoveNextConveyorBelt;
                break;
        }
    }

    public void Drop(Item droppedItem)
    {
        holdItem = droppedItem.Copy();
    }

    public bool HasItem()
    {
        return holdItem != null;
    }

    public ItemSO GetStoredItemSO()
    {
        ItemSO itemSO = holdItem.ItemSO;
        holdItem.DestroySelf();
        return itemSO;
    }
}
