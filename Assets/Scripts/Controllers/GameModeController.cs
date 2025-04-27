using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModeController
{
    private GameMode gameMode;
    public void SetGameMode(GameMode gameMode) 
    {
        this.gameMode = gameMode;
    }

    public GameMode GetGameMode() 
    {
        return this.gameMode;
    }

}
