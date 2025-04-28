using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Game : MonoBehaviour
{
    private Team team1;
    private Team team2;

    private Ball ball;

    private GameController gameController;

    //Prefabs del jugador CPU y humano
    [SerializeField] private GameObject cpuPrefab; //Prefab del CPU
    [SerializeField] private GameObject humanPrefab; //Prefab del humano


    private const int MAX_SCORE_TO_WIN = 7;
    private const float MAX_TIME_TO_PLAY = 60f; //Tiempo en segundos

    private float timeToPlay=0f; //Cuenta el tiempo transcurrido de juego.
    private bool isPlaying=false; //Indica si el juego está en curso o no.

    private float timeToStartCounter = 3f; //Tiempo de espera para iniciar el juego
    private float timeToCelebrateGoal = 2f; //Tiempo de espera para celebrar el gol

    private List<IScoreObserver> observers = new List<IScoreObserver>();

    private TextMeshProUGUI timerText;
    private TextMeshProUGUI startCounterText;

    private void Start(){
        gameController = GameController.Instance;
        this.timerText = GameObject.FindWithTag("Timer").GetComponent<TextMeshProUGUI>();
        this.startCounterText = GameObject.FindWithTag("StartCounter").GetComponent<TextMeshProUGUI>();
        this.ball = GameObject.FindWithTag("Ball").GetComponent<Ball>();

        this.cpuPrefab = Resources.Load<GameObject>("PlayerTeam2 (CPU)"); // Cargamos un prefab del CPU
        this.humanPrefab = Resources.Load<GameObject>("PlayerTeam2 (Human)"); // Cargamos un prefab del humano


        this.StartGame(gameController.GameModeController.GetGameMode());
    }

    private void Update(){
        
        //Si el juego está terminado, cambiamos de pantalla TODO
        this.UpdateGameOver();

        //Si el contador de tiempo para iniciar el juego, es mayor a 0, se decrementa el contador
        //y se inicia el juego cuando llega a 0
        this.UpdateStartGame();

        //Si el juego está en curso, se actualiza el tiempo
        this.UpdateTimer();

        //Se actualiza el tiempo de celebración en caso de que no se esté jugando
        this.UpdateTimerCelebration();
    
    }

    public void UpdateTimer(){
        if(this.isPlaying){
            this.timeToPlay += Time.deltaTime;
            this.timerText.text = this.GetLeftTime().ToString("0");
        }
    }

    //Actualiza la condición para que el juego finalice.
    public void UpdateGameOver(){
        if(this.IsGameOver()){
            this.FinishGame();
        }
    }
    public void UpdateStartGame(){
        if(this.timeToStartCounter > 0f){
            this.timeToStartCounter -= Time.deltaTime;
            this.startCounterText.text = this.timeToStartCounter.ToString("0");
            if(this.timeToStartCounter <= 0f){
                this.SetIsPlaying(true);
                this.ball.SetFreezeBall(false); //Descongelamos la pelota para que se pueda mover
            }
        } else {
            this.startCounterText.text = "";
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


    private void InitializeTeams(GameMode gameMode){
        //Seteamos player 1
        this.SetTeam1("Inter Miami", GameObject.FindWithTag("Player").GetComponent<Character>());
        
        //Seteamos player 2 en base a la selección del menú
        //Cargamos el prefab "PlayerTeam2 (CPU)" o "PlayerTeam2 (Human)" en base al modo de juego seleccionado
        //Luego seteamos el equipo 2
        GameObject playerTeam2GameObject = Instantiate(this.GetPrefabByGameMode(gameMode), new Vector2(-8, -2), Quaternion.identity);
        this.SetTeam2("Barcelona", playerTeam2GameObject.GetComponent<Character>());

    }

    public void StartGame(GameMode gameMode){
        this.InitializeTeams(gameMode); //Inicializamos los equipos.
        this.ResetTimeToStartCounter(); //Reinicia el contador de tiempo para iniciar el juego
    }

    public void FinishGame(){
        this.isPlaying = false;
        this.timeToStartCounter = 0f;
        gameController.ResultsController.SetGameResult(this.GetWinner()); //Seteamos el resultado del juego en el GameController
        gameController.ResultsController.SetTeams(this.team1, this.team2); //Seteamos el nombre del equipo 1 en el GameController
    }

    public Results GetWinner(){
        if(this.team1.GetScore() > this.team2.GetScore()){
            return Results.TEAM1_WIN;
        } else if(this.team1.GetScore() < this.team2.GetScore()){
            return Results.TEAM2_WIN;
        } else if(this.team1.GetScore() == this.team2.GetScore()){
            return Results.DRAW;
        } else {
            return Results.NONE;
        }
    }

    public float GetTimeToStartCounter(){
        return this.timeToStartCounter;
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

    /* Dado el equipo que anotó, se incrementa el marcador del equipo, 
    se reproduce el sonido del gol y comienza la celebración */
    public void GoalScored(TeamSide teamSide){
        if(!this.isPlaying) return; //Si no se está jugando, no se incrementa el marcador
        //Esto es porque puede suceder que la pelota entre y salga mientras se está festejando el gol

        this.ScoredGoalByTeam(teamSide);

        //Agregamos el gol al log de goles
        gameController.GoalLogController.AddGoal(this.team1.GetScore(), this.team2.GetScore(), (int)this.timeToPlay); 

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

        this.NotifyObservers(); //Notificamos a los observadores que el marcador ha cambiado
    }

    public void PlayGoalSound(){
        //TODO buscar un sonido de gol y colocarlo para reproducir.
        //GameObject.Find("GoalSound").GetComponent<AudioSource>().Play();
    }

    public void ResetAllPositions(){
        this.ball.ResetPosition();
        this.team1.SetPositionCharacter(new Vector2(8,-2));
        this.team2.SetPositionCharacter(new Vector2(-8,-2));
        this.ResetTimeToStartCounter(); //Reinicia el contador de tiempo para iniciar el juego
    }

    private void SetTeam1(string teamName, Character character)
    {
        this.team1 = new Team(TeamSide.Team1, teamName, character);
    }

    private void SetTeam2(string teamName, Character character)
    {
        this.team2 = new Team(TeamSide.Team2, teamName, character);
    }

    public GameObject GetPrefabByGameMode(GameMode gameMode){
        if(gameMode == GameMode.ONE_PLAYER){
            return this.cpuPrefab;
        } else if(gameMode == GameMode.TWO_PLAYERS) {
            return this.humanPrefab;
        } else {
            Debug.LogError("Modo de juego no encontrado");
            return null;
        }
    }

    public void SetIsPlaying(bool value){
        this.isPlaying = value;
    }

    public void ResetTimeToCelebrateGoal(){
        this.timeToCelebrateGoal = 2f;
    }

    public void ResetTimeToStartCounter(){
        this.timeToStartCounter = 3f;
    }



    //------- OBSERVER LOGIC -----------\\
    public void AddObserver(IScoreObserver observer)
    {
        observers.Add(observer);
    }

    public void RemoveObserver(IScoreObserver observer)
    {
        observers.Remove(observer);
    }

    private void NotifyObservers()
    {
        foreach (var observer in observers)
        {
            observer.OnScoreChanged(this.team1.GetScore(), this.team2.GetScore());
        }
    }
}
