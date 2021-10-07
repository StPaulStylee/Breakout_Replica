using UnityEngine;

namespace Breakout {
  public class BallController : MonoBehaviour {
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
    private Vector2 _previousVelocity;
    private Rigidbody2D _ballRigidBody { get; set; }
    [SerializeField]
    private int _paddleCollisionCount = 0;
    [SerializeField]
    private float _speedMultiplier = 2f;
    private Bounds _paddleBounds { get; set; }

    #region Private Methods
    private void Start() {
      _startingPosition = transform.position;
      _ballRigidBody = GetComponent<Rigidbody2D>();
      _currentVelocity = new Vector2(MinXSpeed, -MinYSpeed);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
      if (collision.gameObject.CompareTag("Paddle")) {
        EventsController.OnEnablingCollision();
        ++_paddleCollisionCount;
        _paddleBounds = collision.collider.bounds;
        ContactPoint2D contactPoint = collision.GetContact(0);
        var centerSegmentSize = 0.0516004f; // TODO: Calculate this from the actual collider size
        var distanceFromCenter = contactPoint.point.x - _paddleBounds.center.x;
        SetVelocityFromCollisionSegment(distanceFromCenter, centerSegmentSize);
      }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
      if (collision.CompareTag("UpperLimit")) {
        EventsController.OnEnablingCollision();
        _currentVelocity = new Vector2(_currentVelocity.x, -_currentVelocity.y);
        return;
      }
      if (collision.CompareTag("Brick")) {
        EventsController.OnBrickCollision();
        //Destroy(collision.gameObject); // Instead of destroy maybe we just inactivate it
        collision.gameObject.SetActive(false);
        _currentVelocity = new Vector2(_currentVelocity.x, -_currentVelocity.y);
        return;
      }
      if (collision.CompareTag("RightLimit") || collision.CompareTag("LeftLimit")) {
        _currentVelocity = new Vector2(-_currentVelocity.x, _currentVelocity.y);
        return;
      }
    }

    private void FixedUpdate() {
      _ballRigidBody.velocity = _currentVelocity;
      //Debug.Log(_ballRigidBody.velocity.y);
      if (_isResetPosition) {
        transform.position = _startingPosition;
        _isResetPosition = false;
      }
    }
    private void SetVelocityFromCollisionSegment(float distanceFromCenter, float centerSegmentSize) {
      // if ball velocity on x is negative (moving left)
      if (_ballRigidBody.velocity.x < 0) {
        SetVelocity(distanceFromCenter, centerSegmentSize, BallDirection.Left);
      }
      // if ball velocity on x is positive (moving right)
      if (_ballRigidBody.velocity.x > 0) {
        SetVelocity(distanceFromCenter, centerSegmentSize, BallDirection.Right);
      }
    }

    private void SetVelocity(float distanceFromCenter, float centerSegmentSize, BallDirection direction) {
      if (distanceFromCenter <= centerSegmentSize && distanceFromCenter >= -centerSegmentSize) { // Center Collision
        _currentVelocity = new Vector2(_currentVelocity.x, -_currentVelocity.y);
        return;
      }
      if (distanceFromCenter > centerSegmentSize) { // Right Collision
        var velocityOnX = direction == BallDirection.Left ? -_currentVelocity.x : _currentVelocity.x;
        _currentVelocity = new Vector2(velocityOnX, -_currentVelocity.y);
        return;
      }
      if (distanceFromCenter < -centerSegmentSize) { // Left Collision
        var velocityOnX = direction == BallDirection.Left ? _currentVelocity.x : -_currentVelocity.x;
        _currentVelocity = new Vector2(velocityOnX, -_currentVelocity.y);
        return;
      }
    }
    #endregion
  }
  enum BallDirection {
    Right,
    Left
  }
}