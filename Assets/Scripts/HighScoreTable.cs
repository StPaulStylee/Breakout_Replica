using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Breakout {
  public class HighScoreTable : MonoBehaviour {
    private Transform dataContainer;
    private Transform tableRowTemplate;
    private void Awake() {
      dataContainer = transform.Find("DataContainer");
      tableRowTemplate = dataContainer.Find("TableRowTemplate");
      if (dataContainer == null || tableRowTemplate == null) {
        Debug.LogWarning("The elements required to generate the highscore table are not present");
        return;
      }
      tableRowTemplate.gameObject.SetActive(false);
      for (int i = 0; i < 10; i++) {
        Transform createdRow = CreateDataRow(i);
        SetRankText(i, createdRow);
        SetScoreText(createdRow);
      }
    }

    private Transform CreateDataRow(int currentRowIndex) {
      float rowTemplateHeight = 50f;
      Transform rowTransform = Instantiate(tableRowTemplate, dataContainer);
      RectTransform rowRectTransform = rowTransform.GetComponent<RectTransform>();
      rowRectTransform.anchoredPosition = new Vector2(0, -rowTemplateHeight * currentRowIndex);
      rowTransform.gameObject.SetActive(true);
      return rowTransform;
    }

    private void SetRankText(int rankIndex, Transform rowTransform) {
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
      rowTransform.Find("Rank").GetComponent<TextMeshProUGUI>().SetText(rankString);
    }

    private void SetScoreText(Transform rowTransform) {
      int score = Random.Range(0, 897);
      rowTransform.Find("Score").GetComponent<TextMeshProUGUI>().SetText(score.ToString().PadLeft(3, '0'));
    }
  }
}
