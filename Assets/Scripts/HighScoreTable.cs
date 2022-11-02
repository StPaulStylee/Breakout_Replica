using System;
using System.Collections;
using System.Collections.Generic;
using Breakout.Web;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;

namespace Breakout.HighScore {
  public class HighScoreTable : MonoBehaviour {
    [Tooltip("The amount of time the Highscore table will be visible. During this time the user has no input ability")]
    [SerializeField]
    int visibleFor = 5;
    private Transform dataContainer;
    private Transform tableRowTemplate;
    private void Awake() {
      gameObject.SetActive(false);
    }

    private void Deactivate() {
      gameObject.SetActive(false);
      GameController.OnSetHighScoreTable(false);
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
      Invoke("Deactivate", visibleFor);
    }

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
