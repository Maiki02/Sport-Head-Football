using UnityEngine;
using TMPro;
public class ScoreUIUpdater : MonoBehaviour, IScoreObserver
{
    [SerializeField] private TextMeshProUGUI scoreTeam1Text;
    [SerializeField] private TextMeshProUGUI scoreTeam2Text;

    private void Start()
    {
        this.scoreTeam1Text = GameObject.FindWithTag("ScoreTeam1").GetComponent<TextMeshProUGUI>();
        this.scoreTeam2Text = GameObject.FindWithTag("ScoreTeam2").GetComponent<TextMeshProUGUI>();
        Game game = FindObjectOfType<Game>();
        game.AddObserver(this);
    }

    public void OnScoreChanged(int newScoreTeam1, int newScoreTeam2)
    {
        scoreTeam1Text.text = newScoreTeam1.ToString();
        scoreTeam2Text.text = newScoreTeam2.ToString();
    }

    private void OnDestroy()
    {
        Game game = FindObjectOfType<Game>();
        if (game != null)
            game.RemoveObserver(this);
    }
}
