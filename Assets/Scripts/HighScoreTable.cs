using System;
using System.Collections;
using System.Collections.Generic;
using Breakout.Web;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;

namespace Breakout.HighScore {
  public class HighScoreTable : MonoBehaviour {
    private Transform dataContainer;
    private Transform tableRowTemplate;
    private void Awake() {
      gameObject.SetActive(false);
    }

    private Transform CreateDataRow(int currentRowIndex) {
      float rowTemplateHeight = 50f;
      Transform rowTransform = Instantiate(tableRowTemplate, dataContainer);
      RectTransform rowRectTransform = rowTransform.GetComponent<RectTransform>();
      rowRectTransform.anchoredPosition = new Vector2(0, -rowTemplateHeight * currentRowIndex);
      rowTransform.gameObject.SetActive(true);
      return rowTransform;
    }

    public void LoadHighScoreData(Leaderboard leaderboardData) {
      Leaderboard leaderboard = leaderboardData;
      dataContainer = transform.Find("DataContainer");
      tableRowTemplate = dataContainer.Find("TableRowTemplate");
      if (dataContainer == null || tableRowTemplate == null) {
        Debug.LogWarning("The elements required to generate the highscore table are not present");
        return;
      }
      tableRowTemplate.gameObject.SetActive(false);
      for (int i = 0; i < leaderboard.LeaderboardEntryList.Count; i++) {
        LeaderboardEntry entry = leaderboard.LeaderboardEntryList[i];
        Transform createdRow = CreateDataRow(i);
        SetRankText(i, createdRow, entry.IsNewEntry);
        SetScoreText(createdRow, entry.Score, entry.IsNewEntry);
        SetNameText(createdRow, entry.Name, entry.IsNewEntry);
      }
      gameObject.SetActive(true);
    }

    // private void LoadHighScoreData() {
    //   WebRequests.Get("https://breakoutleaderboard-jeffreymillerdotdev.azurewebsites.net/api/GetLeaderboard?code=8OXrLAXIlCwkTpfOlEC-B_9o-Kq9ts4gV7aY-R0ZIWtAAzFuKIffiA==",
    //     (string error) => {
    //       gameObject.SetActive(false);
    //       Debug.LogError("Cannot load the High Score Table: " + error);
    //     },
    //     (string response) => {
    //       Leaderboard leaderboard = JsonConvert.DeserializeObject<Leaderboard>(response);
    //       dataContainer = transform.Find("DataContainer");
    //       tableRowTemplate = dataContainer.Find("TableRowTemplate");
    //       if (dataContainer == null || tableRowTemplate == null) {
    //         Debug.LogWarning("The elements required to generate the highscore table are not present");
    //         return;
    //       }
    //       tableRowTemplate.gameObject.SetActive(false);
    //       for (int i = 0; i < leaderboard.LeaderboardEntryList.Count; i++) {
    //         Transform createdRow = CreateDataRow(i);
    //         SetRankText(i, createdRow);
    //         SetScoreText(createdRow, leaderboard.LeaderboardEntryList[i].Score);
    //         SetNameText(createdRow, leaderboard.LeaderboardEntryList[i].Name);
    //       }
    //       gameObject.SetActive(true);
    //     });
    // }

    private void SetRankText(int rankIndex, Transform rowTransform, bool isNewEntry) {
      // Set Position Text
      int rank = rankIndex + 1;
      string rankString;
      switch (rank) {
        case 1: {
            rankString = "1st";
            break;
          }
        case 2: {
            rankString = "2nd";
            break;
          }
        case 3: {
            rankString = "3rd";
            break;
          }
        default: {
            rankString = rank + "th";
            break;
          }
      }
      TextMeshProUGUI textMesh = rowTransform.Find("Rank").GetComponent<TextMeshProUGUI>();
      textMesh.SetText(rankString);
      SetColorIfIsNewEntry(textMesh, isNewEntry);
    }

    private void SetScoreText(Transform rowTransform, int score, bool isNewEntry) {
      TextMeshProUGUI textMesh = rowTransform.Find("Score").GetComponent<TextMeshProUGUI>();
      textMesh.SetText(score.ToString().PadLeft(3, '0'));
      SetColorIfIsNewEntry(textMesh, isNewEntry);
    }

    private void SetNameText(Transform rowTransform, string name, bool isNewEntry) {
      TextMeshProUGUI textMesh = rowTransform.Find("Name").GetComponent<TextMeshProUGUI>();
      textMesh.SetText(name);
      SetColorIfIsNewEntry(textMesh, isNewEntry);
    }

    private void SetColorIfIsNewEntry(TextMeshProUGUI textMesh, bool isNewEntry) {
      if (isNewEntry) {
        textMesh.color = new Color32(61, 173, 29, 255);
      }
    }
  }
}