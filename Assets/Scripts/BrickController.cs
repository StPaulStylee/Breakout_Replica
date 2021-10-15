using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Breakout {
  public class BrickController : MonoBehaviour {
    [SerializeField]
    private int Points;

    private void OnTriggerEnter2D(Collider2D collision) {
      if (collision.CompareTag("Ball")) {
        GameController.OnBrickCollision(Points);
        gameObject.SetActive(false);
      }
    }
  }

}