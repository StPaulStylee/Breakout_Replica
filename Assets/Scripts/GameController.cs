using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Breakout {
  public class GameController : MonoBehaviour {
    public delegate void OnBallEventHandler();
    public delegate void OnBrickEventHandler(int points);

    public static OnBallEventHandler OnDisablingCollision;
    public static OnBallEventHandler OnEnablingCollision;
    public static OnBallEventHandler OnTurnEnd;
    public static OnBrickEventHandler OnBrickCollision;
    [field:SerializeField]
    public int PlayerTurnsRemaining { get; private set; }
    public int PlayerPoints { get; private set; } = 0;

    private bool isBricksEnabled = false;
    [SerializeField]
    private GameObject[] bricks;

    void Start() {
      isBricksEnabled = true;
      bricks = GameObject.FindGameObjectsWithTag("Brick");
      OnDisablingCollision += DisableBrickIsTrigger;
      OnEnablingCollision += EnableBrickIsTrigger;
      OnTurnEnd += UpdateTurnsRemaining;
      OnBrickCollision += GivePlayerPoints;
    }

    public void GivePlayerPoints(int points) {
      PlayerPoints += points;
      Debug.Log(PlayerPoints);
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
      --PlayerTurnsRemaining;
      Debug.Log($"Turns Remaining: {PlayerTurnsRemaining}");
      if (PlayerTurnsRemaining <= 0) {
        PaddleController.OnGameOver();
        BallController.OnGameOver();
      }
    }
  }
}