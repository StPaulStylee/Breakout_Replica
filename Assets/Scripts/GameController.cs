﻿using Breakout.HighScore;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Breakout {
  public class GameController : MonoBehaviour {
    public delegate void OnBallEventHandler();
    public delegate void OnBrickEventHandler(int points);
    public delegate void OnHighScoreHandler(bool isVisible);

    public static OnBallEventHandler OnDisablingCollision;
    public static OnBallEventHandler OnEnablingCollision;
    public static OnBallEventHandler OnTurnEnd;
    public static OnBrickEventHandler OnBrickCollision;
    public static OnHighScoreHandler OnSetHighScoreTable;


    [field: SerializeField] public int PlayerCurrentTurn { get; private set; } = 1;
    public int PlayerPoints { get; private set; } = 0;

    private bool isBricksEnabled = false;
    [SerializeField] private int PlayerTurnsAllowed = 3;
    [SerializeField] private GameObject[] bricks;
    [SerializeField] private Text player1TurnsText;
    [SerializeField] private Text player1ScoreText;
    [SerializeField] private bool isGameOver = false;
    [SerializeField] private bool isHighScoreCanvasVisible = false;

    private void Awake() {
      OnDisablingCollision += DisableBrickIsTrigger;
      OnEnablingCollision += EnableBrickIsTrigger;
      OnTurnEnd += UpdateTurnsRemaining;
      OnBrickCollision += GivePlayerPoints;
      OnSetHighScoreTable += SetHighScoreCanvaseVisibility;
      isBricksEnabled = true;
    }

    void Start() {
      bricks = GameObject.FindGameObjectsWithTag("Brick");
      player1TurnsText = GameObject.Find("PlayerTurnCount").GetComponent<Text>();
      player1ScoreText = GameObject.Find("Player1ScoreText").GetComponent<Text>();
      if (player1TurnsText == null || player1ScoreText == null) {
        Debug.LogError("Required text field(s) could not be identified.");
      }
      BrickController.OnGameOver(isGameOver);
      LimitController.OnGameOver(isGameOver);
      PaddleController.OnGameOver(isGameOver);
      player1TurnsText.text = PlayerCurrentTurn.ToString();
    }

    private void Update() {
      if (isHighScoreCanvasVisible) {
        Cursor.visible = true;
        return;
      }
      if (isGameOver) {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse0)) {
          if (Screen.fullScreen == false) {
            Screen.fullScreen = true;
          }
          if (Cursor.visible == true) {
            Cursor.visible = false;
          }
          // Restart game
          SceneManager.LoadScene(1);
        }
      }
    }

    private void OnDisable() {
      OnDisablingCollision -= DisableBrickIsTrigger;
      OnEnablingCollision -= EnableBrickIsTrigger;
      OnTurnEnd -= UpdateTurnsRemaining;
      OnBrickCollision -= GivePlayerPoints;
      OnSetHighScoreTable -= SetHighScoreCanvaseVisibility;
    }

    private void SetHighScoreCanvaseVisibility(bool isVisible) {
      isHighScoreCanvasVisible = isVisible;
    }

    private void GivePlayerPoints(int points) {
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
        isGameOver = true;
        GameController.OnSetHighScoreTable(true);
        PaddleController.OnGameOver(isGameOver);
        BallController.OnGameOver();
        BrickController.OnGameOver(isGameOver);
        LimitController.OnGameOver(isGameOver);
        TextBlink.OnBlink(false);
        HighScoreManager.OnHighScoreRoutine(PlayerPoints);
        return;
      }
    }
  }
}