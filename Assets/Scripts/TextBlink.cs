using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class TextBlink : MonoBehaviour
{
  Text text;
  [SerializeField]
  private float blinkRate;
  private bool stopBlinking;

  private void Start() {
    text = GetComponent<Text>();
    StartBlinking();
  }

  private void Update() {
    if (stopBlinking) {
      StopBlinking();
    }
  }

  IEnumerator Blink() {
    while(true) {
      switch(text.color.a.ToString()) {
        case "0":
          text.color = new Color(text.color.r, text.color.g, text.color.b, 1);
          yield return new WaitForSeconds(blinkRate);
          break;
        case "1":
          text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
          yield return new WaitForSeconds(blinkRate);
          break;
      }
    }
  }

  private void StartBlinking() {
    StartCoroutine("Blink");
  }

  private void StopBlinking() {
    StopCoroutine("Blink");
  }
}
