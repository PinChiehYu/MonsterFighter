using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class PhysicsObject : MonoBehaviour {

    [SerializeField]
    private float jumpHeight;
    [SerializeField]
    private float jumpTimeToTop;
    [SerializeField]
    private float shellWidth;
    [SerializeField]
    private int rayCount;
    [SerializeField]
    private LayerMask collisionLayerMask;

    protected bool isGrounded;
    protected Vector2 gravity;
    protected Vector2 velocity;

    private float jumpVelocity;

    private BoxCollider2D bodyCollider;

    public virtual void Awake()
    {
        bodyCollider = GetComponent<BoxCollider2D>(); 

        gravity = Vector2.down * (2 * jumpHeight) / Mathf.Pow(jumpTimeToTop, 2);
        jumpVelocity = Mathf.Abs(gravity.magnitude) * jumpTimeToTop;

        velocity = new Vector2(0f, 0f);
    }

    void FixedUpdate()
    {
        velocity += gravity * Time.fixedDeltaTime;
        Vector2 deltaPosition = velocity * Time.fixedDeltaTime;

        Move(deltaPosition);
    }

    private void Move(Vector2 movement)
    {
        isGrounded = false;

        if (movement.y < 0f)
        {
            HorizontalMovementAdapt(ref movement);
            VerticalMovementAdapt(ref movement);
        }

        if (isGrounded)
        {
            velocity.y = 0f;
        }

        transform.Translate(movement);
    }

    void OnGUI()
    {
        GUI.Label(new Rect(0, 0, 100, 50), velocity.ToString());

    }

    private void HorizontalMovementAdapt(ref Vector2 movement)
    {
        Bounds colliderBounds = bodyCollider.bounds;
        float anchorX = movement.x > 0f ? (colliderBounds.max.x - shellWidth) : (colliderBounds.min.x + shellWidth);
        float bottomY = colliderBounds.min.y + shellWidth;
        float spaceLenght = 2f * (colliderBounds.extents.y - shellWidth) / (rayCount - 1);

        float raycastLength = Mathf.Abs(movement.x) + shellWidth;
        Vector2 raycastDirection = Vector2.right * Mathf.Sign(movement.x);
        Vector2 raycastPoint = new Vector2(anchorX, bottomY - spaceLenght);

        for (int i = 0; i < rayCount; i++)
        {
            raycastPoint.y += spaceLenght;
            RaycastHit2D hit = Physics2D.Raycast(raycastPoint, raycastDirection, raycastLength, collisionLayerMask);
            if (hit)
            {
                //Debug.DrawRay(raycastPoint, raycastDirection, Color.red, raycastLength);
                movement.x = Mathf.Sign(movement.x) * (hit.distance - shellWidth);
            }
        }
    }

    private void VerticalMovementAdapt(ref Vector2 movement)
    {
        Bounds colliderBounds = bodyCollider.bounds;
        float anchorY = colliderBounds.min.y + shellWidth;
        float leftX = colliderBounds.min.x + shellWidth;
        float spaceLenght = 2f * (colliderBounds.extents.x - shellWidth) / (rayCount - 1);

        float raycastLength = -movement.y + shellWidth;
        Vector2 raycastPoint = new Vector2(leftX - spaceLenght, anchorY);

        for (int i = 0; i < rayCount; i++)
        {
            raycastPoint.x += spaceLenght;
            RaycastHit2D hit = Physics2D.Raycast(raycastPoint, Vector2.down, raycastLength, collisionLayerMask);
            if (hit)
            {
                isGrounded = true;
                //Debug.DrawRay(raycastPoint, Vector2.down, Color.red, raycastLength);
                movement.y = -(hit.distance - shellWidth);
            }
        }
    }

    protected void Jump()
    {
        isGrounded = false;
        velocity.y = jumpVelocity;
    }
}
