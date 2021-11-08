using Breakout.Data;
using UnityEngine;

namespace Breakout {
  [RequireComponent(typeof(Rigidbody2D), typeof(BallVelocityManager))]
  public class BallController : MonoBehaviour {
    public delegate void OnGameOverHandler();
    public static OnGameOverHandler OnGameOver;
    public delegate void OnForceVelocityChangeHandler(Vector2 velocity);
    public static OnForceVelocityChangeHandler OnForceVelocityChange;

    private Rigidbody2D ballRigidBody;
    private BallVelocityManager velocityManager;
    private bool isNewTurn = false;
    [SerializeField]
    private Vector2 currentVelocity;
    [SerializeField]
    private bool isResetPosition;

    private void Awake() {
      velocityManager = GetComponent<BallVelocityManager>();
      ballRigidBody = GetComponent<Rigidbody2D>();
    }

    private void Start() {
      OnGameOver += SetStartingPosition;
      OnForceVelocityChange += SetCurrentVelocity;
      SetStartingPosition();
      currentVelocity = velocityManager.GetStartingVelocity();
    }

    private void Update() {
      if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse0)) && isNewTurn) {
        isResetPosition = true;
      }
    }

    private void FixedUpdate() {
      ballRigidBody.velocity = currentVelocity;
      if (isResetPosition) {
        SetStartingPosition();
        currentVelocity = velocityManager.GetStartingVelocity();
        isResetPosition = false;
        isNewTurn = false;
      }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
      if (collision.gameObject.CompareTag("Paddle")) {
        GameController.OnEnablingCollision();
        velocityManager.SetDataFromPaddleCollision(collision);
        currentVelocity = velocityManager.GetVelocity(ColliderTag.Paddle);
        PaddleController.OnBallCollision(false);
      }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
      PaddleController.OnBallCollision(true);
      if (collision.CompareTag("UpperLimit")) {
        GameController.OnEnablingCollision();
        PaddleController.OnMaxVelocity();
        currentVelocity = velocityManager.GetVelocity(ColliderTag.UpperLimit);
        return;
      }
      if (collision.CompareTag("Brick")) {
        GameController.OnDisablingCollision();
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

    private void OnTriggerExit2D(Collider2D collision) {
      if (collision.CompareTag("LowerLimit") || collision.CompareTag("EscapeLimit")) {
        GameController.OnTurnEnd();
        PaddleController.OnTurnEnd();
        velocityManager.RestoreToTurnStartState();
        isNewTurn = true;
      }
    }

    private void OnDisable() {
      OnGameOver -= SetStartingPosition;
      OnForceVelocityChange -= SetCurrentVelocity;
    }

    private void SetStartingPosition() {
      var positionOnX = Random.Range(-2f, 2f);
      transform.position = new Vector2(positionOnX, 1f);
    }

    private void SetCurrentVelocity(Vector2 velocity) {
      currentVelocity = velocity;
    }
  }
}