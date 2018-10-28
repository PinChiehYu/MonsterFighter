﻿using System.Collections;
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
    protected bool isFaceRight;

    protected float jumpVelocity;

    private BoxCollider2D bodyCollider;

    public virtual void Awake()
    {
        bodyCollider = GetComponent<BoxCollider2D>(); 

        gravity = Vector2.down * (2 * jumpHeight) / Mathf.Pow(jumpTimeToTop, 2);
        jumpVelocity = Mathf.Abs(gravity.magnitude) * jumpTimeToTop;
        isFaceRight = true;
    }

    void FixedUpdate()
    {
        velocity += gravity * Time.fixedDeltaTime;
        Vector2 deltaPosition = velocity * Time.fixedDeltaTime;

        Move(deltaPosition);
        velocity.x = 0f;
    }

    private void Move(Vector2 movement)
    {
        isGrounded = false;

        if (movement.y < 0f)
        {
            VerticalMovementAdapt(ref movement);
        }

        if (isGrounded)
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
                isGrounded = true;
                movement.y = -(hit.distance - shellWidth);
            }
        }
    }

}
