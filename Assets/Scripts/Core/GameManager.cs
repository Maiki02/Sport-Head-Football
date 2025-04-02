using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour 
{
    public static GameManager Instance { get; private set; }    
    private GameMode gameMode;
    
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
}