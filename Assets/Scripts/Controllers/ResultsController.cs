using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultsController: IResettable
{ 
    private Results gameResult;
    private Team team1;
    private Team team2;

    public void SetTeams(Team team1, Team team2)
    {
        this.team1 = team1;
        this.team2 = team2;
    }

    public string GetTeam1Name() 
    {
        return this.team1.GetTeamName();
    }
    
    public int GetTeam1Score() 
    {
        return this.team1.GetScore();
    }

    public Dictionary<string, int> GetStatsTeam1() 
    {
        return this.team1.GetCharacter().GetStats();
    }

    public string GetTeam2Name() 
    {
        return this.team2.GetTeamName();
    }

    public int GetTeam2Score() 
    {
        return this.team2.GetScore();
    }

    public Dictionary<string, int> GetStatsTeam2() 
    {
        return this.team2.GetCharacter().GetStats();
    }

    public void SetGameResult(Results gameResult) 
    {
        this.gameResult = gameResult;
        SceneManager.LoadScene("GameOver");
    }

    public Results GetGameResult() 
    {
        return this.gameResult;
    }

    public void Reset()
    {
        this.gameResult = Results.NONE;
        this.team1 = null;
        this.team2 = null;
    }
}
