using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Breakout {
  public class PaddleController : MonoBehaviour {
    private Camera gameCamera;
    private Vector3 startingPosition;
    [SerializeField]
    private bool isFrozen = false;
    public float colliderOffset { get; private set; }

    private void Awake() {
      colliderOffset = GetComponent<BoxCollider2D>().size.x / 3;  
    }

    private void Start() {
      gameCamera = Camera.main;
      startingPosition = transform.position;
      if (isFrozen) {
        transform.position = new Vector3(startingPosition.x, startingPosition.y);
        transform.localScale += new Vector3(17f, 0);
        return;
      }
    }
    private void Update() {
      if (isFrozen) {
        transform.position = new Vector3(startingPosition.x, startingPosition.y);
        return;
      }
      Vector3 newMousePosition = gameCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, startingPosition.y));
      transform.position = new Vector3(newMousePosition.x, startingPosition.y);
    }
  }

}