using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Mover : MonoBehaviour
{
    [SerializeField] float runSpeed = 4f;
    [SerializeField] float jumpSpeed = 5f;

    Vector2 moveInput;
    Rigidbody2D rb;
    CapsuleCollider2D myBodyCollider;
    BoxCollider2D myFeetCollider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myFeetCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {

        Run();
        FlipSprite();
        
    }

    void OnMove(InputValue value){

        moveInput = value.Get<Vector2>();

    }

    void OnJump(InputValue value){

        if(!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))){return;}
        if(value.isPressed){

            rb.linearVelocity += new Vector2 (0f, jumpSpeed);

        }
        
    }

    void Run(){
        Vector2 playerVelocity = new Vector2 (moveInput.x * runSpeed, rb.linearVelocity.y);
        rb.linearVelocity = playerVelocity;
    }

  void FlipSprite(){

        bool playerHasHorizSpeed = Mathf.Abs(rb.linearVelocity.x) > 0;

        if(playerHasHorizSpeed){
            transform.localScale = new Vector2 (Mathf.Sign(rb.linearVelocity.x), 1f);
        }

    } 

}
