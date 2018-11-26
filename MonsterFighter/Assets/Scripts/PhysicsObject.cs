using System.Collections;
using System.Collections.Generic;
using System;
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

    public bool IsGrounded { get; private set; }
    public Vector2 Velocity => velocity;
    private Vector2 velocity;
    private float jumpVelocity;
    private Vector2 acceleration;

    private Vector2 defaultGravity;

    public bool IsFaceRight
    {
        get { return isFaceRight; }
        set {
            if (value ^ isFaceRight)
            {
                isFaceRight = value;
                transform.Rotate(Vector3.up * 180);
            }
        }
    }
    private bool isFaceRight;

    private BoxCollider2D bodyCollider;

    void Awake()
    {
        bodyCollider = GetComponent<BoxCollider2D>(); 

        acceleration = Vector2.down * (2 * jumpHeight) / Mathf.Pow(jumpTimeToTop, 2);
        jumpVelocity = Mathf.Abs(acceleration.magnitude) * jumpTimeToTop;
        velocity = Vector2.zero;

        defaultGravity = acceleration;

        isFaceRight = true;
        IsGrounded = false;
    }

    void FixedUpdate()
    {
        velocity += acceleration * Time.fixedDeltaTime;
        Vector2 deltaPosition = velocity * Time.fixedDeltaTime;

        Move(deltaPosition);
    }

    private void Move(Vector2 movement)
    {
        IsGrounded = false;

        if (movement.y < 0f)
        {
            VerticalMovementAdapt(ref movement);
        }

        if (IsGrounded)
        {
            HorizontalMovementAdapt(ref movement);
            velocity.y = 0f;
        }

        transform.Translate(movement);
    }

    private void HorizontalMovementAdapt(ref Vector2 movement)
    {
        Bounds colliderBounds = bodyCollider.bounds;
        float anchorX = isFaceRight ? (colliderBounds.max.x - shellWidth) : (colliderBounds.min.x + shellWidth);
        float bottomY = colliderBounds.min.y + shellWidth;
        float spaceLenght = 2f * (colliderBounds.extents.y - shellWidth) / (rayCount - 1);

        float raycastLength = movement.x + shellWidth;
        Vector2 raycastDirection = isFaceRight ? Vector2.right : Vector2.left;
        Vector2 raycastPoint = new Vector2(anchorX, bottomY - spaceLenght);

        for (int i = 0; i < rayCount; i++)
        {
            raycastPoint.y += spaceLenght;
            RaycastHit2D hit = Physics2D.Raycast(raycastPoint, raycastDirection, raycastLength, collisionLayerMask);
            if (hit)
            {
                movement.x = hit.distance - shellWidth;
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
                IsGrounded = true;
                movement.y = -(hit.distance - shellWidth);
            }
        }
    }

    public void Forward(float v)
    {
        velocity.x = v;
    }

    public void Jump()
    {
        IsGrounded = false;
        velocity.y = jumpVelocity;
    }

    public void SetPhysicsParam(Vector2? velo, Vector2? accel)
    {
        velocity = velo.HasValue ? velo.Value : velocity;
        acceleration = accel.HasValue ? accel.Value : defaultGravity;
    }
}
