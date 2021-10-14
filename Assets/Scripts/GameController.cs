using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Breakout {
  public class GameController : MonoBehaviour {
    public delegate void OnBallEventHandler();
    public static OnBallEventHandler OnBrickCollision;
    public static OnBallEventHandler OnEnablingCollision;
    public static OnBallEventHandler OnTurnEnd;
    [field:SerializeField]
    public int PlayerTurnsRemaining { get; private set; }
    private bool isBricksEnabled = false;
    [SerializeField]
    private GameObject[] bricks;

    void Start() {
      isBricksEnabled = true;
      bricks = GameObject.FindGameObjectsWithTag("Brick");
      OnBrickCollision += DisableBrickIsTrigger;
      OnEnablingCollision += EnableBrickIsTrigger;
      OnTurnEnd += UpdateTurnsRemaining;
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
    }
  }
}