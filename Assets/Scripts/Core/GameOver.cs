using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GameOver : MonoBehaviour 
{
    private TextMeshProUGUI gameOverText;
    private TextMeshProUGUI winnerText;
    private TextMeshProUGUI scoreText;
    private TextMeshProUGUI backToMenuText;

    private GameManager gameManager;

    void Start() 
    {
        this.gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        this.gameOverText = GameObject.Find("GameOver").GetComponent<TextMeshProUGUI>();
        this.winnerText = GameObject.Find("Winner").GetComponent<TextMeshProUGUI>();
        this.scoreText = GameObject.Find("Score").GetComponent<TextMeshProUGUI>();
        this.backToMenuText = GameObject.Find("BackToMenu").GetComponent<TextMeshProUGUI>();

        this.UpdateGameOverScreen();
    }

    private void UpdateGameOverScreen()
    {
        Results result = this.gameManager.GetGameResult();

        if (result == Results.Team1Win)
        {
            this.winnerText.text = "Team 1 Wins!";
        }
        else if (result == Results.Team2Win)
        {
            this.winnerText.text = "Team 2 Wins!";
        }
        else
        {
            this.winnerText.text = "It's a Draw!";
        }

        // Actualizar el texto de puntuación
        int scoreTeam1 = gameManager.GetTeam1().GetScore();
        int scoreTeam2 = gameManager.GetTeam2().GetScore();
        this.scoreText.text = $"Score: {scoreTeam1} - {scoreTeam2}";

        this.backToMenuText.text = "Back to Menu";
    }

}
