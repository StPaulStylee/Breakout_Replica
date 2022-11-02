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
    public int PlayerScore { private get; set; }
    private Leaderboard leaderboard = null;
    [SerializeField] private HighScoreManager highScoreManager;
    [SerializeField] private InputOptionSO[] inputOptionsData;
    [SerializeField] private Transform inputOptionContainer;
    [SerializeField] private TextMeshProUGUI inputText;
    // public int PlayerScore { private get; set; }
    private void Awake() {
      ExtractDependencies();

      OnUserAddInput += SetInputText;
      OnUserRemoveInput += RemoveInputCharacter;
      OnUserSubmitInput += SubmitHighScore;

      CreateInputOptions();
    }

    private void CreateInputOptions() {
      foreach (InputOptionSO option in inputOptionsData) {
        GameObject inputOptionGO = Instantiate(option.InputOptionPrefab);
        inputOptionGO.transform.SetParent(inputOptionContainer);
        inputOptionGO.transform.localScale = new Vector3(1f, 1f, 1f);
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
      OnUserSubmitInput -= SubmitHighScore;
    }

    private void ExtractDependencies() {
      if (highScoreManager == null) {
        highScoreManager = transform.parent.GetComponent<HighScoreManager>();
      }
      if (inputOptionContainer == null) {
        inputOptionContainer = transform.Find("InputOptionContainer");
      }
      if (inputText == null) {
        inputText = transform.Find("InputSelected").GetComponentInChildren<TextMeshProUGUI>();
      }
    }

    // I've made a mess of this. Right now the HighScoreManager is trying to
    // call a routine that opens the input manager, submits the score, and then
    // shows the leaderboard with all of the data (including the newest entry)
    // however, onclick submit does the exact some routine and I don't think that is 
    // going to work. I need to rethink how the HighScoreManager will call the
    // post request to set up the new data and then how the highscore board
    // gets presented.
    public void SubmitHighScore() {
      // Do I need to clear any instance data after submission?
      LeaderboardEntry entry = new LeaderboardEntry {
        Name = inputText.text,
        Score = PlayerScore,
        IsNewEntry = true
      };
      SubmitInput(entry);
      highScoreManager.LoadLeaderboardGUI(entry);
      gameObject.SetActive(false);
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

    private void SubmitInput(LeaderboardEntry entry) {
      WebRequests.PostJson("https://breakoutleaderboard-jeffreymillerdotdev.azurewebsites.net/api/AddScore?code=5k0ne2Vsp0H_qpfPmPsR5BHZ8mvsTJ-dboOzdqFeMAO7AzFuFUHR5A==",
        JsonConvert.SerializeObject(entry),
          (string error) => {
            // Determine the workflow here if an error does occur
            leaderboard = new Leaderboard { HasError = true };
            Debug.LogError(error);
          },
          (string response) => {
            leaderboard = JsonConvert.DeserializeObject<Leaderboard>(response);
            Debug.Log(response);
          });
    }
  }
}
