using UnityEngine;

public class Mover : MonoBehaviour
{

    [SerializeField] float walkSpeed = 1.0f;
    float moveInput;
    [SerializeField] float jumpSpeed = 1.0f;
    float jumpInput;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        moveInput = Input.GetAxis("Horizontal") * walkSpeed * Time.deltaTime;
        jumpInput = Input.GetAxis("Vertical") * walkSpeed * Time.deltaTime;
        transform.Translate(moveInput, 0, 0);
        transform.Translate(0, jumpInput, 0);

        
    }
}
