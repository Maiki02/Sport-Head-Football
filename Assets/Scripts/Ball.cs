using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private Rigidbody2D rb;
    private Game game;

    // Start is called before the first frame update
    void Start()
    {
        game = GameObject.Find("Game").GetComponent<Game>();
        rb = GetComponent<Rigidbody2D>();
        ResetPosition();
    }

    public void ResetPosition()
    {
        transform.position = new Vector3(0, 0, 0);
        rb.velocity = new Vector2(0, 0);
        rb.angularVelocity = 0; 
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("OnTriggerEnter2D");
        if (other.gameObject.CompareTag("GoalLine"))
        {
            //Si la etiqueta de GoalLine, contiene "Team1" entonces el equipo 2 anotó
            TeamSide teamSide = other.gameObject.name.Contains("Team1") ? TeamSide.Team2 : TeamSide.Team1;
            game.GoalScored(teamSide);
        }
    }

}
