using Utils;
using UnityEngine;
using System.Collections.Generic;

public class GridXZ : MonoBehaviour, IGrid
{
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private float cellSize;
    [SerializeField] private Vector3 origin;

    [SerializeField] private GameObject nodePrefab;
    [SerializeField] private Transform nodeContainer;

    [SerializeField] private bool debug;

    private Node[,] gridArray;

    private TextMesh[,] gridDebugArray;

    public int Width { get => width; }
    public int Height { get => height; }
    public float CellSize { get => cellSize; }

    private void Awake()
    {
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        gridArray = new Node[width, height];
        gridDebugArray = new TextMesh[width, height];

        for (int x = 0; x < Width; x++)
        {
            for (int z = 0; z < Height; z++)
            {
                GridIndex gridIndex = new GridIndex(x, 0, z);

                GameObject spawnedNodeObject = Instantiate(nodePrefab, GetNodeCenter(new GridIndex(x, 0, z)), Quaternion.identity, nodeContainer);

                Node spawnedNode = spawnedNodeObject.GetComponent<Node>();
                SetNode(spawnedNode, gridIndex);

                #region Debugging

                if (debug)
                {
                    gridDebugArray[x, z] = Utility.CreateWorldText(
                        gridArray[x, z].ToString(),
                        position: GetNodeOrigin(new GridIndex(x, 0, z)) + GetOffsetBetweenNodeOriginAndCenterMath(),
                        rotation: Quaternion.Euler(90, 0, 0),
                        localScale: new Vector3(0.15f, 0.15f, 0.15f),
                        color: Color.white,
                        fontSize: 10
                    );

                    Debug.DrawLine(GetNodeOriginDebug(new GridIndex(x, 0, z)), GetNodeOriginDebug(new GridIndex(x, 0, z + 1)), Color.white, 1000f);
                    Debug.DrawLine(GetNodeOriginDebug(new GridIndex(x, 0, z)), GetNodeOriginDebug(new GridIndex(x + 1, 0, z)), Color.white, 1000f);
                }

                #endregion
            }
        }

        #region Debugging

        if (debug)
        {
            Debug.DrawLine(GetNodeOriginDebug(new GridIndex(0, 0, height)), GetNodeOriginDebug(new GridIndex(width, 0, height)), Color.white, 1000f);
            Debug.DrawLine(GetNodeOriginDebug(new GridIndex(width, 0, 0)), GetNodeOriginDebug(new GridIndex(width, 0, height)), Color.white, 1000f);
        }

        #endregion
    }

    private Vector3 GetNodeOriginDebug(GridIndex gridIndex)
    {
        return gridIndex.ToVector3() * cellSize + origin;
    }

    public Vector3 GetNodeOrigin(Node node)
    {
        return GetNodeOrigin(node.GridIndex);
    }

    public Vector3 GetNodeOrigin(Vector3 nodePosition)
    {
        GridIndex gridIndex = GetGridIndex(nodePosition);
        return GetNodeOrigin(gridIndex);
    }

    public Vector3 GetNodeOrigin(GridIndex gridIndex)
    {
        if (!IsValidCoordinate(gridIndex))
        {
            GridIndex clampedGridIndex = ClampToGrid(gridIndex);
            return GetNodeOrigin(clampedGridIndex);
        }

        return gridIndex.ToVector3() * cellSize + origin;
    }

    private GridIndex ClampToGrid(GridIndex gridIndex)
    {
        gridIndex.X = (gridIndex.X < 0) ? 0 : gridIndex.X;
        gridIndex.X = (gridIndex.X >= width) ? width - 1 : gridIndex.X;
        gridIndex.Z = (gridIndex.Z < 0) ? 0 : gridIndex.Z;
        gridIndex.Z = (gridIndex.Z >= height) ? height - 1 : gridIndex.Z;

        return gridIndex;
    }

    public Vector3 GetNodeCenter(GridIndex gridIndex)
    {
        if (!IsValidCoordinate(gridIndex))
        {
            gridIndex = ClampToGrid(gridIndex);
        }

        Vector3 nodeOrigin = GetNodeOrigin(gridIndex);
        Vector3 offsetBetweenNodeOriginAndCenter = GetOffsetBetweenNodeOriginAndCenterMath();
        Vector3 nodeCenter = nodeOrigin + offsetBetweenNodeOriginAndCenter;

        return nodeCenter;
    }

    public Vector3 GetNodeCenter(Node node)
    {
        return GetNodeCenter(node.GridIndex);
    }

    public Vector3 GetNodeCenter(Vector3 nodePosition)
    {
        GridIndex gridIndex = GetGridIndex(nodePosition);
        return GetNodeCenter(gridIndex);
    }

    private Vector3 GetOffsetBetweenNodeOriginAndCenterMath()
    {
        return new Vector3(cellSize / 2, 0, cellSize / 2);
    }

    public void SetNode(Node node, Vector3 nodePosition)
    {
        GridIndex gridIndexAtPosition = GetGridIndex(nodePosition);
        SetNode(node, gridIndexAtPosition);
    }

    private void SetNode(Node node, GridIndex gridIndex)
    {
        if (!IsValidCoordinate(gridIndex)) return;

        node.Initialize(this, gridIndex);

        gridArray[gridIndex.X, gridIndex.Z] = node;
    }

    public GridIndex GetGridIndex(Vector3 nodeWorldPosition)
    {
        int x = Mathf.FloorToInt((nodeWorldPosition - origin).x / cellSize);
        int z = Mathf.FloorToInt((nodeWorldPosition - origin).z / cellSize);

        GridIndex gridIndex = new GridIndex(x, 0, z);

        return IsValidCoordinate(gridIndex) ? gridIndex : ClampToGrid(gridIndex);
    }

    public Node GetNode(Vector3 cellWorldPosition)
    {
        GridIndex gridIndex = GetGridIndex(cellWorldPosition);
        return IsValidCoordinate(gridIndex) ? gridArray[gridIndex.X, gridIndex.Z] : default;
    }

    private bool IsValidCoordinate(GridIndex gridIndex)
    {
        return gridIndex.X >= 0 && gridIndex.Z >= 0 && gridIndex.X < width && gridIndex.Z < height;
    }

    public List<Node> GetRowAt(int z)
    {
        List<Node> rowNodes = new List<Node>();

        for (int x = 0; x < width; x++)
        {
            Node node = gridArray[x, z];
            rowNodes.Add(node);
        }

        return rowNodes;
    }

    public List<Node> GetColAt(int x)
    {
        List<Node> columnNodes = new List<Node>();

        for (int z = 0; z < height; z++)
        {
            Node node = gridArray[x, z];
            columnNodes.Add(node);
        }

        return columnNodes;
    }

    public Node GetNeighbourNode(Vector3 position, Vector3 direction)
    {
        Vector3 neighbourNodePosition = (position + direction) * cellSize;
        return GetNode(neighbourNodePosition);
    }

    public List<Node> GetNeighbours(Node node, bool includeCorners)
    {
        List<Node> neighbours = new List<Node>();

        neighbours.Add(gridArray[node.GridIndex.X, node.GridIndex.Z + 1]);
        neighbours.Add(gridArray[node.GridIndex.X + 1, node.GridIndex.Z]);
        neighbours.Add(gridArray[node.GridIndex.X, node.GridIndex.Z - 1]);
        neighbours.Add(gridArray[node.GridIndex.X - 1, node.GridIndex.Z]);

        if (includeCorners)
        {
            neighbours.Add(gridArray[node.GridIndex.X + 1, node.GridIndex.Z + 1]);
            neighbours.Add(gridArray[node.GridIndex.X + 1, node.GridIndex.Z - 1]);
            neighbours.Add(gridArray[node.GridIndex.X - 1, node.GridIndex.Z - 1]);
            neighbours.Add(gridArray[node.GridIndex.X - 1, node.GridIndex.Z + 1]);
        }

        return neighbours;
    }
}
