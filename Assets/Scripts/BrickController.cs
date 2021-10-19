using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Breakout {
  public class BrickController : MonoBehaviour {
    public delegate void OnGameOverHandler(bool value);
    public static OnGameOverHandler OnGameOver;
    [SerializeField]
    private int Points;
    private bool isGameOver;

    private void Start() {
      OnGameOver += SetIsGameOver;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
      if (isGameOver) {
        return;
      }
      if (collision.CompareTag("Ball")) {
        GameController.OnBrickCollision(Points);
        gameObject.SetActive(false);
      }
    }

    private void SetIsGameOver(bool gameOverValue) {
      isGameOver = gameOverValue;
    }
  }

}