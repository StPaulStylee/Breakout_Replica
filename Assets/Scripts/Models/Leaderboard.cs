using System.Collections.Generic;
namespace Breakout.HighScore {
  public class Leaderboard {
    public List<LeaderboardEntry> LeaderboardEntryList;
    public bool HasError = false;
  }

  public class LeaderboardEntry {
    public string Name;
    public int Score;
    public bool IsNewEntry;
  }
}
