using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace Breakout.Web {
  public class TestLeaderboard : MonoBehaviour {
    void Update() {
      if (Input.GetKeyDown(KeyCode.T)) {
        WebRequests.Get("https://breakoutleaderboard-jeffreymillerdotdev.azurewebsites.net/api/GetLeaderboard?code=8OXrLAXIlCwkTpfOlEC-B_9o-Kq9ts4gV7aY-R0ZIWtAAzFuKIffiA==",
        (string error) => {
          Debug.LogError(error);
        },
        (string response) => {
          Leaderboard leaderboard = JsonConvert.DeserializeObject<Leaderboard>(response);
          Debug.Log(leaderboard.LeaderboardEntryList[0].Name);
        });
      }

      if (Input.GetKeyDown(KeyCode.P)) {
        WebRequests.PostJson("https://breakoutleaderboard-jeffreymillerdotdev.azurewebsites.net/api/AddScore?code=5k0ne2Vsp0H_qpfPmPsR5BHZ8mvsTJ-dboOzdqFeMAO7AzFuFUHR5A==",
        "{}",
        (string error) => {
          Debug.LogError(error);
        },
        (string response) => {
          Debug.Log(response);
        });
      }
    }
  }
}
