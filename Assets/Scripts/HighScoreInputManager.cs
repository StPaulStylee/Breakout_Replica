using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Breakout.HighScore {
  public class HighScoreInputManager : MonoBehaviour {
    [SerializeField] private InputOptionSO[] inputOptionsData;
    [SerializeField] private Transform inputOptionContainer;
    private void Awake() {
      if (inputOptionContainer == null) {
        inputOptionContainer = transform.Find("InputOptionContainer");
      }
      foreach (InputOptionSO option in inputOptionsData) {
        GameObject inputOptionGO = Instantiate(option.InputOptionPrefab);
        inputOptionGO.transform.SetParent(inputOptionContainer);
        InputOption inputOptionScript = inputOptionGO.GetComponent<InputOption>();
        inputOptionScript.InputCharacter = option.InputCharacter;
        inputOptionScript.InputCharacterOverride = option.InputCharacterOverride;
        inputOptionScript.FontSize = option.FontSize;
        inputOptionScript.SetOptionText();
      }
    }
  }
}
