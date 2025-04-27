using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalLogController: IResettable
{
    private List<Goal> goalLogs = new List<Goal>();

    public List<Goal> GetGoalLogs() 
    {
        return goalLogs;
    }

    public void AddGoal(int scoreTeam1, int scoreTeam2, int time) 
    {
        goalLogs.Add(new Goal(scoreTeam1, scoreTeam2, time));
    }
   
    public void Reset()
    {
        goalLogs.Clear();
    }

}
