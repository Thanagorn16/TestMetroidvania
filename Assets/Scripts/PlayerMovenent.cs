using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovenent : MonoBehaviour
{
    Vector2 moveInput;
    Rigidbody2D rbd2;
    Animator myAnimator;
    CapsuleCollider2D myCapsuleCollider;

    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float jumpSpeed = 5f;

    void Start()
    {
        rbd2 = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myCapsuleCollider = GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Run();
        FlipSprite();
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
        // Debug.Log(moveInput);
    }

    void Run()
    {
        // Vector2 playerVelocity = new Vector2(moveInput.x * moveSpeed, 0f);
        // Vector2 playerVelocity = new Vector2(moveInput.x, 0f) * moveSpeed * Time.deltaTime;
        Vector2 playerVelocity = new Vector2(moveInput.x * moveSpeed, rbd2.velocity.y);
        rbd2.velocity = playerVelocity;

        // animate running
        bool playerHasHorizontalSpeed = Mathf.Abs(rbd2.velocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("isRunning", playerHasHorizontalSpeed);
    }


    void OnJump(InputValue value)
    {
        if(!myCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
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
        // Debug.Log("velocity: " + rbd2.velocity.x);
        // Debug.Log("math--: " + Mathf.Abs(rbd2.velocity.x));
        // Debug.Log(playerHasHorizontalSpeed);
        // Debug.Log("Epsilon: " + Mathf.Epsilon);

        if(playerHasHorizontalSpeed)
        {
            // we use scale to do flip so if the condition above is true it will do flip
            // the condition will turn true when the character moves 
            // if the character does not move --> false. Therefore, no flip.
            transform.localScale = new Vector2(Mathf.Sign(rbd2.velocity.x), 1f);
            // Debug.Log("transform: " + transform.localScale);
        }
    }
}
