using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxBounder : MonoBehaviour
{
    public enum BlockedStatus 
    {
        NotBlocked = 0,
        Left,
        Right
    }

    public BlockedStatus Status;

    private void Start()
    {
        var rectTransform = gameObject.transform as RectTransform;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Status = transform.position.x < collision.collider.transform.position.x ? BlockedStatus.Right : BlockedStatus.Left;
        Debug.Log("Colliding with object on the " + Status.ToString());
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        Debug.Log("Collision Exit");
        Status = BlockedStatus.NotBlocked;
    }
}
