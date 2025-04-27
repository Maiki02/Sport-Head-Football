using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }

    public GameModeController GameModeController { get; private set; }
    public ResultsController ResultsController { get; private set; }
    public GoalLogController GoalLogController { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            GameModeController = new GameModeController();
            ResultsController = new ResultsController();
            GoalLogController = new GoalLogController();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ResetAllControllers()
    {
        GameModeController.Reset();
        ResultsController.Reset();
        GoalLogController.Reset();
    }
}
