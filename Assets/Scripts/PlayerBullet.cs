using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    Rigidbody2D rbd2;
    PlayerMovenent player;

    [SerializeField] float bulletSpeed = 5f;
    float xSpeed;
    void Start()
    {
       rbd2 = GetComponent<Rigidbody2D>(); 
       player = FindObjectOfType<PlayerMovenent>();
       xSpeed = player.transform.localScale.x * bulletSpeed;
    }

    void Update()
    {
       rbd2.velocity = new Vector2(xSpeed,0f);
    }

    // void OnTriggerEnter2D(Collider2D other)
    // {
    //     if(other.tag == "Enemy")   
    //     {
    //         Destroy(other.gameObject);
    //         // Destroy(this.gameObject);
    //     }
    //     // Destroy(gameObject);
    // }

    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            Destroy(other.gameObject);
        }
        Destroy(gameObject);
    }
}
