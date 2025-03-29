using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Mover : MonoBehaviour
{
    [SerializeField] float runSpeed = 4f;

    Vector2 moveInput;
    Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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
