using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{

    public void PlayGame()
    {
        Debug.Log("PlayGame");
        GameController.Instance.GameModeController.SetGameMode(GameMode.ONE_PLAYER);
        SceneManager.LoadScene("Game");
    }

    public void Multiplayer()
    {
        Debug.Log("Multiplayer"); 
        GameController.Instance.GameModeController.SetGameMode(GameMode.TWO_PLAYERS);
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
