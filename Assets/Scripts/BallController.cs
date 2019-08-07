using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public float MinXSpeed = 0f;
    public float MaxXSpeed = 2.0f;
    public float MinYSpeed = 0.5f;
    public float MaxYSpeed = 2.0f;
    public float CurrentBallSpeed; // This may not be necessary

    private Rigidbody2D ballRigidBody;

    // Difficulty Multiplier - Is it a static product or no?


    // Start is called before the first frame update
    void Start()
    {
        ballRigidBody = GetComponent<Rigidbody2D>();
        ballRigidBody.velocity = new Vector2(MinXSpeed, -MinYSpeed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string tag = collision.tag;
        switch (tag)
        {
            case "Paddle":
                Debug.Log("Hit the Paddle!");
                // What side of the paddle has it hit - need a reference to the paddle
                ballRigidBody.velocity = new Vector2(ballRigidBody.velocity.x, -ballRigidBody.velocity.y);
                break;
            case "SideLimit":
                Debug.Log("Hit the SideLimit!");
                ballRigidBody.velocity = new Vector2(-ballRigidBody.velocity.x, ballRigidBody.velocity.y);
                break;
            case "UpperLimit":
                Debug.Log("Hit the UpperLimit!");
                ballRigidBody.velocity = new Vector2(ballRigidBody.velocity.x, -ballRigidBody.velocity.y);
                break;
            default:
                break;
        }
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    string tag = collision.collider.tag;
    //    switch (tag)
    //    {
    //        case "Paddle":
    //            Debug.Log("Hit the Paddle!");
    //            // What side of the paddle has it hit - need a reference to the paddle
    //            ballRigidBody.velocity = new Vector2(ballRigidBody.velocity.x, -ballRigidBody.velocity.y);
    //            break;
    //        case "SideLimit":
    //            Debug.Log("Hit the SideLimit!");
    //            ballRigidBody.velocity = new Vector2(-ballRigidBody.velocity.x, ballRigidBody.velocity.y);
    //            break;
    //        case "UpperLimit":
    //            Debug.Log("Hit the UpperLimit!");
    //            ballRigidBody.velocity = new Vector2(ballRigidBody.velocity.x, -ballRigidBody.velocity.y);
    //            break;
    //        default:
    //            break;
    //    }
    //}

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(ballRigidBody.velocity);
    }
}
