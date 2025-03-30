using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class Grapple : MonoBehaviour
{
    [Header("Grapple Settings")]
    [SerializeField] private float maxGrappleDistance = 15f;
    [SerializeField] private float swingForce = 5f;
    [SerializeField] private float ropeFlexibility = 0.9f;
    [SerializeField] private float ropeWidth = 0.2f;
    [SerializeField] private Color ropeColor = Color.red;
    [SerializeField] private float grappleSpeed = 20f;
    [SerializeField] private Transform grappleTip;

    private LineRenderer ropeLine;
    private DistanceJoint2D ropeJoint;
    private Rigidbody2D rb;
    private BoxCollider2D myFeetCollider;
    private bool isGrappling;
    private bool isExtending;
    private Vector2 grappleTarget;
    private float currentGrappleLength;
    private bool grappleUsedThisJump = false;
    private bool wasGroundedLastFrame;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        myFeetCollider = GetComponent<BoxCollider2D>();
        InitializeRopeRenderer();
        wasGroundedLastFrame = true; // Start assuming grounded
    }

    void Update()
    {
        bool isGrounded = myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));
        
        // Reset when landing (was airborne and now grounded)
        if (!wasGroundedLastFrame && isGrounded)
        {
            grappleUsedThisJump = false;
        }
        wasGroundedLastFrame = isGrounded;

        // Check for mouse release
        if (isGrappling && Mouse.current.leftButton.wasReleasedThisFrame)
        {
            StopGrapple();
        }

        if (isExtending)
        {
            UpdateGrappleExtension();
        }
        else if (isGrappling)
        {
            UpdateRopePosition();
        }
    }

    public void OnGrapple(InputValue value)
    {
        if (value.isPressed && !grappleUsedThisJump && !isGrappling && !isExtending)
        {
            grappleUsedThisJump = true; // Mark as used until next ground touch
            ShootUpward();
        }
    }

    private void InitializeRopeRenderer()
    {
        ropeLine = gameObject.AddComponent<LineRenderer>();
        ropeLine.startWidth = ropeWidth;
        ropeLine.endWidth = ropeWidth;
        ropeLine.positionCount = 2;
        ropeLine.material = new Material(Shader.Find("Sprites/Default"));
        ropeLine.startColor = ropeColor;
        ropeLine.endColor = ropeColor;
        ropeLine.enabled = false;
    }

    private void ShootUpward()
    {
        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            Vector2.up,
            maxGrappleDistance,
            LayerMask.GetMask("Ground")
        );

        if (hit.collider != null)
        {
            isExtending = true;
            grappleTarget = hit.point;
            currentGrappleLength = 0;
            ropeLine.enabled = true;
            ropeLine.SetPosition(0, transform.position);
            ropeLine.SetPosition(1, transform.position);

            if (grappleTip != null)
            {
                grappleTip.gameObject.SetActive(true);
                grappleTip.position = transform.position;
            }
        }
    }

    private void UpdateGrappleExtension()
    {
        currentGrappleLength += grappleSpeed * Time.deltaTime;
        float distanceToTarget = Vector2.Distance(transform.position, grappleTarget);

        if (currentGrappleLength >= distanceToTarget)
        {
            CompleteGrapple();
        }
        else
        {
            Vector2 currentEnd = Vector2.Lerp(transform.position, grappleTarget, currentGrappleLength/distanceToTarget);
            
            ropeLine.SetPosition(0, transform.position);
            ropeLine.SetPosition(1, currentEnd);

            if (grappleTip != null)
            {
                grappleTip.position = currentEnd;
            }
        }
    }

    private void CompleteGrapple()
    {
        isExtending = false;
        isGrappling = true;

        if (ropeJoint != null)
        {
            Destroy(ropeJoint);
        }

        ropeJoint = gameObject.AddComponent<DistanceJoint2D>();
        ropeJoint.connectedAnchor = grappleTarget;
        ropeJoint.autoConfigureDistance = false;
        ropeJoint.distance = Vector2.Distance(transform.position, grappleTarget) * ropeFlexibility;
        ropeJoint.maxDistanceOnly = false;

        rb.linearVelocity += new Vector2(0f, swingForce);
    }

    private void StopGrapple()
    {
        isGrappling = false;
        isExtending = false;
        ropeLine.enabled = false;

        if (ropeJoint != null)
        {
            Destroy(ropeJoint);
            ropeJoint = null;
        }

        if (grappleTip != null)
        {
            grappleTip.gameObject.SetActive(false);
        }
    }

    private void UpdateRopePosition()
    {
        ropeLine.SetPosition(0, transform.position);
        ropeLine.SetPosition(1, ropeJoint.connectedAnchor);

        if (grappleTip != null)
        {
            grappleTip.position = ropeJoint.connectedAnchor;
        }
    }
}