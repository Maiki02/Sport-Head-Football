using UnityEngine;

public class Goal {
    public int ScoreTeam1 { get; }
    public int ScoreTeam2 { get; }
    public int Time { get; }

    public Goal(int scoreTeam1, int scoreTeam2, int time) {
        this.ScoreTeam1 = scoreTeam1;
        this.ScoreTeam2 = scoreTeam2;
        this.Time = time;
    }
}