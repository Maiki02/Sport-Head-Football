using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour 
{
    public static GameManager Instance { get; private set; }    
    private GameMode gameMode;
    private Results gameResult;

    private Team team1;
    private Team team2;
    private List<Goal> goalLogs = new List<Goal>();
    
    private void Awake() 
    {
        // Patrón Singleton
        if (Instance == null) 
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } 
        else 
        {
            Destroy(gameObject);
        }
    }
    
    public void SetGameMode(GameMode gameMode) 
    {
        this.gameMode = gameMode;
    }

    public GameMode GetGameMode() 
    {
        return this.gameMode;
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

    public List<Goal> GetGoalLogs() 
    {
        return goalLogs;
    }

    public void SetTeams(Team team1, Team team2)
    {
        this.team1 = team1;
        this.team2 = team2;
    }

    public string GetTeam1Name() 
    {
        return this.team1.GetTeamName();
    }

    public string GetTeam2Name() 
    {
        return this.team2.GetTeamName();
    }

    public void AddGoal(int scoreTeam1, int scoreTeam2, int time) 
    {
        goalLogs.Add(new Goal(scoreTeam1, scoreTeam2, time));
    }
}