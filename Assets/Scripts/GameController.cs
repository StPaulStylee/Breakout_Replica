using UnityEngine;
using UnityEngine.UI;

namespace Breakout {
  public class GameController : MonoBehaviour {
    public delegate void OnBallEventHandler();
    public delegate void OnBrickEventHandler(int points);

    public static OnBallEventHandler OnDisablingCollision;
    public static OnBallEventHandler OnEnablingCollision;
    public static OnBallEventHandler OnTurnEnd;
    public static OnBrickEventHandler OnBrickCollision;


    [field: SerializeField]
    public int PlayerCurrentTurn { get; private set; } = 1;
    public int PlayerPoints { get; private set; } = 0;

    private bool isBricksEnabled = false;
    [SerializeField]
    private int PlayerTurnsAllowed = 3;
    [SerializeField]
    private GameObject[] bricks;
    [SerializeField]
    private Text player1TurnsText;
    [SerializeField]
    private Text player1ScoreText;

    void Start() {
      isBricksEnabled = true;
      bricks = GameObject.FindGameObjectsWithTag("Brick");
      OnDisablingCollision += DisableBrickIsTrigger;
      OnEnablingCollision += EnableBrickIsTrigger;
      OnTurnEnd += UpdateTurnsRemaining;
      OnBrickCollision += GivePlayerPoints;
      BrickController.OnGameOver(false);
      player1TurnsText.text = PlayerCurrentTurn.ToString();
    }

    public void GivePlayerPoints(int points) {
      PlayerPoints += points;
      player1ScoreText.text = PlayerPoints.ToString().PadLeft(3, '0');
    }

    private void DisableBrickIsTrigger() {
      if (isBricksEnabled) {
        foreach (GameObject brick in bricks) {
          if (brick != null) {
            Collider2D collider = brick.GetComponent<Collider2D>();
            collider.enabled = false;
          }
        }
        isBricksEnabled = false;
      }
    }

    private void EnableBrickIsTrigger() {
      if (!isBricksEnabled) {
        foreach (GameObject brick in bricks) {
          if (brick != null) {
            Collider2D collider = brick.GetComponent<Collider2D>();
            collider.enabled = true;
          }
        }
        isBricksEnabled = true;
      }
    }

    private void UpdateTurnsRemaining() {
      ++PlayerCurrentTurn;
      player1TurnsText.text = PlayerCurrentTurn.ToString();
      if (PlayerCurrentTurn > PlayerTurnsAllowed) {
        PaddleController.OnGameOver();
        BallController.OnGameOver();
        BrickController.OnGameOver(true);
        TextBlink.OnBlink(false);
        return;
      }
    }
  }
}