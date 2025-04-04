using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private Rigidbody2D rb;
    private Game game;

    //Variables que utilizo para destrabar la pelota cuando se queda trabada arriba del arco contra la pared
    [SerializeField] private float inactivityThreshold = 0.1f;    // Velocidad mínima para considerarla inactiva
    private float inactivityTimer = 0f;          // Contador de tiempo de inactividad
    private float resetDelay = 3f;               // Tiempo en segundos antes de aplicar la fuerza
    [SerializeField] private float bounceForce = 5f;              // Fuerza del desplazamiento cuando le aplicamos la fuerza
    private float wallDetectionDistance = 3f;  // Distancia para detectar la pared 
    //No entiendo muy bien por qué cuando la pelota está pegada a la pared, la distancia es de 2.77f.

    private GameObject leftWall; // Referencia a la pared izquierda
    private GameObject rightWall; // Referencia a la pared derecha

    // Start is called before the first frame update
    void Start()
    {
        //Detectamos las paredes para cuando se quede estancada la pelota
        this.leftWall = GameObject.Find("Wall Left"); 
        this.rightWall = GameObject.Find("Wall Right");

        game = GameObject.FindWithTag("GameController").GetComponent<Game>();
        rb = GetComponent<Rigidbody2D>();
        ResetPosition();
    }

    void Update()
    {
        
        // Verificcamos si la pelota está casi quieta
        if (rb.velocity.magnitude < inactivityThreshold)
        {

            inactivityTimer += Time.deltaTime;
            //Debug.Log("Tiempo de inactividad: " + inactivityTimer + " Velocidad: " + rb.velocity.magnitude );
            // Si pasaron 3 segundos, verificamos si está cerca de una pared
            if (inactivityTimer >= resetDelay)
            {
                CheckAndBounceFromWall();
                inactivityTimer = 0f;
            }
        }
        else
        {
            //Si la pelota se mueve, reiniciamos el contador de inactividad.
            inactivityTimer = 0f;
        }
    }

    bool IsNearToWall(GameObject wall)
    {
        /*Debug.Log("IsNearToWall: " + wall.name + 
        " Distancia: " + Vector2.Distance(transform.position, wall.transform.position)
        + " Distancia para estar cerca: " + wallDetectionDistance);*/
        return Vector2.Distance(transform.position, wall.transform.position) < wallDetectionDistance;        
    }

 void CheckAndBounceFromWall()
    {
        // Variable para almacenar la dirección de rebote
        Vector2 bounceDirection = Vector2.zero;
        
        // Comprobar si está cerca de la pared izquierda
        if (this.IsNearToWall(leftWall))
        {
            // Rebotar hacia la derecha
            bounceDirection = Vector2.right;
        }
        // Comprobar si está cerca de la pared derecha
        else if (this.IsNearToWall(rightWall))
        {
            // Rebotar hacia la izquierda
            bounceDirection = Vector2.left;
        }
        
        // Si se detectó una pared cercana, aplicar fuerza
        if (bounceDirection != Vector2.zero)
        {
            // Aplicar la fuerza de rebote
            //rb.velocity = Vector2.zero;
            rb.AddForce(bounceDirection * bounceForce, ForceMode2D.Impulse);
        }
        
        // No hacer nada si la pelota está en el suelo
    }


    public void ResetPosition()
    {
        transform.position = new Vector3(0, 0, 0);
        rb.velocity = new Vector2(0, 0);
        rb.angularVelocity = 0; 
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("OnCollisionEnter2D");
        if (other.gameObject.CompareTag("Player"))
        {
            //Si la etiqueta de Player, contiene "Team1" entonces el equipo 1 anotó
            TeamSide teamSide = other.gameObject.name.Contains("Team1") ? TeamSide.Team1 : TeamSide.Team2;
            game.GoalScored(teamSide);
        }
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
