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

        this.UpdateGameOverScreen();
    }

    private void UpdateGameOverScreen()
    {
        this.resultText.text = getResultText();
        this.nameTeam1Text.text = this.gameManager.GetTeam1Name();
        this.nameTeam2Text.text = this.gameManager.GetTeam2Name();
        this.goalLogsText.text = getGoalLogsText();

    }

    private string getResultText(){
        Results result = this.gameManager.GetGameResult();

        if (result == Results.TEAM1_WIN)
        {
            return "Ganador\n"+this.gameManager.GetTeam1Name();
        }
        else if (result == Results.TEAM2_WIN)
        {
            return "Ganador\n"+this.gameManager.GetTeam2Name();
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
}
