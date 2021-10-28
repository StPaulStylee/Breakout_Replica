using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitController : MonoBehaviour {
  [SerializeField]
  private AudioSource collisionSfx;

  private void Awake() {
    collisionSfx = GetComponent<AudioSource>();
    if (!collisionSfx) {
      Debug.LogError($"'{gameObject.name}' does not have an audio source");
    }
  }
  private void OnTriggerEnter2D(Collider2D collision) {
    if (collision.CompareTag("Ball")) {
      collisionSfx.Play();
    }
  }
}
