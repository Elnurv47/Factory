using Utils;
using UnityEngine;
using UnityEngine.EventSystems;

public class GridBuildingSystem : MonoBehaviour
{
    public static GridBuildingSystem Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private const float HALF_OF_SCALE_MULTIPLIER = 0.5f;
    private PlaceableObject selectedPlaceableObjectPrefab;
    private PlaceableObject selectedPlaceableObject;

    [SerializeField] private GridXZ grid;
    public GridXZ Grid { get => grid; }

    [SerializeField] private BuildingButton[] buildingButtons;
    [SerializeField] private LayerMask groundLayerMask;

    private void Start()
    {
        foreach (var buildingButton in buildingButtons)
        {
            buildingButton.SubscribeToOnClick(() =>
            {
                SetSelectedBuildingPrefab(buildingButton.BuildingPrefab);
                selectedPlaceableObject = Spawner.Spawn(selectedPlaceableObjectPrefab, Utility.GetMouseWorldPosition3D(groundLayerMask), Quaternion.identity);
            });
        }
    }

    private void Update()
    {
        if (selectedPlaceableObject == null) return;

        selectedPlaceableObject.transform.position =
            Utility.GetMouseWorldPosition3D(groundLayerMask) + GetCenterXZOffsetCausedByScale(selectedPlaceableObject);

        HandlePlacingOnClick();

        if (Input.GetMouseButtonDown(1))
        {
            selectedPlaceableObject.Apply90RotationAntiClockwise();
        }
    }

    private void HandlePlacingOnClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;
            Node clickedNode = GetClickedNode();

            if (!clickedNode.IsAvailable) return;

            Vector3 clickedNodeOrigin = grid.GetNodeOrigin(clickedNode);
            selectedPlaceableObject.transform.position =
                clickedNodeOrigin + GetCenterXZOffsetCausedByScale(selectedPlaceableObject);
            clickedNode.SetObject(selectedPlaceableObject.gameObject);
            selectedPlaceableObject.InvokeOnPlaced(clickedNode);
            selectedPlaceableObject = null;
        }
    }

    private Node GetClickedNode()
    {
        Vector3 clickPosition = Utility.GetMouseWorldPosition3D(groundLayerMask);
        Node clickedNode = grid.GetNode(clickPosition);
        return clickedNode;
    }

    // Change this code so that the building system uses object's pivot to arrange it.
    // The object's pivot need to be adjusted beforehand

    private Vector3 GetCenterXZOffsetCausedByScale(PlaceableObject placedObject)
    {
        return new Vector3(placedObject.LocalScale.x, 0, placedObject.LocalScale.z) * HALF_OF_SCALE_MULTIPLIER;
    }

    public void SetSelectedBuildingPrefab(PlaceableObject selectedPlaceableObjectPrefab)
    {
        this.selectedPlaceableObjectPrefab = selectedPlaceableObjectPrefab;
    }

    public float GetGridCellSize()
    {
        return grid.CellSize;
    }
}
