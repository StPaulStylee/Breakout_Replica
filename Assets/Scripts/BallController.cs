using UnityEngine;

public class BallController : MonoBehaviour
{
    public float MinXSpeed = 0f;
    public float MaxXSpeed = 2.0f;
    public float MinYSpeed = 0.5f;
    public float MaxYSpeed = 2.0f;
    public Vector2 CurrentVelocity { get; set; }

    private Rigidbody2D ballRigidBody;
    private Collider2D ballCollider;

    // Difficulty Multiplier - Is it a static product or no?


    // Start is called before the first frame update
    void Start()
    {
        ballCollider = GetComponent<Collider2D>();
        ballRigidBody = GetComponent<Rigidbody2D>();
        CurrentVelocity = new Vector2(MinXSpeed, -MinYSpeed);
        ballRigidBody.velocity = CurrentVelocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var tag = collision.gameObject.tag;
        switch (tag)
        {
            case "Paddle":
                Debug.Log("Hit the Paddle!");
                // What side of the paddle has it hit - need a reference to the paddle
                ballRigidBody.velocity = new Vector2(CurrentVelocity.x, -CurrentVelocity.y);
                //Debug.Log(ballRigidBody.velocity);
                break;
            case "RightLimit":
                Debug.Log("Hit the SideLimit!");
                ballRigidBody.velocity = new Vector2(-CurrentVelocity.x, -CurrentVelocity.y);
                //Debug.Log(ballRigidBody.velocity);
                break;
            case "LeftLimit":
                Debug.Log("Hit the SideLimit!");
                ballRigidBody.velocity = new Vector2(CurrentVelocity.x, CurrentVelocity.y);
                //Debug.Log(ballRigidBody.velocity);
                break;
            case "UpperLimit":
                Debug.Log("Hit the UpperLimit!");
                ballRigidBody.velocity = new Vector2(-CurrentVelocity.x, CurrentVelocity.y);
                //Debug.Log(ballRigidBody.velocity);
                break;
            default:
                break;
        }
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    ContactPoint2D[] contacts = new ContactPoint2D[10];
    //    int contactCount = ballCollider.GetContacts(contacts);
    //    for (int i = 0; i < contactCount; i++)
    //    {
    //        Debug.Log(contacts[i]);
    //        ContactPoint2D contact = contacts[i];
    //    }
    //    string tag = collision.tag;
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
    void FixedUpdate()
    {
        //Debug.Log(ballRigidBody.velocity);
    }
}
