using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    
    void PlayGame()
    {
        Application.LoadLevel("Game");
    }




    void QuitGame()
    {
        Application.Quit();
    }
}
