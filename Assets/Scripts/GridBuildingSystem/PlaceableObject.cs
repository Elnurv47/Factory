using System;
using UnityEngine;

public class PlaceableObject : MonoBehaviour
{
    public Direction Direction { get; private set; }
    public Vector3 LocalScale { get => transform.localScale; }

    private const int ROTATIONDEGREE = 90;

    protected Node placedOnNode;

    [SerializeField] private int xSize;
    [SerializeField] private int ySize;
    [SerializeField] private Transform visualTransform;

    public Action<Direction.DirectionType> OnDirectionChanged;
    public Action<ItemSO, int> OnObjectClicked;

    public virtual void Awake()
    {
        Direction = Direction.Forward;
    }

    public void Apply90RotationAntiClockwise()
    {
        Direction = Direction.GetDirectionWithType(Direction.DirectionAfter90Degree);
        visualTransform.Rotate(new Vector3(0, ROTATIONDEGREE, 0));
        OnDirectionChanged?.Invoke(Direction.Type);
    }

    public virtual void InvokeOnPlaced(Node placedOnNode)
    {
        this.placedOnNode = placedOnNode;
    }
}
