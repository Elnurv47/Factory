using UnityEngine;

public class ConveyorBeltVisual : MonoBehaviour
{
    [SerializeField] private TextMesh directionText;
    [SerializeField] private ConveyorBelt conveyorBelt;

    private void Start()
    {
        conveyorBelt.OnDirectionChanged += ConveyorBelt_OnDirectionChanged;
    }

    private void ConveyorBelt_OnDirectionChanged(Direction.DirectionType direction)
    {
        directionText.text = direction.ToString();
    }
}
