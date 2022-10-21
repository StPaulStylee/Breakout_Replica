using UnityEngine;

namespace Breakout {
  [RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
  public class PaddleController : MonoBehaviour {
    public delegate void OnGameOverHandler(bool value);
    public delegate void OnTurnEndHandler();
    public delegate void OnMaxVelocityHandler();
    public delegate void OnBallCollisionHandler(bool value);

    public static OnGameOverHandler OnGameOver;
    public static OnTurnEndHandler OnTurnEnd;
    public static OnMaxVelocityHandler OnMaxVelocity;
    public static OnBallCollisionHandler OnBallCollision;

    public PaddleSegmentHit CurrentSegmentHit { get; private set; }
    public PaddleSegmentHit PreviousSegmentHit { get; private set; }
    private Rigidbody2D rb;
    private BoxCollider2D col;
    private Camera gameCamera;
    private Vector3 startingPosition;
    private float centerSegmentSize;
    private bool isBallMaxVelocityScale;
    [SerializeField] private bool isFrozen = false;
    [SerializeField] private AudioSource collisionSfx;

    private void Awake() {
      rb = GetComponent<Rigidbody2D>();
      col = GetComponent<BoxCollider2D>();
      CurrentSegmentHit = PaddleSegmentHit.Center;
      startingPosition = transform.position;
      SetCenterSegmentSize();
      OnGameOver += SetIsGameOver;
      OnTurnEnd += ResetState;
      OnMaxVelocity += SetScaleToHalf;
      OnBallCollision += SetColliderState;
      if (isFrozen) {
        FreezePaddle();
        return;
      }
    }

    private void Start() {
      collisionSfx = GetComponentInParent<AudioSource>();
      if (!collisionSfx) {
        Debug.LogError($"'{gameObject.name}' does not have an audio source in it's parent");
      }
      gameCamera = Camera.main;
    }

    private void Update() {
      if (isFrozen) {
        return;
      }
      Vector3 newMousePosition = gameCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, startingPosition.y));
      transform.position = new Vector3(newMousePosition.x, startingPosition.y);
    }

    // Disable the paddle collider on collision until the ball hits something else
    private void OnCollisionEnter2D(Collision2D collision) {
      if (isFrozen) {
        return;
      }
      if (collision.gameObject.CompareTag("Ball")) {
        collisionSfx.Play();
      }
    }

    private void OnDisable() {
      OnGameOver -= SetIsGameOver;
      OnTurnEnd -= ResetState;
      OnMaxVelocity -= SetScaleToHalf;
      OnBallCollision -= SetColliderState;
    }

    public void SetSegmentHit(float hitDistanceFromCenter) {
      PreviousSegmentHit = CurrentSegmentHit;
      if (hitDistanceFromCenter <= centerSegmentSize && hitDistanceFromCenter >= -centerSegmentSize) {
        CurrentSegmentHit = PaddleSegmentHit.Center;
        return;
      }
      if (hitDistanceFromCenter > centerSegmentSize) {
        CurrentSegmentHit = PaddleSegmentHit.Right;
        return;
      }
      if (hitDistanceFromCenter < -centerSegmentSize) {
        CurrentSegmentHit = PaddleSegmentHit.Left;
        return;
      }
    }

    private void FreezePaddle() {
      isFrozen = true;
      transform.position = new Vector3(startingPosition.x, startingPosition.y);
      transform.localScale = Vector3.one;
      transform.localScale += new Vector3(14.71f, 0);
      rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    private void SetScaleToHalf() {
      if (isBallMaxVelocityScale) {
        return;
      }
      transform.localScale = new Vector3(0.5f, 1f, 1f);
      SetCenterSegmentSize();
      isBallMaxVelocityScale = true;
    }

    private void SetCenterSegmentSize() {
      centerSegmentSize = (GetComponent<BoxCollider2D>().size.x / 3) / 2;
    }

    private void ResetState() {
      if (!isFrozen) {
        transform.localScale = Vector3.one;
      }
      isBallMaxVelocityScale = false;
      SetCenterSegmentSize();
    }

    private void SetIsGameOver(bool gameOverValue) {
      if (gameOverValue) {
        FreezePaddle();
        return;
      }
      ResetState();
    }

    private void SetColliderState(bool isEnabled) {
      col.enabled = isEnabled;
    }
  }
  public enum PaddleSegmentHit {
    Left,
    Center,
    Right
  }
}