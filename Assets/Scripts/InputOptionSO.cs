using System.Collections;
using System.Collections.Generic;
using Breakout.HighScore.Data;
using UnityEngine;

namespace Breakout.HighScore {
  [CreateAssetMenu(fileName = "InputOption_SO", menuName = "New Input Option")]
  public class InputOptionSO : ScriptableObject {
    public GameObject InputOptionPrefab;
    public char InputCharacter;
    public string InputCharacterOverride;
    public float FontSize = 244;
    public InputOptionType OptionType = InputOptionType.Add;
  }
}
