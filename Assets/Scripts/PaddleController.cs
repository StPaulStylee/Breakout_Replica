using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Breakout {
  [RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
  public class PaddleController : MonoBehaviour {
    public delegate void OnGameOverHandler();
    public delegate void OnTurnEndHandler();
    public delegate void OnMaxVelocityHandler();

    public static OnGameOverHandler OnGameOver;
    public static OnTurnEndHandler OnTurnEnd;
    public static OnMaxVelocityHandler OnMaxVelocity;

    public PaddleSegmentHit CurrentSegmentHit { get; private set; }
    public PaddleSegmentHit PreviousSegmentHit { get; private set; }
    private Rigidbody2D rb;
    private Camera gameCamera;
    private Vector3 startingPosition;
    private float centerSegmentSize;
    private bool isBallMaxVelocityScale;
    [SerializeField]
    private bool isFrozen = false;

    private void Awake() {
      rb = GetComponent<Rigidbody2D>();
      CurrentSegmentHit = PaddleSegmentHit.Center;
      startingPosition = transform.position;
      SetCenterSegmentSize();
      OnGameOver += FreezePaddle;
      OnTurnEnd += ResetState;
      OnMaxVelocity += SetScaleToHalf;
      if (isFrozen) {
        FreezePaddle();
        return;
      }
    }

    private void Start() {
      gameCamera = Camera.main;
    }

    private void Update() {
      if (isFrozen) {
        return;
      }
      Vector3 newMousePosition = gameCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, startingPosition.y));
      transform.position = new Vector3(newMousePosition.x, startingPosition.y);
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
  }
  public enum PaddleSegmentHit {
    Left,
    Center,
    Right
  }
}