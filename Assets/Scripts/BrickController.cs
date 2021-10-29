using UnityEngine;

namespace Breakout {
  public class BrickController : MonoBehaviour {
    public delegate void OnGameOverHandler(bool value);
    public static OnGameOverHandler OnGameOver;
    [SerializeField]
    private int Points;
    private AudioSource collisionSfx;
    private bool isGameOver;
    private bool isMaxVelocityInitiator;

    private void Start() {
      collisionSfx = GetComponentInParent<AudioSource>();
      if (!collisionSfx) {
        Debug.LogError($"'{gameObject.name}' does not have an audio source in it's parent");
      }
      OnGameOver += SetIsGameOver;
      SetIsMaxVelocityInitiator();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
      if (isGameOver) {
        return;
      }
      if (collision.CompareTag("Ball")) {
        collisionSfx.Play();
        GameController.OnBrickCollision(Points);
        gameObject.SetActive(false);
        if (isMaxVelocityInitiator) { // Put an or condition here to set max velocity if isBreakout
          BallVelocityManager.OnMaxVelocity();
        }
      }
    }

    private void OnDisable() {
      OnGameOver -= SetIsGameOver;
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