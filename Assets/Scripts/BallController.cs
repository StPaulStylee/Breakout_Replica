using UnityEngine;

namespace Breakout {
  [RequireComponent(typeof(Rigidbody2D))]
  public class BallController : MonoBehaviour {
    public float MinXSpeed = 1f;
    public float MaxXSpeed = 1f;
    public float MinYSpeed = 1f;
    public float MaxYSpeed = 1f;

    private float _previousVelocityOnX;
    private Rigidbody2D _ballRigidBody { get; set; }
    private Bounds _paddleBounds { get; set; }
    private PaddleController _paddleController;
    [SerializeField]
    private Vector2 _startingPosition;
    [SerializeField]
    private bool _isResetPosition;
    [SerializeField]
    private Vector2 _currentVelocity;
    [SerializeField]
    private int _paddleCollisionCount = 0;
    [SerializeField]
    private float _speedMultiplier = 2f;

    #region Private Methods
    private void Start() {
      _startingPosition = transform.position;
      _ballRigidBody = GetComponent<Rigidbody2D>();
      _currentVelocity = new Vector2(MinXSpeed, -MinYSpeed);
      _previousVelocityOnX = MinXSpeed;
      _paddleController = GameObject.Find("Paddle").GetComponent<PaddleController>();
      if (_paddleController == null) {
        Debug.LogError("No paddle found!");
      }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
      if (collision.gameObject.CompareTag("Paddle")) {
        EventsController.OnEnablingCollision();
        ++_paddleCollisionCount;
        _paddleBounds = collision.collider.bounds;
        ContactPoint2D contactPoint = collision.GetContact(0);
        var distanceFromCenter = contactPoint.point.x - _paddleBounds.center.x;
        _paddleController.SetSegmentHit(distanceFromCenter);
        SetVelocityFromCollisionSegment(_paddleController);
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
    private void SetVelocityFromCollisionSegment(PaddleController paddleController) {
      // if ball velocity on x is negative (moving left)
      if (_ballRigidBody.velocity.x < 0) {
        SetVelocity(paddleController, BallDirection.Left);
      }
      // if ball velocity on x is positive (moving right)
      if (_ballRigidBody.velocity.x > 0) {
        SetVelocity(paddleController, BallDirection.Right);
      }
    }

    private void SetVelocity(PaddleController paddleController, BallDirection direction) {
      if (paddleController.CurrentSegmentHit == PaddleSegmentHit.Center) {
        var velocityOnX = direction == BallDirection.Left ? -_previousVelocityOnX : _previousVelocityOnX;
        _currentVelocity = new Vector2(velocityOnX, -_currentVelocity.y);
        return;
      }
      if (paddleController.CurrentSegmentHit == PaddleSegmentHit.Right) {
        if (paddleController.PreviousSegmentHit == PaddleSegmentHit.Center) {
          var velocityOnX = direction == BallDirection.Left ? -_currentVelocity.x * 2f : _currentVelocity.x * 2f;
          _currentVelocity = new Vector2(velocityOnX, -_currentVelocity.y);
          return;
        }
        if (paddleController.PreviousSegmentHit == PaddleSegmentHit.Right || paddleController.PreviousSegmentHit == PaddleSegmentHit.Left) {
          var velocityOnX = direction == BallDirection.Left ? -_currentVelocity.x : _currentVelocity.x;
          _currentVelocity = new Vector2(velocityOnX, -_currentVelocity.y);
          return;
        }
      }
      if (paddleController.CurrentSegmentHit == PaddleSegmentHit.Left) {
        if (paddleController.PreviousSegmentHit == PaddleSegmentHit.Center) {
          var velocityOnX = direction == BallDirection.Left ? _currentVelocity.x * 2f : -_currentVelocity.x * 2f;
          _currentVelocity = new Vector2(velocityOnX, -_currentVelocity.y);
          return;
        }
        if (paddleController.PreviousSegmentHit == PaddleSegmentHit.Right || paddleController.PreviousSegmentHit == PaddleSegmentHit.Left) {
          var velocityOnX = direction == BallDirection.Left ? _currentVelocity.x : -_currentVelocity.x;
          _currentVelocity = new Vector2(velocityOnX, -_currentVelocity.y);
          return;
        }
      }
    }

    // The size of the SetVelocity method is too big and could use refactoring and this is a placeholder for such
    //private float GetVelocityOnX(PaddleController paddleController) {
    //  if (paddleController.PreviousSegmentHit == PaddleSegmentHit.Center) {

    //  }
    //  if (paddleController.PreviousSegmentHit == PaddleSegmentHit.Right || paddleController.PreviousSegmentHit == PaddleSegmentHit.Left) {

    //  }
    //}
    #endregion
  }
  enum BallDirection {
    Right,
    Left
  }
}