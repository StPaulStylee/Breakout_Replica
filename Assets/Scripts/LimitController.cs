using UnityEngine;

namespace Breakout {
  public class LimitController : MonoBehaviour {
    public delegate void OnGameOverHandler(bool value);
    public static OnGameOverHandler OnGameOver;
    private bool isGameOver;
    [SerializeField]
    private AudioSource collisionSfx;

    private void Awake() {
      OnGameOver += SetIsGameOver;
      collisionSfx = GetComponent<AudioSource>();
      if (!collisionSfx) {
        Debug.LogError($"'{gameObject.name}' does not have an audio source");
      }
    }
    private void OnTriggerEnter2D(Collider2D collision) {
      if (isGameOver) {
        return;
      }
      if (collision.CompareTag("Ball")) {
        collisionSfx.Play();
      }
    }

    private void OnDisable() {
      OnGameOver -= SetIsGameOver;
    }

    private void SetIsGameOver(bool gameOverValue) {
      isGameOver = gameOverValue;
    }
  }
}