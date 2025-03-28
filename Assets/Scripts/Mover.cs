using UnityEngine;

public class Mover : MonoBehaviour
{

    public float walkSpeed;
    private float moveInput;
    public bool isGrounded;
    private Rigidbody2D rb;
    public bool canJump = true;
    public float jumpSpeed = 0.0f;
    public LayerMask groundMask;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");

        rb.linearVelocity = new Vector2(moveInput*walkSpeed, rb.linearVelocity.y);

        isGrounded = Physics2D.OverlapBox(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - 0.5f),
        new Vector2(0.9f, 0.4f), 0f, groundMask);

    if(Input.GetKey("space") && isGrounded && canJump){

    }
        
    }
}
