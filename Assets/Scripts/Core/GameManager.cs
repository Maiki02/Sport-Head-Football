using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour 
{
    public static GameManager Instance { get; private set; }    
    private GameMode gameMode;
    private Results gameResult;
    
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
}