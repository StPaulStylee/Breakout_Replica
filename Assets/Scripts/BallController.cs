﻿using UnityEngine;

public class BallController : MonoBehaviour
{
    public float MinXSpeed = 1f;
    public float MaxXSpeed = 1f;
    public float MinYSpeed = 1f;
    public float MaxYSpeed = 1f;

    private Vector2 CurrentVelocity { get; set; }
    private Rigidbody2D BallRigidBody { get; set; }

    // Difficulty Multiplier - Is it a static product or no?

    // Start is called before the first frame update
    void Start()
    {
        BallRigidBody = GetComponent<Rigidbody2D>();
        CurrentVelocity = new Vector2(MinXSpeed, -MinYSpeed);
        BallRigidBody.velocity = CurrentVelocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Paddle"))
        {
            ContactPoint2D contactPoint = collision.GetContact(0);
            Bounds paddleBounds = collision.collider.bounds;
            // if ball velocity on x is negative (moving left)
            if (BallRigidBody.velocity.x < 0)
            {
                // if hits left of center, maintain
                if (contactPoint.point.x <= paddleBounds.center.x)
                {
                    CurrentVelocity = new Vector2(CurrentVelocity.x, -CurrentVelocity.y);
                    BallRigidBody.velocity = CurrentVelocity;
                    return;
                }
                // else inverse
                CurrentVelocity = new Vector2(-CurrentVelocity.x, -CurrentVelocity.y);
                BallRigidBody.velocity = CurrentVelocity;
                return;
            } 
            // if ball velocity on x is positive (moving right)
            if (BallRigidBody.velocity.x > 0)
            {
                // if hits right of center, maintain
                if (contactPoint.point.x >= paddleBounds.center.x)
                {
                    CurrentVelocity = new Vector2(CurrentVelocity.x, -CurrentVelocity.y);
                    BallRigidBody.velocity = CurrentVelocity;
                    return;
                }
                // else, inverse
                CurrentVelocity = new Vector2(-CurrentVelocity.x, -CurrentVelocity.y);
                BallRigidBody.velocity = CurrentVelocity;
                return;
            }
        }
        if (collision.gameObject.CompareTag("UpperLimit"))
        {
            CurrentVelocity = new Vector2(CurrentVelocity.x, -CurrentVelocity.y);
            BallRigidBody.velocity = CurrentVelocity;
            return;
        }
        if (collision.gameObject.CompareTag("RightLimit"))
        {
            CurrentVelocity = new Vector2(-CurrentVelocity.x, CurrentVelocity.y);
            BallRigidBody.velocity = CurrentVelocity;
            return;
        }
        if (collision.gameObject.CompareTag("LeftLimit"))
        {
            CurrentVelocity = new Vector2(-CurrentVelocity.x, CurrentVelocity.y);
            BallRigidBody.velocity = CurrentVelocity;
            return;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Debug.Log(ballRigidBody.velocity);
    }
}
