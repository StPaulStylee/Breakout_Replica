using Breakout.Data;
using UnityEngine;

namespace Breakout {
  [RequireComponent(typeof(Rigidbody2D), typeof(BallVelocityManager))]
  public class BallController : MonoBehaviour {
    private Rigidbody2D ballRigidBody;
    private BallVelocityManager velocityManager;
    [SerializeField]
    private Vector2 startingPosition;
    [SerializeField]
    private Vector2 currentVelocity;
    [SerializeField]
    private bool isResetPosition;

    private void Awake() {
      velocityManager = GetComponent<BallVelocityManager>();
      ballRigidBody = GetComponent<Rigidbody2D>();
    }
    private void Start() {
      startingPosition = transform.position;
      currentVelocity = velocityManager.GetStartingVelocity();
    }

    private void Update() {
      if (Input.GetKeyDown(KeyCode.Space)) {
        isResetPosition = true;
      }
    }

    private void FixedUpdate() {
      ballRigidBody.velocity = currentVelocity;
      if (isResetPosition) {
        transform.position = startingPosition;
        isResetPosition = false;
      }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
      if (collision.gameObject.CompareTag("Paddle")) {
        EventsController.OnEnablingCollision();
        velocityManager.SetDataFromPaddleCollision(collision);
        currentVelocity = velocityManager.GetVelocity(ColliderTag.Paddle);
      }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
      if (collision.CompareTag("UpperLimit")) {
        EventsController.OnEnablingCollision();
        currentVelocity = velocityManager.GetVelocity(ColliderTag.UpperLimit);
        return;
      }
      if (collision.CompareTag("Brick")) {
        EventsController.OnBrickCollision();
        currentVelocity = velocityManager.GetVelocity(ColliderTag.Brick);
        return;
      }
      if (collision.CompareTag("RightLimit")) {
        currentVelocity = velocityManager.GetVelocity(ColliderTag.RightLimit);
        return;
      }
      if (collision.CompareTag("LeftLimit")) {
        currentVelocity = velocityManager.GetVelocity(ColliderTag.LeftLimit);
        return;
      }
    }
  }
}