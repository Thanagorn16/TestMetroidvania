using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;
    Rigidbody2D rbd2;

    void Start()
    {
        rbd2 = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        rbd2.velocity = new Vector2(moveSpeed, 0f);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        moveSpeed = -moveSpeed;
        // // Debug.Log(moveSpeed);
        FlipEnemyFacing();
    }

    void FlipEnemyFacing()
    {
        transform.localScale = new Vector2(Mathf.Sign(-(rbd2.velocity.x)), 1f);
    }
}
