using UnityEngine;

public class MineVisual : MonoBehaviour
{
    [SerializeField] private Mine mine;
    [SerializeField] private Canvas canvas;
    [SerializeField] private Transform uiPanel;

    private void Awake()
    {
        canvas.worldCamera = Camera.main;
    }

    private void Start()
    {
        mine.OnObjectClicked += Mine_OnObjectClicked;
    }

    private void Mine_OnObjectClicked(ItemSO storedItem, int amount)
    {
        Debug.Log("StoredItem: " + storedItem + " " + amount);
        canvas.gameObject.SetActive(true);
    }
}
