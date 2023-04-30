using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class BuildingButton : MonoBehaviour
{
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    [SerializeField] private PlaceableObject placeableObject;

    public PlaceableObject BuildingPrefab { get => placeableObject; }

    public void SubscribeToOnClick(Action onClickAction)
    {
        UnityAction onClickUnityAction = new UnityAction(onClickAction);
        button.onClick.AddListener(onClickUnityAction);
    }
}
