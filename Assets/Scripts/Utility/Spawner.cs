using UnityEngine;

public class Spawner : MonoBehaviour
{
    public static T Spawn<T>(T itemPrefab, Vector3 position, Quaternion rotation, Transform parent = null) where T : Object
    {
        return Instantiate(itemPrefab, position, rotation, parent);
    }
}
