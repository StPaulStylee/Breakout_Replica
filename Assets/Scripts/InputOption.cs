using System;
using System.Collections;
using System.Collections.Generic;
using Breakout.HighScore.Data;
using TMPro;
using UnityEngine;

namespace Breakout.HighScore {
  public class InputOption : MonoBehaviour {
    public char InputCharacter { get; set; }
    public string InputCharacterOverride { get; set; }
    public float FontSize { get; set; }
    [field: SerializeField] public InputOptionType OptionType { get; set; }
    [SerializeField] private TextMeshProUGUI optionText;
    private void Awake() {
      optionText = GetComponentInChildren<TextMeshProUGUI>();
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

    public void OnClickHandler() {
      if (OptionType == InputOptionType.Add) {
        string input = InputCharacterOverride != String.Empty ? InputCharacterOverride : InputCharacter.ToString();
        Debug.Log(input);
        HighScoreInputManager.OnUserAddInput(input);
        return;
      }
      if (OptionType == InputOptionType.Remove) {
        HighScoreInputManager.OnUserRemoveInput();
      }
      if (OptionType == InputOptionType.Submit) {
        // Do stuff
        HighScoreInputManager.OnUserSubmitInput();
      }
    }
  }
}
