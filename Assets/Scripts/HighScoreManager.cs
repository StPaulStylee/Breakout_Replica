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
      // Set the dependencies and check if null
      OnHighScoreRoutine += GetHighScoreData;
    }

    private void OnDisable() {
      OnHighScoreRoutine -= GetHighScoreData;
    }

    private void GetHighScoreData(int playerScore) {
      WebRequests.Get("https://breakoutleaderboard-jeffreymillerdotdev.azurewebsites.net/api/GetLeaderboard?code=8OXrLAXIlCwkTpfOlEC-B_9o-Kq9ts4gV7aY-R0ZIWtAAzFuKIffiA==",
        (string error) => {
          leaderboard = new Leaderboard { HasError = true };
          gameObject.SetActive(false);
          Debug.LogError("Cannot load the High Score Table: " + error);
        },
        (string response) => {
          leaderboard = JsonConvert.DeserializeObject<Leaderboard>(response);
          // Check players score to see if they achieved a highscore
          bool isHighScore = leaderboard.LeaderboardEntryList.TrueForAll((entry) => !(entry.Score > playerScore));
          if (isHighScore) {
            print("High Score Achieved!");
            highScoreInputManager.gameObject.SetActive(true);
            return;
          }
          print("No high score!");
          highScoreTable.LoadHighScoreData(leaderboard);
          // once captured, set entry to IsNewEntry, add highscore to leaderboard data and sort
        });
    }
  }
}
