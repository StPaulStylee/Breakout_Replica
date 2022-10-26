using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Breakout.HighScore {
  [CreateAssetMenu(fileName = "InputOption_SO", menuName = "New Input Option")]
  public class InputOptionSO : ScriptableObject {
    // <- 159
    // Enter 61.5
    public GameObject InputOptionPrefab;
    public char InputCharacter;
    public string InputCharacterOverride;
    public float FontSize = 244;
  }
}
