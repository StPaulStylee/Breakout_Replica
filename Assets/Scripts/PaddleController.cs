using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Breakout {
  [RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
  public class PaddleController : MonoBehaviour {
    public delegate void OnGameOverHandler();
    public static OnGameOverHandler OnGameOver;

    public PaddleSegmentHit CurrentSegmentHit { get; private set; }
    public PaddleSegmentHit PreviousSegmentHit { get; private set; }
    private Camera gameCamera;
    private Vector3 startingPosition;
    private float centerSegmentSize = 0.0516004f;
    private Rigidbody2D rb;
    [SerializeField]
    private bool isFrozen = false;

    private void Awake() {
      rb = GetComponent<Rigidbody2D>();
      OnGameOver += FreezePaddle;
      CurrentSegmentHit = PaddleSegmentHit.Center;
      startingPosition = transform.position;
      if (isFrozen) {
        FreezePaddle();
        return;
      }
    }

    private void Start() {
      gameCamera = Camera.main;
      // Try moving everyting below to Awake
      //OnGameOver += FreezePaddle;
      //CurrentSegmentHit = PaddleSegmentHit.Center;
      //startingPosition = transform.position;
      //if (isFrozen) {
      //  FreezePaddle();
      //  return;
      //}
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
      transform.localScale += new Vector3(14.71f, 0);
      rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }
  }
  public enum PaddleSegmentHit {
    Left,
    Center,
    Right
  }
}