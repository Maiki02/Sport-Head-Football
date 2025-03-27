using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Team : MonoBehaviour
{
    private string teamName;
    private Character character;
    private int score = 0;

    public Team(string teamName, Character character)
    {
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

    public void AddScore(int score)
    {
        this.score += score;
    }

}
