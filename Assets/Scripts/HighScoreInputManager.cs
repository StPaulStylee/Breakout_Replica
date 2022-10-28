using System.Collections;
using System.Collections.Generic;
using Breakout.HighScore.Data;
using Breakout.Web;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;

namespace Breakout.HighScore {
  public class HighScoreInputManager : MonoBehaviour {
    public delegate void OnInputAddHandler(string input);
    public delegate void OnInputRemoveHandler();
    public delegate void OnInputSubmitHandler();
    public static OnInputAddHandler OnUserAddInput;
    public static OnInputRemoveHandler OnUserRemoveInput;
    public static OnInputSubmitHandler OnUserSubmitInput;
    [SerializeField] private InputOptionSO[] inputOptionsData;
    [SerializeField] private Transform inputOptionContainer;
    [SerializeField] private TextMeshProUGUI inputText;
    private void Awake() {
      if (inputOptionContainer == null) {
        inputOptionContainer = transform.Find("InputOptionContainer");
      }
      if (inputText == null) {
        inputText = transform.Find("InputSelected").GetComponentInChildren<TextMeshProUGUI>();
      }

      OnUserAddInput += SetInputText;
      OnUserRemoveInput += RemoveInputCharacter;
      OnUserSubmitInput += SubmitInput;

      foreach (InputOptionSO option in inputOptionsData) {
        GameObject inputOptionGO = Instantiate(option.InputOptionPrefab);
        inputOptionGO.transform.SetParent(inputOptionContainer);
        InputOption inputOptionScript = inputOptionGO.GetComponent<InputOption>();
        inputOptionScript.InputCharacter = option.InputCharacter;
        inputOptionScript.InputCharacterOverride = option.InputCharacterOverride;
        inputOptionScript.FontSize = option.FontSize;
        inputOptionScript.OptionType = option.OptionType;
        inputOptionScript.SetOptionText();
      }
    }

    private void OnDisable() {
      OnUserAddInput -= SetInputText;
      OnUserRemoveInput -= RemoveInputCharacter;
      OnUserSubmitInput -= SubmitInput;
    }

    private void SetInputText(string text) {
      if (inputText.text.Length >= 3) {
        // Play no-no noise
        return;
      }
      inputText.text += text;
    }

    private void RemoveInputCharacter() {
      if (inputText.text.Length == 0) {
        return;
      }
      inputText.text = inputText.text.Remove(inputText.text.Length - 1);
    }

    private void SubmitInput() {
      LeaderboardEntry entry = new LeaderboardEntry {
        Name = inputText.text,
        Score = 215
      };
      WebRequests.PostJson("https://breakoutleaderboard-jeffreymillerdotdev.azurewebsites.net/api/AddScore?code=5k0ne2Vsp0H_qpfPmPsR5BHZ8mvsTJ-dboOzdqFeMAO7AzFuFUHR5A==",
      JsonConvert.SerializeObject(entry),
        (string error) => {
          Debug.LogError(error);
        },
        (string response) => {
          Debug.Log(response);
        });
    }
  }
}
