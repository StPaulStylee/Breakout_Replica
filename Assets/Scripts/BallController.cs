using UnityEngine;

public class BallController : MonoBehaviour
{
    public float MinXSpeed = 0f;
    public float MaxXSpeed = 2.0f;
    public float MinYSpeed = 0.5f;
    public float MaxYSpeed = 2.0f;

    private Vector2 currentVelocity { get; set; }
    private Rigidbody2D ballRigidBody;

    // Difficulty Multiplier - Is it a static product or no?


    // Start is called before the first frame update
    void Start()
    {
        ballRigidBody = GetComponent<Rigidbody2D>();
        currentVelocity = new Vector2(MinXSpeed, -MinYSpeed);
        ballRigidBody.velocity = currentVelocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Paddle") || collision.gameObject.CompareTag("UpperLimit"))
        {
            Debug.Log("Hit the Paddle!");
            // What side of the paddle has it hit - need a reference to the paddle
            ballRigidBody.velocity = new Vector2(currentVelocity.x, -currentVelocity.y);
            currentVelocity = new Vector2(currentVelocity.x, -currentVelocity.y);
            return;
            //Debug.Log(ballRigidBody.velocity);
        }
        if (collision.gameObject.CompareTag("RightLimit"))
        {
            Debug.Log("Hit the SideLimit!");
            ballRigidBody.velocity = new Vector2(-currentVelocity.x, currentVelocity.y);
            currentVelocity = new Vector2(-currentVelocity.x, currentVelocity.y);
            //Debug.Log(ballRigidBody.velocity);
            return;
        }
        if (collision.gameObject.CompareTag("LeftLimit"))
        {
            Debug.Log("Hit the SideLimit!");
            ballRigidBody.velocity = new Vector2(-currentVelocity.x, currentVelocity.y);
            currentVelocity = new Vector2(-currentVelocity.x, currentVelocity.y);
            //Debug.Log(ballRigidBody.velocity);
            return;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Debug.Log(ballRigidBody.velocity);
    }
}
