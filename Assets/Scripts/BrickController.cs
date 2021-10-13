using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Breakout {
  public class BrickController : MonoBehaviour {
    public int Points;

    private void OnTriggerEnter2D(Collider2D collision) {
      if (collision.CompareTag("Ball")) {
        gameObject.SetActive(false);
        Debug.Log(Points);
      }
    }
  }

}