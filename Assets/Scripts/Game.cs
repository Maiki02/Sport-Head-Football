using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Game : MonoBehaviour
{
    private Team team1;
    private Team team2;

    private Ball ball;

    private int MAX_SCORE_TO_WIN = 7;
    private float MAX_TIME_TO_PLAY = 120f; //Tiempo en segundos

    private float timeToPlay=0f; //Cuenta el tiempo transcurrido de juego.
    private bool isPlaying;


    private TextMeshProUGUI scoreTeam1Text;
    private TextMeshProUGUI scoreTeam2Text;
    private TextMeshProUGUI timerText;

    public Game(Team team1, Team team2, Ball ball)
    {
        this.team1 = team1;
        this.team2 = team2;
        this.ball = ball;
        this.timeToPlay = 0f;
        this.isPlaying = false;
    }

    private void Start(){
        this.scoreTeam1Text = GameObject.Find("ScoreTeam1").GetComponent<TextMeshProUGUI>();
        this.scoreTeam2Text = GameObject.Find("ScoreTeam2").GetComponent<TextMeshProUGUI>();
        this.timerText = GameObject.Find("Timer").GetComponent<TextMeshProUGUI>();
        Debug.Log("Comienza el juego: "+ this.scoreTeam1Text);
        this.StartGame();
    }

    private void Update(){
        this.scoreTeam1Text.text = "0";
        this.scoreTeam2Text.text = "0";

        this.UpdateTimer();

        this.timerText.text = this.GetLeftTime().ToString("0");

        
    }

    public void UpdateTimer(){
        if(this.isPlaying){
            this.timeToPlay += Time.deltaTime;
        }
    }

    public void StartGame(){
        this.isPlaying = true;
        this.MAX_TIME_TO_PLAY = 120f;
    }

    private float GetLeftTime(){
        Debug.Log("Tiempo restante: "+ (MAX_TIME_TO_PLAY - this.timeToPlay) +
        " Tiempo transcurrido: "+ this.timeToPlay + 
        " Tiempo total: "+ MAX_TIME_TO_PLAY);
        return this.MAX_TIME_TO_PLAY - this.timeToPlay;
    }

    private void SetTeam1(string teamName, Character character)
    {
        this.team1 = new Team(teamName, character);
    }

    private void SetTeam2(string teamName, Character character)
    {
        this.team2 = new Team(teamName, character);
    }
    
}
