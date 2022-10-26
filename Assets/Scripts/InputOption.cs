using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Breakout.HighScore {
  public class InputOption : MonoBehaviour {
    public char InputCharacter { get; set; }
    public string InputCharacterOverride { get; set; }
    public float FontSize { get; set; }
    [SerializeField] private TextMeshProUGUI optionText;
    private void Awake() {
      optionText = GetComponentInChildren<TextMeshProUGUI>();
      //   SetOptionText();
    }

    public void SetOptionText() {
      optionText.fontSize = FontSize;
      if (InputCharacterOverride != String.Empty) {
        optionText.SetText(InputCharacterOverride);
        return;
      }
      optionText.SetText(InputCharacter.ToString());
      return;
    }
  }
}
