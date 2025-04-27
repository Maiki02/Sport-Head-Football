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

    private GameController gameController;

    void Start() 
    {
        this.gameController = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameController>();
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
        this.nameTeam1Text.text = gameController.ResultsController.GetTeam1Name() + "\n" + gameController.ResultsController.GetTeam1Score();
        this.nameTeam2Text.text = gameController.ResultsController.GetTeam2Name() + "\n" + gameController.ResultsController.GetTeam2Score();
        this.statsTeam1Text.text = this.getStatsTeam1();
        this.statsTeam2Text.text = this.getStatsTeam2();
        this.goalLogsText.text = getGoalLogsText();

    }

    private string getResultText(){
        Results result = gameController.ResultsController.GetGameResult();

        if (result == Results.TEAM1_WIN)
        {
            return "Ganador\n"+gameController.ResultsController.GetTeam1Name();
        }
        else if (result == Results.TEAM2_WIN)
        {
            return "Ganador\n"+gameController.ResultsController.GetTeam2Name();
        }
        else
        {
            return "Empate";
        }
    }

    private string getGoalLogsText()
    {
        string logs = "";
        foreach (Goal goal in gameController.GoalLogController.GetGoalLogs())
        {
            logs += $"{goal.ScoreTeam2} - {goal.ScoreTeam1} | {goal.Time} segundos\n";
        }
        return logs;
    }

    private string getStatsTeam1()
    {
        return getStatsByDictionary(gameController.ResultsController.GetStatsTeam1());
    }

    private string getStatsTeam2()
    {
        return getStatsByDictionary(gameController.ResultsController.GetStatsTeam2());
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
