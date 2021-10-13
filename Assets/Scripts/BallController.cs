using Breakout.Data;
using System.Collections.Generic;
using UnityEngine;

namespace Breakout {
  [RequireComponent(typeof(Rigidbody2D))]
  public class BallController : MonoBehaviour {
    private float _previousVelocityOnX;
    private Rigidbody2D _ballRigidBody { get; set; }
    private Bounds _paddleBounds { get; set; }
    private PaddleController _paddleController;
    private BallVelocityManager _velocityManager;
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
    private void Awake() {
      _velocityManager = GetComponent<BallVelocityManager>();
      _ballRigidBody = GetComponent<Rigidbody2D>();
      if (_velocityManager == null || _ballRigidBody == null) {
        Debug.LogError("The BallController is missing required components.");
      }
    }
    private void Start() {
      _startingPosition = transform.position;
      //_ballRigidBody = GetComponent<Rigidbody2D>();
      // Create methods to set the starting velocity and position??
      _currentVelocity = _velocityManager.GetStartingVelocity();
      //_previousVelocityOnX = _currentVelocity.x;
      //_paddleController = GameObject.Find("Paddle").GetComponent<PaddleController>();
      //if (_paddleController == null) {
      //  Debug.LogError("No paddle found!");
      //}
    }

    private void OnCollisionEnter2D(Collision2D collision) {
      if (collision.gameObject.CompareTag("Paddle")) {
        EventsController.OnEnablingCollision();
        _velocityManager.SetDataFromPaddleCollision(collision);
        _currentVelocity = _velocityManager.GetVelocity(ColliderTag.Paddle);
        //SetVelocityFromCollisionSegment(_paddleController);
        //Debug.Log(_currentVelocity);
      }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
      if (collision.CompareTag("UpperLimit")) {
        EventsController.OnEnablingCollision();
        _currentVelocity = _velocityManager.GetVelocity(ColliderTag.UpperLimit);
        //_velocityManager.SetVelocity(ColliderTag.UpperLimit);
        //_currentVelocity = new Vector2(_currentVelocity.x, -_currentVelocity.y);
        return;
      }
      if (collision.CompareTag("Brick")) {
        EventsController.OnBrickCollision();
        collision.gameObject.SetActive(false); // Have the brick disable itself on collision 
        _currentVelocity = _velocityManager.GetVelocity(ColliderTag.Brick);
        //_currentVelocity = new Vector2(_currentVelocity.x, -_currentVelocity.y);
        return;
      }
      if (collision.CompareTag("RightLimit")) {
        _currentVelocity = _velocityManager.GetVelocity(ColliderTag.RightLimit);
        //_currentVelocity = new Vector2(-_currentVelocity.x, _currentVelocity.y);
        return;
      }
      if (collision.CompareTag("LeftLimit")) {
        _currentVelocity = _velocityManager.GetVelocity(ColliderTag.LeftLimit);
        //_currentVelocity = new Vector2(-_currentVelocity.x, _currentVelocity.y);
        return;
      }
    }

    private void Update() {
      if (Input.GetKeyDown(KeyCode.Space)) {
        _isResetPosition = true;
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
    //private void SetVelocityFromCollisionSegment(PaddleController paddleController) {
    //  // if ball velocity on x is negative (moving left)
    //  if (_ballRigidBody.velocity.x < 0) {
    //    //SetVelocity(paddleController, BallDirection.Left);
    //    _currentVelocity = _velocityManager.GetCurrentVelocity(paddleController, BallDirection.Left, _paddleCollisionCount);
    //  }
    //  // if ball velocity on x is positive (moving right)
    //  if (_ballRigidBody.velocity.x > 0) {
    //    //SetVelocity(paddleController, BallDirection.Right);
    //    _currentVelocity = _velocityManager.GetCurrentVelocity(paddleController, BallDirection.Right, _paddleCollisionCount);
    //  }
    //}

    //private void SetVelocity(PaddleController paddleController, BallDirection direction) {

    //}

    // The size of the SetVelocity method is too big and could use refactoring and this is a placeholder for such
    //private float GetVelocityOnX(PaddleController paddleController) {
    //  if (paddleController.PreviousSegmentHit == PaddleSegmentHit.Center) {

    //  }
    //  if (paddleController.PreviousSegmentHit == PaddleSegmentHit.Right || paddleController.PreviousSegmentHit == PaddleSegmentHit.Left) {

    //  }
    //}
    #endregion
  }
}