using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    GameController gameController;

    void Start()
    {
        gameController = GameController.Instance;  
    }
    public void PlayGame()
    {
        Debug.Log("PlayGame");
        gameController.GameModeController.SetGameMode(GameMode.ONE_PLAYER);
        SceneManager.LoadScene("Game");
    }

    public void Multiplayer()
    {
        Debug.Log("Multiplayer"); 
        gameController.GameModeController.SetGameMode(GameMode.TWO_PLAYERS);
        SceneManager.LoadScene("Game");
    }
    public void HowToPlay()
    {
        Debug.Log("HowToPlay");
        SceneManager.LoadScene("HowToPlay");
    }


    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
