using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    GameManager gameManager;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();   
    }
    public void PlayGame()
    {
        Debug.Log("PlayGame");
        this.gameManager.SetGameMode(GameMode.ONE_PLAYER);
        SceneManager.LoadScene("Game");
    }

    public void Multiplayer()
    {
        Debug.Log("Multiplayer"); 
        this.gameManager.SetGameMode(GameMode.TWO_PLAYERS);
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
