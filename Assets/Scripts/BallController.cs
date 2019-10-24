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
        private Vector2 _startingPosition;

        [SerializeField]
        private bool _isResetPosition;
        [SerializeField]
        private Vector2 _currentVelocity;
        private Rigidbody2D _ballRigidBody { get; set; }
        [SerializeField]
        private int _paddleCollisionCount = 0;
        [SerializeField]
        private float _speedMultiplier = 2f;
        private Bounds _paddleBounds { get; set; }


        // Difficulty Multiplier - Is it a static product or no?

        // Start is called before the first frame update
        private void Start()
        {
            _startingPosition = transform.position;
            _ballRigidBody = GetComponent<Rigidbody2D>();
            _paddleBounds = GameObject.Find("Paddle").GetComponent<Collider2D>().bounds;
            _currentVelocity = new Vector2(MinXSpeed, -MinYSpeed);
            //_ballRigidBody.velocity = _currentVelocity;
        }

        #region Private Methods
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Paddle"))
            {
                Debug.Log("Paddle Collision Detected!");
                ++_paddleCollisionCount;
                ContactPoint2D contactPoint = collision.GetContact(0);
                //Bounds _paddleBounds = collision.collider.bounds;

                EventsController.OnEnablingCollision();
                // if ball velocity on x is negative (moving left)
                if (_ballRigidBody.velocity.x < 0)
                {
                    // if hits left of center, maintain
                    if (contactPoint.point.x <= _paddleBounds.center.x)
                    {   
                        // If the paddle has had 4, 8, or 12 collisions with ball, increase the speed
                        // Will need to adjust this logic to account for orange/red bricks that max the speed right away
                        if (_paddleCollisionCount == 4 || _paddleCollisionCount == 8 || _paddleCollisionCount == 12)
                        {
                            _currentVelocity = new Vector2(_currentVelocity.x, (-_currentVelocity.y  * _speedMultiplier));
                            //_ballRigidBody.velocity = _currentVelocity;
                            return;
                        }
                        _currentVelocity = new Vector2(_currentVelocity.x, -_currentVelocity.y);
                        //_ballRigidBody.velocity = _currentVelocity;
                        return;
                    }
                    // else inverse
                    if (_paddleCollisionCount == 4 || _paddleCollisionCount == 8 || _paddleCollisionCount == 12)
                    {
                        _currentVelocity = new Vector2(-_currentVelocity.x , (-_currentVelocity.y  * _speedMultiplier));
                        //_ballRigidBody.velocity = _currentVelocity;
                        return;
                    }
                    _currentVelocity = new Vector2(-_currentVelocity.x, -_currentVelocity.y);
                    //_ballRigidBody.velocity = _currentVelocity;
                    return;
                }
                // if ball velocity on x is positive (moving right)
                if (_ballRigidBody.velocity.x > 0)
                {
                    // if hits right of center, maintain
                    if (contactPoint.point.x >= _paddleBounds.center.x)
                    {
                        if (_paddleCollisionCount == 4 || _paddleCollisionCount == 8 || _paddleCollisionCount == 12)
                        {
                            _currentVelocity = new Vector2(_currentVelocity.x, (-_currentVelocity.y * _speedMultiplier));
                            //_ballRigidBody.velocity = _currentVelocity;
                            return;
                        }
                        _currentVelocity = new Vector2(_currentVelocity.x, -_currentVelocity.y);
                        //_ballRigidBody.velocity = _currentVelocity;
                        return;
                    }
                    // else, inverse
                    if (_paddleCollisionCount == 4 || _paddleCollisionCount == 8 || _paddleCollisionCount == 12)
                    {
                        _currentVelocity = new Vector2(-_currentVelocity.x, (-_currentVelocity.y * _speedMultiplier));
                        //_ballRigidBody.velocity = _currentVelocity;
                        return;
                    }
                    _currentVelocity = new Vector2(-_currentVelocity.x, -_currentVelocity.y);
                    //_ballRigidBody.velocity = _currentVelocity;
                    return;
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("UpperLimit"))
            {
                _currentVelocity = new Vector2(_currentVelocity.x, -_currentVelocity.y);
                //_ballRigidBody.velocity = _currentVelocity;
                EventsController.OnEnablingCollision();
                return;
            }
            if (collision.CompareTag("Brick"))
            {
                EventsController.OnBrickCollision();
                Destroy(collision.gameObject); // Instead of destroy maybe we just inactivate it
                _currentVelocity = new Vector2(_currentVelocity.x, -_currentVelocity.y);
                //_ballRigidBody.velocity = _currentVelocity;
                return;
            }
            if (collision.CompareTag("RightLimit"))
            {
                _currentVelocity = new Vector2(-_currentVelocity.x, _currentVelocity.y);
                //_ballRigidBody.velocity = _currentVelocity;
                return;
            }
            if (collision.CompareTag("LeftLimit"))
            {
                _currentVelocity = new Vector2(-_currentVelocity.x, _currentVelocity.y);
                //_ballRigidBody.velocity = _currentVelocity;
                return;
            }
        }

        // Update is called once per frame
        private void FixedUpdate()
        {
            _ballRigidBody.velocity = _currentVelocity;
            //Debug.Log(ballRigidBody.velocity);
            if (_isResetPosition)
            {
                //_currentVelocity = new Vector2(MinXSpeed, -MinYSpeed);
                transform.position = _startingPosition;
                _isResetPosition = false;
            }
        }
    }
    #endregion
}
