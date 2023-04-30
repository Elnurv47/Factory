using UnityEngine;

public class Grabber : PlaceableObject
{
    private const int COOLDOWN_TIME = 3;

    private enum State
    {
        NotPlaced,
        Cooldown,
        GrabbingItem,
        MovingItemToGrabber,
        MovingToDropItem,
        DroppingItem
    }

    private State state;
    private float timer = 0;

    private Node grabbingNode;
    private Node droppingNode;

    private Item grabbedItem;

    private IItemContainer grabStorage;
    private ConveyorBelt dropConveyorBelt;

    public override void InvokeOnPlaced(Node placedOnNode)
    {
        base.InvokeOnPlaced(placedOnNode);

        Vector3 facingDirection = Direction.DirectionVector;
        Vector3 facingDirectionReverse = Direction.OppositeDirectionVector;

        grabbingNode = GridBuildingSystem.Instance.Grid.GetNeighbourNode(transform.position, facingDirectionReverse);
        droppingNode = GridBuildingSystem.Instance.Grid.GetNeighbourNode(transform.position, facingDirection);

        state = State.Cooldown;
    }

    private void Update()
    {
        switch (state)
        {
            case State.NotPlaced:
                break;
            case State.Cooldown:
                timer += Time.deltaTime;
                if (timer >= COOLDOWN_TIME)
                {
                    timer = 0;
                    state = State.GrabbingItem;
                }

                break;
            case State.GrabbingItem:

                // Check GetObject() before GetComponent() call
                grabStorage = grabbingNode.GetObject().GetComponent<IItemContainer>();

                if (grabStorage != null)
                {
                    if (grabStorage.TryGetStoredItemSO(out ItemSO storedItemSO))
                    {
                        GameObject grabbedItemObject = Spawner.Spawn(storedItemSO.Prefab, grabStorage.Position, Quaternion.identity);
                        grabbedItem = grabbedItemObject.GetComponent<Item>();
                        grabbedItem.ItemSO = storedItemSO;
                        state = State.MovingItemToGrabber;
                    }
                }

                break;
            case State.MovingItemToGrabber:

                grabbedItem.MoveTo(transform.position, onArrived: () =>
                {
                    state = State.MovingToDropItem;
                });

                break;
            case State.MovingToDropItem:

                dropConveyorBelt = droppingNode.GetObject().GetComponent<ConveyorBelt>();

                if (dropConveyorBelt == null || !dropConveyorBelt.IsAvailable()) return;
                if (grabbedItem.IsMoving()) return;

                grabbedItem.MoveTo(dropConveyorBelt.transform.position, onArrived: () =>
                {
                    state = State.DroppingItem;
                });

                break;
            case State.DroppingItem:
                dropConveyorBelt.Drop(grabbedItem);
                grabbedItem.DestroySelf();
                grabbedItem = null;
                state = State.Cooldown;
                break;
        }
    }
}
