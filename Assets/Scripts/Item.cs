using System;
using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour
{
    private ItemSO itemSO;
    public ItemSO ItemSO { get => itemSO; set => itemSO = value; }

    private float speed = 4f;

    private bool isMoving;

    public void MoveTo(Vector3 targetPosition, Action onArrived)
    {
        StartCoroutine(MoveToCoroutine(targetPosition, onArrived));
    }

    private IEnumerator MoveToCoroutine(Vector3 targetPosition, Action onArrived)
    {
        isMoving = true;

        while (transform.position != targetPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * speed);
            yield return null;
        }

        onArrived();
        isMoving = false;
    }

    public bool IsMoving()
    {
        return isMoving;
    }

    public Item Copy()
    {
        Item copy = Instantiate(this);
        copy.ItemSO = ItemSO;
        return copy;
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
