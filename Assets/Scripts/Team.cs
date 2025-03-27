using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Team : MonoBehaviour
{
    private string teamName;
    private Character character; //Personaje del equipo
    private int score = 0; //Cantidad de goles del equipo

    private GameObject Goal; //Portería del equipo
    private TeamSide teamSide; //Lado del equipo: Equipo 1 o 2


    public Team(TeamSide teamSide, string teamName, Character character)
    {
        this.teamSide = teamSide;
        this.teamName = teamName;
        this.character = character;
        this.score = 0;
    }

    public string GetTeamName()
    {
        return teamName;
    }

    public Character GetCharacter()
    {
        return character;
    }

    public int GetScore()
    {
        return score;
    }

    public void ResetScore()
    {
        this.score = 0;
    }

    public void AddScore()
    {
        this.score++;
    }

    public void ResetPosition(){
        
    }

}
