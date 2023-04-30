using UnityEngine;

public class Direction
{
    public enum DirectionType
    {
        Forward,
        Right,
        Back,
        Left,
    }

    public DirectionType Type { get; }
    public DirectionType DirectionAfter90Degree { get; }
    public DirectionType OppositeDirection { get; }
    public Vector3 DirectionVector { get; }
    public Vector3 OppositeDirectionVector { get; }

    public static Direction Forward { get; } = new Direction(DirectionType.Forward, DirectionType.Right, DirectionType.Back, Vector3.forward, Vector3.back);
    public static Direction Right { get; } = new Direction(DirectionType.Right, DirectionType.Back, DirectionType.Left, Vector3.right, Vector3.left);
    public static Direction Back { get; } = new Direction(DirectionType.Back, DirectionType.Left, DirectionType.Forward, Vector3.back, Vector3.forward);
    public static Direction Left { get; } = new Direction(DirectionType.Left, DirectionType.Forward, DirectionType.Right, Vector3.left, Vector3.right);

    public Direction(
        DirectionType type, 
        DirectionType directionAfter90Degree, 
        DirectionType oppositeDirection,
        Vector3 directionVector, 
        Vector3 oppositeDirectionVector)
    {
        Type = type;
        DirectionAfter90Degree = directionAfter90Degree;
        OppositeDirection = oppositeDirection;
        DirectionVector = directionVector;
        OppositeDirectionVector = oppositeDirectionVector;
    }

    public static Direction GetDirectionWithType(DirectionType direction)
    {
        Direction directionWithType = null;

        switch (direction)
        {
            case DirectionType.Forward: directionWithType = Forward; break;
            case DirectionType.Right: directionWithType = Right; break;
            case DirectionType.Back: directionWithType = Back; break;
            case DirectionType.Left: directionWithType = Left; break;
        }

        if (directionWithType == null)
        {
            Debug.LogError("Not all directions are present in the above switch-case!");
        }

        return directionWithType;
    }
}
