using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovenent : MonoBehaviour
{
    Vector2 moveInput;
    Rigidbody2D rbd2;
    Animator myAnimator;
    CapsuleCollider2D myBodyCollider;
    BoxCollider2D myFeetCollider;
    PlayerInput myPlayerInput;
    [SerializeField] GameObject playerBullet;
    [SerializeField] Transform playerGun;
    
    

    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float climbSpeed = 5f;
    [SerializeField] Vector2 deathKick = new Vector2(10f, 10f);
    // bool isAlive = true;
    float gravityScaleAtStart;
    void Start()
    {
        rbd2 = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myFeetCollider = GetComponent<BoxCollider2D>();
        myPlayerInput = GetComponent<PlayerInput>();

        gravityScaleAtStart = rbd2.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        Run();
        FlipSprite();
        ClimbLadder();
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
        // Debug.Log(moveInput);
    }

    void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * moveSpeed, rbd2.velocity.y);
        rbd2.velocity = playerVelocity;

        // animate running
        bool playerHasHorizontalSpeed = Mathf.Abs(rbd2.velocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("isRunning", playerHasHorizontalSpeed);
    }

    void OnFire(InputValue value)
    {
        Instantiate(playerBullet, playerGun.position, Quaternion.identity);
    }

    void OnJump(InputValue value)
    {
        if(!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            // this means stop executing this method
            return;
        }
        
        else if(value.isPressed) // could be just 'else' or 'if'
        {
            rbd2.velocity += new Vector2(0f, jumpSpeed);
        }
    }

    void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(rbd2.velocity.x) > Mathf.Epsilon;

        if(playerHasHorizontalSpeed)
        {
            // we use scale to do flip so if the condition above is true it will do flip
            // the condition will turn true when the character moves 
            // if the character does not move --> false. Therefore, no flip.
            transform.localScale = new Vector2(Mathf.Sign(rbd2.velocity.x), 1f);
            // Debug.Log("transform: " + transform.localScale);
        }
    }

    void ClimbLadder()
    {
        // check if it is where to climb or  not
        if(!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            rbd2.gravityScale = gravityScaleAtStart;
            myAnimator.SetBool("isClimbing", false);
            myAnimator.enabled = true;
            return;
        }

        // make movement
        Vector2 climbVelocity = new Vector2(rbd2.velocity.x, moveInput.y * climbSpeed);
        rbd2.velocity = climbVelocity;

        // set gravity while climbing
        rbd2.gravityScale = 0f;

        // animate climbing
        bool playerHasVerticalSpeed = Mathf.Abs(rbd2.velocity.y * climbSpeed) > Mathf.Epsilon;
        myAnimator.SetBool("isClimbing", playerHasVerticalSpeed);
        // to freeze animation if no velocity during the climbing
        myAnimator.enabled = playerHasVerticalSpeed;
    }


    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Enemy" || other.gameObject.tag == "Hazard")
        {
            Die();
        }
    }
    void Die()
    {
        // InputSystem.DisableAllEnabledActions();

        myPlayerInput.enabled = false; //disable player's input

        myAnimator.SetTrigger("Dying"); //play dead animation 

        rbd2.velocity = deathKick;

        // get rid off the player
        myBodyCollider.enabled = false;
        myFeetCollider.enabled = false;
        // gameObject.layer = 11;
    }
}

