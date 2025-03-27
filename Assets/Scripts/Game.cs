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
        Debug.Log("Constructor de Game");
        //No se ejecuta nunca si no lo llamo desde otra clase
    }

    private void Start(){

        this.InitializeTeams();

        this.scoreTeam1Text = GameObject.Find("ScoreTeam1").GetComponent<TextMeshProUGUI>();
        this.scoreTeam2Text = GameObject.Find("ScoreTeam2").GetComponent<TextMeshProUGUI>();
        this.timerText = GameObject.Find("Timer").GetComponent<TextMeshProUGUI>();
        this.ball = GameObject.Find("Ball").GetComponent<Ball>();
        
        this.StartGame();
    }

    private void Update(){
        this.scoreTeam1Text.text = this.team1.GetScore().ToString();
        this.scoreTeam2Text.text = this.team2.GetScore().ToString();

        this.UpdateTimer();

        this.timerText.text = this.GetLeftTime().ToString("0");
        
    }

    public void UpdateTimer(){
        if(this.isPlaying){
            this.timeToPlay += Time.deltaTime;
        }
    }

    private void InitializeTeams(){
        this.SetTeam1("Team 1", GameObject.Find("PlayerTeam1").GetComponent<Character>());
        this.SetTeam2("Team 2", GameObject.Find("PlayerTeam2").GetComponent<Character>());
    }

    public void StartGame(){
        this.isPlaying = true;
        this.timeToPlay = 0f;
        this.MAX_TIME_TO_PLAY = 120f;
    }

    private float GetLeftTime(){
        return this.MAX_TIME_TO_PLAY - this.timeToPlay;
    }


    /* Dada el lado del equipo, se incrementa el marcador del equipo, se reinicia la pelota y se reproduce el sonido del gol */
    public void GoalScored(TeamSide teamSide){
        if(teamSide == TeamSide.Team1){
            this.team1.AddScore();
        } else if(teamSide == TeamSide.Team2){
            this.team2.AddScore();
        }

        //TODO: Reproducir sonido de gol

        this.ball.ResetPosition();    
        this.team1.SetPositionCharacter(new Vector2(8,-2));
        this.team2.SetPositionCharacter(new Vector2(-8,-2));
    }

    private void SetTeam1(string teamName, Character character)
    {
        this.team1 = new Team(TeamSide.Team1, teamName, character);
    }

    private void SetTeam2(string teamName, Character character)
    {
        this.team2 = new Team(TeamSide.Team2, teamName, character);
    }
    
}
