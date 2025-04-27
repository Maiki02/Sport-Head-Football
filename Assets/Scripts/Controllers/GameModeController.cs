using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModeController: IResettable
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

    public void Reset()
    {
        this.gameMode = GameMode.NONE;
    }

}
