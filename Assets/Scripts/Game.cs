using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Game : MonoBehaviour
{
    private Team team1;
    private Team team2;

    private Ball ball;

    private const int MAX_SCORE_TO_WIN = 7;
    private const float MAX_TIME_TO_PLAY = 60f; //Tiempo en segundos

    private float timeToPlay=0f; //Cuenta el tiempo transcurrido de juego.
    private bool isPlaying=false; //Indica si el juego está en curso o no.

    private float timeToStartCounter = 3f; //Tiempo de espera para iniciar el juego
    private float timeToCelebrateGoal = 2f; //Tiempo de espera para celebrar el gol

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
    }

    private void Update(){
        
        //Si el juego está terminado, cambiamos de pantalla TODO
        this.UpdateGameOver();

        //Si el contador de tiempo para iniciar el juego, es mayor a 0, se decrementa el contador
        //y se inicia el juego cuando llega a 0
        this.UpdateStartGame();

        //Si el juego está en curso, se actualiza el tiempo
        this.UpdateTimer();

        //Se actualiza el marcador de los equipos
        this.UpdateBoardMarker();

        //Se actualiza el tiempo de celebración en caso de que no se esté jugando
        this.UpdateTimerCelebration();
    
    }

    public void UpdateTimer(){
        if(this.isPlaying){
            this.timeToPlay += Time.deltaTime;
            this.timerText.text = this.GetLeftTime().ToString("0");
        }
    }

    public void UpdateBoardMarker(){
        this.scoreTeam1Text.text = this.team1.GetScore().ToString();
        this.scoreTeam2Text.text = this.team2.GetScore().ToString();
    }


    //Actualiza la condición para que el juego finalice.
    public void UpdateGameOver(){
        if(this.IsGameOver()){
            this.isPlaying = false;
            this.timeToStartCounter = 0f;
        }
    }
    public void UpdateStartGame(){
        if(this.timeToStartCounter > 0f){
            this.timeToStartCounter -= Time.deltaTime;
            this.timerText.text = this.timeToStartCounter.ToString("0");
            if(this.timeToStartCounter <= 0f){
                this.StartGame();
            }
        }
    }

    public void UpdateTimerCelebration(){
        if(!IsTimeToCelebrateGoal()) return;

        if(this.timeToCelebrateGoal > 0f){
            this.timeToCelebrateGoal -= Time.deltaTime;
            if(this.timeToCelebrateGoal <= 0f){
                this.ResetAllPositions();
            }
        }
    }


    private void InitializeTeams(){
        this.SetTeam1("Team 1", GameObject.Find("PlayerTeam1").GetComponent<Character>());
        this.SetTeam2("Team 2", GameObject.Find("PlayerTeam2").GetComponent<Character>());
    }

    public void StartGame(){
        this.timeToPlay = 0f;
        this.isPlaying = true;
        //this.ResetAllPositions(); //Creo que no es necesario porque ya estarían reiniciados después del gol e imposibles de mover.
    }

    private float GetLeftTime(){
        return MAX_TIME_TO_PLAY - this.timeToPlay;
    }

    public bool IsGameOver(){
        return this.team1.GetScore() >= MAX_SCORE_TO_WIN || 
        this.team2.GetScore() >= MAX_SCORE_TO_WIN ||
        this.GetLeftTime() <= 0f;
    }

    public bool IsTimeToCelebrateGoal(){
        return !this.isPlaying;
    }

    public void ResetTimeToCelebrateGoal(){
        this.timeToCelebrateGoal = 2f;
    }

    /* Dada el lado del equipo, se incrementa el marcador del equipo, 
    se reproduce el sonido del gol y comienza la celebración */
    public void GoalScored(TeamSide teamSide){
        this.ScoredGoalByTeam(teamSide);

        this.PlayGoalSound();

        this.ResetTimeToCelebrateGoal();
        this.isPlaying = false; //Con esta variable, activamos el tiempo de celebración
    }

    public void ScoredGoalByTeam(TeamSide teamSide){
        switch(teamSide){
            case TeamSide.Team1:
                this.team1.AddScore();
                break;
            case TeamSide.Team2:
                this.team2.AddScore();
                break;
            default:
                Debug.Log("Equipo no encontrado");
                break;
        }
    }

    public void PlayGoalSound(){
        //TODO buscar un sonido de gol y colocarlo para reproducir.
        //GameObject.Find("GoalSound").GetComponent<AudioSource>().Play();
    }

    public void ResetAllPositions(){
        this.ball.ResetPosition();
        this.team1.SetPositionCharacter(new Vector2(8,-2));
        this.team2.SetPositionCharacter(new Vector2(-8,-2));
        this.timeToStartCounter = 3f; //Reinicia el contador de tiempo para iniciar el juego
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
