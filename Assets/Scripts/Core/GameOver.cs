using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GameOver : MonoBehaviour 
{
    private TextMeshProUGUI nameTeam1Text;
    private TextMeshProUGUI nameTeam2Text;

    private TextMeshProUGUI statsTeam1Text;

    private TextMeshProUGUI statsTeam2Text;
    private TextMeshProUGUI resultText;
    private TextMeshProUGUI goalLogsText;

    private GameManager gameManager;

    void Start() 
    {
        this.gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        this.resultText = GameObject.FindGameObjectWithTag("ResultText").GetComponent<TextMeshProUGUI>();
        this.goalLogsText = GameObject.FindGameObjectWithTag("GoalLog").GetComponent<TextMeshProUGUI>();
        this.nameTeam1Text = GameObject.FindGameObjectWithTag("NameTeam1").GetComponent<TextMeshProUGUI>();
        this.nameTeam2Text = GameObject.FindGameObjectWithTag("NameTeam2").GetComponent<TextMeshProUGUI>();
        this.statsTeam1Text = GameObject.FindGameObjectWithTag("StatsTeam1").GetComponent<TextMeshProUGUI>();
        this.statsTeam2Text = GameObject.FindGameObjectWithTag("StatsTeam2").GetComponent<TextMeshProUGUI>(); 

        this.UpdateGameOverScreen();
    }

    private void UpdateGameOverScreen()
    {
        this.resultText.text = getResultText();
        this.nameTeam1Text.text = this.gameManager.GetTeam1().GetTeamName() + "\n" + this.gameManager.GetTeam1().GetScore();
        this.nameTeam2Text.text = this.gameManager.GetTeam2().GetTeamName() + "\n" + this.gameManager.GetTeam2().GetScore();
        this.statsTeam1Text.text = this.getStatsTeam1();
        this.statsTeam2Text.text = this.getStatsTeam2();
        this.goalLogsText.text = getGoalLogsText();

    }

    private string getResultText(){
        Results result = this.gameManager.GetGameResult();

        if (result == Results.TEAM1_WIN)
        {
            return "Ganador\n"+this.gameManager.GetTeam1().GetTeamName();
        }
        else if (result == Results.TEAM2_WIN)
        {
            return "Ganador\n"+this.gameManager.GetTeam2().GetTeamName();
        }
        else
        {
            return "Empate";
        }
    }

    private string getGoalLogsText()
    {
        string logs = "";
        foreach (Goal goal in gameManager.GetGoalLogs())
        {
            logs += $"{goal.ScoreTeam2} - {goal.ScoreTeam1} | {goal.Time} segundos\n";
        }
        return logs;
    }

    private string getStatsTeam1()
    {
        return getStatsByDictionary(this.gameManager.GetTeam1().GetCharacter().GetStats());
    }

    private string getStatsTeam2()
    {
        return getStatsByDictionary(this.gameManager.GetTeam2().GetCharacter().GetStats());
    }

    private string getStatsByDictionary(Dictionary<string, int> stats)
    {
        string statsText = "Estadísticas\n";
        foreach (KeyValuePair<string, int> stat in stats)
        {
            statsText += stat.Key + ": " + stat.Value + "\n";
        }
        return statsText;
    }
}
