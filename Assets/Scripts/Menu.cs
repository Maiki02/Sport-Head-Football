using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    
    public void PlayGame()
    {
        Debug.Log("PlayGame");
        SceneManager.LoadScene("Game");
    }

    public void Multiplayer()
    {
        Debug.Log("Multiplayer");
    }
    public void HowToPlay()
    {
        Debug.Log("HowToPlay");
    }


    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
