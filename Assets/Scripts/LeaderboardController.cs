using System.Collections;
using System.Collections.Generic;
using Breakout;
using Breakout.Web;
using Newtonsoft.Json;
using UnityEngine;

namespace Breakout.HighScore {
  public static class LeaderboardController {
    public static Leaderboard GetAllLeaderboardData() {
      Leaderboard leaderboard = null;
      WebRequests.Get("https://breakoutleaderboard-jeffreymillerdotdev.azurewebsites.net/api/GetLeaderboard?code=8OXrLAXIlCwkTpfOlEC-B_9o-Kq9ts4gV7aY-R0ZIWtAAzFuKIffiA==",
      (string error) => {
        Debug.LogError(error);
      },
      (string response) => {
        leaderboard = JsonConvert.DeserializeObject<Leaderboard>(response);
      });
      return leaderboard;
    }
  }
}
