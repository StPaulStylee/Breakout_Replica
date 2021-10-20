using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class TextBlink : MonoBehaviour {
  public delegate void OnBlinkHandler(bool enableBlink);
  public static OnBlinkHandler OnBlink;
  Text text;
  [SerializeField]
  private float blinkRate;
  
  private void Awake() {
    OnBlink += EnableBlinking;
  }

  private void Start() {
    text = GetComponent<Text>();
    StartBlinking();
  }

  private IEnumerator Blink() {
    while (true) {
      switch (text.color.a.ToString()) {
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

  private void EnableBlinking(bool enableBlink) {
    if (enableBlink) {
      StartBlinking();
    }
    StopBlinking();
  }

  public void StartBlinking() {
    StartCoroutine("Blink");
  }

  public void StopBlinking() {
    StopCoroutine("Blink");
  }
}
