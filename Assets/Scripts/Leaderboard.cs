using System.Collections.Generic;
namespace Breakout {
  public class Leaderboard {
    public List<LeaderboardEntry> LeaderboardEntryList;
  }

  public class LeaderboardEntry {
    public string Name;
    public int Score;
  }
}