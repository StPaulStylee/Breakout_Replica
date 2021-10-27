using UnityEngine;

namespace Breakout {
  public class BrickController : MonoBehaviour {
    public delegate void OnGameOverHandler(bool value);
    public static OnGameOverHandler OnGameOver;
    [SerializeField]
    private int Points;
    private bool isGameOver;
    private bool isMaxVelocityInitiator;

    private void Start() {
      OnGameOver += SetIsGameOver;
      SetIsMaxVelocityInitiator();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
      if (isGameOver) {
        return;
      }
      if (collision.CompareTag("Ball")) {
        GameController.OnBrickCollision(Points);
        gameObject.SetActive(false);
        if (isMaxVelocityInitiator) { // Put an or condition here to set max velocity if isBreakout
          BallVelocityManager.OnMaxVelocity();
        }
      }
    }

    private void SetIsGameOver(bool gameOverValue) {
      isGameOver = gameOverValue;
    }

    private void SetIsMaxVelocityInitiator() {
      var parentName = transform.parent.name;
      if (parentName == "OrangeBricks" || parentName == "RedBricks") {
        isMaxVelocityInitiator = true;
        return;
      }
      isMaxVelocityInitiator = false;
    }
  }
}