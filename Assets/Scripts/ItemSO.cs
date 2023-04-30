using UnityEngine;

[CreateAssetMenu()]
public class ItemSO : ScriptableObject
{
    public enum ItemType
    {
        Iron,
    }

    [SerializeField] private int id;
    public int Id { get => id; }

    [SerializeField] private GameObject prefab;
    public GameObject Prefab { get => prefab; }

    [SerializeField] private ItemType type;
    public ItemType Type { get => type; }
}
