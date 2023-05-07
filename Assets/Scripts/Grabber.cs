using System.Data.Common;
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

    private IItemContainer grabContainer;
    private IItemContainer dropContainer;

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
                if (grabbingNode.GetObject() == null)
                {
                    Debug.Log("No object found in grabbingNode!");
                }

                grabContainer = grabbingNode.GetObject().GetComponent<IItemContainer>();

                if (grabContainer == null)
                {
                    Debug.Log("No grabbable object found in grabbingNode!");
                }

                if (grabContainer.HasItem())
                {
                    ItemSO storedItemSO = grabContainer.GetStoredItemSO();
                    GameObject grabbedItemObject = Spawner.Spawn(storedItemSO.Prefab, grabContainer.Position, Quaternion.identity);
                    grabbedItem = grabbedItemObject.GetComponent<Item>();
                    grabbedItem.ItemSO = storedItemSO;
                    state = State.MovingItemToGrabber;
                }

                break;
            case State.MovingItemToGrabber:
                if (grabbedItem.IsMoving()) return;

                grabbedItem.MoveTo(transform.position, onArrived: () =>
                {
                    state = State.MovingToDropItem;
                });

                break;
            case State.MovingToDropItem:
                if (droppingNode.GetObject() == null)
                {
                    Debug.Log("No object found in droppingNode!");
                }

                dropContainer = droppingNode.GetObject().GetComponent<IItemContainer>();

                if (dropContainer == null)
                {
                    Debug.Log("No droppable object found in droppingNode!");
                }

                if (grabbedItem.IsMoving()) return;

                grabbedItem.MoveTo(dropContainer.Position, onArrived: () =>
                {
                    state = State.DroppingItem;
                });

                break;
            case State.DroppingItem:
                dropContainer.Drop(grabbedItem);
                grabbedItem.DestroySelf();
                grabbedItem = null;
                state = State.Cooldown;
                break;
        }
    }
}
