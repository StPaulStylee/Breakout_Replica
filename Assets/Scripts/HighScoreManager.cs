using System.Collections;
using System.Collections.Generic;
using Breakout.Web;
using Newtonsoft.Json;
using UnityEngine;

namespace Breakout.HighScore {
  public class HighScoreManager : MonoBehaviour {
    // Do I need a boolean?
    public delegate void OnGameOver(int playerScore);
    public static OnGameOver OnHighScoreRoutine;
    [SerializeField] private HighScoreTable highScoreTable;
    [SerializeField] private HighScoreInputManager highScoreInputManager;
    private Leaderboard leaderboard = null;

    private void Awake() {
      Debug.Log("HighScoreManager awake");
      GetHighScoreData();
      // Set the dependencies and check if null
      OnHighScoreRoutine += HighScoreInputRoutine;
    }

    private void OnDisable() {
      OnHighScoreRoutine -= HighScoreInputRoutine;
    }

    public void LoadLeaderboardGUI(LeaderboardEntry entry) {
      leaderboard.LeaderboardEntryList.Add(entry);
      SortLeaderboardDescending();
      highScoreTable.LoadHighScoreData(leaderboard);
    }

    private void LoadLeaderboardGUI() {
      SortLeaderboardDescending();
      // There is no need to ensure "IsNewEntry" is false when a new leaderboard
      // is rendered because the backend doesn't have a "IsNewEntry" field and therefor
      // is simply ignored when added to the DB
      highScoreTable.LoadHighScoreData(leaderboard);
    }

    private void GetHighScoreData() {
      WebRequests.Get("https://breakoutleaderboard-jeffreymillerdotdev.azurewebsites.net/api/GetLeaderboard?code=8OXrLAXIlCwkTpfOlEC-B_9o-Kq9ts4gV7aY-R0ZIWtAAzFuKIffiA==",
        (string error) => {
          leaderboard = new Leaderboard { HasError = true };
          gameObject.SetActive(false);
          Debug.LogError("Cannot load the High Score Table: " + error);
        },
        (string response) => {
          leaderboard = JsonConvert.DeserializeObject<Leaderboard>(response);
        });
    }

    private void HighScoreInputRoutine(int playerScore) {
      bool isHighScore = GetIsHighScore(playerScore);
      if (isHighScore) {
        highScoreInputManager.PlayerScore = playerScore;
        highScoreInputManager.gameObject.SetActive(true);
        return;
      }
      LoadLeaderboardGUI();
    }

    private bool GetIsHighScore(int playerScore) {
      if (leaderboard.LeaderboardEntryList.Count <= 10) {
        return true;
      }
      return !leaderboard.LeaderboardEntryList.TrueForAll((entry) => entry.Score >= playerScore);
    }

    private void SortLeaderboardDescending() {
      leaderboard.LeaderboardEntryList.Sort((x, y) => y.Score.CompareTo(x.Score));
    }
  }
}
