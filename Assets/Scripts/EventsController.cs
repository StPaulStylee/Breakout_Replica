using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Breakout {
  public class EventsController : MonoBehaviour {
    public delegate void OnBallEventHandler();
    public static OnBallEventHandler OnBrickCollision;
    public static OnBallEventHandler OnEnablingCollision;

    [SerializeField]
    private GameObject[] bricks;

    void Start() {
      bricks = GameObject.FindGameObjectsWithTag("Brick");
      OnBrickCollision += DisableBrickIsTrigger;
      OnEnablingCollision += EnableBrickIsTrigger;
    }

    private void DisableBrickIsTrigger() {
      foreach (GameObject brick in bricks) {
        if (brick != null) {
          Collider2D collider = brick.GetComponent<Collider2D>();
          collider.enabled = false;
        }
      }
    }

    private void EnableBrickIsTrigger() {
      foreach (GameObject brick in bricks) {
        if (brick != null) {
          Collider2D collider = brick.GetComponent<Collider2D>();
          collider.enabled = true;
        }
      }
    }

  }
}