using UnityEngine;

namespace Breakout
{
    public class BallController : MonoBehaviour
    {
        public float MinXSpeed = 1f;
        public float MaxXSpeed = 1f;
        public float MinYSpeed = 1f;
        public float MaxYSpeed = 1f;

        [SerializeField]
        private Vector2 startingPosition;

        [SerializeField]
        private bool isResetPosition;

        private Vector2 CurrentVelocity { get; set; }
        private Rigidbody2D BallRigidBody { get; set; }
        [SerializeField]
        private int PaddleCollisionCount = 0;
        [SerializeField]
        private float SpeedMultiplier = 2f;


        // Difficulty Multiplier - Is it a static product or no?

        // Start is called before the first frame update
        private void Start()
        {
            startingPosition = transform.position;
            BallRigidBody = GetComponent<Rigidbody2D>();
            CurrentVelocity = new Vector2(MinXSpeed, -MinYSpeed);
            BallRigidBody.velocity = CurrentVelocity;
        }

        #region Private Methods
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Paddle"))
            {
                ++PaddleCollisionCount;
                ContactPoint2D contactPoint = collision.GetContact(0);
                Bounds paddleBounds = collision.collider.bounds;

                EventsController.OnEnablingCollision();
                // if ball velocity on x is negative (moving left)
                if (BallRigidBody.velocity.x < 0)
                {
                    // if hits left of center, maintain
                    if (contactPoint.point.x <= paddleBounds.center.x)
                    {   
                        // If the paddle has had 4, 8, or 12 collisions with ball, increase the speed
                        // Will need to adjust this logic to account for orange/red bricks that max the speed right away
                        if (PaddleCollisionCount == 4 || PaddleCollisionCount == 8 || PaddleCollisionCount == 12)
                        {
                            CurrentVelocity = new Vector2(CurrentVelocity.x, (-CurrentVelocity.y  * SpeedMultiplier));
                            BallRigidBody.velocity = CurrentVelocity;
                            return;
                        }
                        CurrentVelocity = new Vector2(CurrentVelocity.x, -CurrentVelocity.y);
                        BallRigidBody.velocity = CurrentVelocity;
                        return;
                    }
                    // else inverse
                    if (PaddleCollisionCount == 4 || PaddleCollisionCount == 8 || PaddleCollisionCount == 12)
                    {
                        CurrentVelocity = new Vector2((-CurrentVelocity.x * SpeedMultiplier), -CurrentVelocity.y);
                        BallRigidBody.velocity = CurrentVelocity;
                        return;
                    }
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
                        if (PaddleCollisionCount == 4 || PaddleCollisionCount == 8 || PaddleCollisionCount == 12)
                        {
                            CurrentVelocity = new Vector2(CurrentVelocity.x, (-CurrentVelocity.y * SpeedMultiplier));
                            BallRigidBody.velocity = CurrentVelocity;
                            return;
                        }
                        CurrentVelocity = new Vector2(CurrentVelocity.x, -CurrentVelocity.y);
                        BallRigidBody.velocity = CurrentVelocity;
                        return;
                    }
                    // else, inverse
                    if (PaddleCollisionCount == 4 || PaddleCollisionCount == 8 || PaddleCollisionCount == 12)
                    {
                        CurrentVelocity = new Vector2((-CurrentVelocity.x * SpeedMultiplier), -CurrentVelocity.y);
                        BallRigidBody.velocity = CurrentVelocity;
                        return;
                    }
                    CurrentVelocity = new Vector2(-CurrentVelocity.x, -CurrentVelocity.y);
                    BallRigidBody.velocity = CurrentVelocity;
                    return;
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("UpperLimit"))
            {
                CurrentVelocity = new Vector2(CurrentVelocity.x, -CurrentVelocity.y);
                BallRigidBody.velocity = CurrentVelocity;
                EventsController.OnEnablingCollision();
                return;
            }
            if (collision.CompareTag("Brick"))
            {
               
                
                    EventsController.OnBrickCollision();
       
                Destroy(collision.gameObject);
                CurrentVelocity = new Vector2(CurrentVelocity.x, -CurrentVelocity.y);
                BallRigidBody.velocity = CurrentVelocity;
                return;
            }
            if (collision.CompareTag("RightLimit"))
            {
                CurrentVelocity = new Vector2(-CurrentVelocity.x, CurrentVelocity.y);
                BallRigidBody.velocity = CurrentVelocity;
                return;
            }
            if (collision.CompareTag("LeftLimit"))
            {
                CurrentVelocity = new Vector2(-CurrentVelocity.x, CurrentVelocity.y);
                BallRigidBody.velocity = CurrentVelocity;
                return;
            }
        }

        // Update is called once per frame
        private void Update()
        {
            //Debug.Log(ballRigidBody.velocity);
            if (isResetPosition)
            {
                transform.position = startingPosition;
            }
            isResetPosition = false;
        }
    }
    #endregion
}
