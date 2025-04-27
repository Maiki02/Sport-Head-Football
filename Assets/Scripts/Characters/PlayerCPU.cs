using UnityEngine;
using System.Collections;

public class PlayerCPU : Rival 
{
    // Referencias
    private Transform ball;
    private Transform ownGoal;
    private Transform enemyGoal;
    private Transform playerOne;

    // Estados de la IA
    private enum AIState 
    {
        Defend,
        Approach,
        PrepareKick,
        Kick,
        Retreat
    }
    private AIState currentState = AIState.Defend;

    // Configuración
    [Header("IA Settings")]
    [SerializeField] private float reactionTime = 0.2f;
    [SerializeField] private float kickDistance = 1.5f;
    [SerializeField] private float jumpThreshold = 2.0f;
    [SerializeField] private float defendPositionX = -5.0f;
    [SerializeField] private float retreatThreshold = 1.0f;
    [SerializeField] private float approachSpeed = 1.0f;
    [SerializeField] private float defendSpeed = 0.7f;
    [SerializeField] private float decisionUpdateInterval = 0.1f;

    // Variables internas
    private bool isPressKick = false;
    private Vector2 targetPosition;
    private float lastDecisionTime = 0f;
    private float horizontalInput = 0f;
    private bool shouldJump = false;
    private Coroutine aiRoutine;

    public override void Start()
    {
        base.Start();

        // Encontrar referencias importantes
        ball = GameObject.FindWithTag("Ball").transform;
        playerOne = GameObject.FindWithTag("Player").transform;

        // Encontrar los arcos
        ownGoal = GameObject.FindWithTag("GoalLeftTeam2").transform;
        enemyGoal = GameObject.FindWithTag("GoalRightTeam1").transform;

        // Iniciar la corrutina de IA
        //aiRoutine = StartCoroutine(AIDecisionLoop());
    }

    void Update()
    {
        if (!this.CanMove()) 
        {
            StopMovement();
            return;
        }

        // Checkear si el personaje está en el suelo
        this.CheckIsGrounded();

        // Realizar acciones basadas en las decisiones de la IA
        this.Move(horizontalInput);

        if (shouldJump && this.getIsGrounded())
        {
            this.Jump();
            shouldJump = false;
        }

        // Actualizar la patada
        this.UpdateKick(isPressKick);
    }

    private void StopMovement()
    {
        horizontalInput = 0;
        shouldJump = false;
        isPressKick = false;
    }

    private IEnumerator AIDecisionLoop()
    {
        while (true)
        {
            if (this.CanMove())
            {
                UpdateAIState();
                MakeDecisions();
            }
            else
            {
                StopMovement();
            }
            
            yield return new WaitForSeconds(decisionUpdateInterval);
        }
    }

    private void UpdateAIState()
    {
        Vector2 ballPos = ball.position;
        Vector2 myPos = this.GetBodyPosition();
        float distanceToBall = Vector2.Distance(ballPos, myPos);
        
        // Verificar si la pelota está en nuestro lado del campo
        bool ballInOurHalf = ballPos.x < 0;
        
        // La pelota está cayendo hacia nosotros
        bool ballComingDown = ball.GetComponent<Rigidbody2D>().velocity.y < 0;
        
        // Cambiamos el estado según la situación
        if (ballInOurHalf)
        {
            if (distanceToBall < kickDistance && ballPos.y <= myPos.y + 1.5f)
            {
                currentState = AIState.Kick;
            }
            else if (distanceToBall < jumpThreshold && ballPos.y > myPos.y)
            {
                currentState = AIState.PrepareKick;
            }
            else
            {
                currentState = AIState.Approach;
            }
        }
        else
        {
            // Si la pelota está lejos, volvemos a defender
            if (Mathf.Abs(myPos.x - defendPositionX) > retreatThreshold)
            {
                currentState = AIState.Retreat;
            }
            else
            {
                currentState = AIState.Defend;
            }
        }
    }

    private void MakeDecisions()
    {
        Vector2 ballPos = ball.position;
        Vector2 myPos = this.GetBodyPosition();
        Vector2 ballVelocity = ball.GetComponent<Rigidbody2D>().velocity;
        
        // Predecir dónde estará la pelota
        Vector2 predictedBallPos = ballPos + ballVelocity * reactionTime;
        
        switch (currentState)
        {
            case AIState.Defend:
                // Mantener una posición defensiva
                horizontalInput = CalculateHorizontalInput(myPos.x, defendPositionX, defendSpeed);
                
                // Saltar si la pelota viene alta
                shouldJump = ShouldJumpForBall(myPos, predictedBallPos);
                isPressKick = false;
                break;
                
            case AIState.Approach:
                // Acercarse a la pelota
                horizontalInput = CalculateHorizontalInput(myPos.x, predictedBallPos.x, approachSpeed);
                
                // Saltar si es necesario para alcanzar la pelota
                shouldJump = ShouldJumpForBall(myPos, predictedBallPos);
                isPressKick = false;
                break;
                
            case AIState.PrepareKick:
                // Posicionarse para patear
                horizontalInput = CalculateHorizontalInput(myPos.x, predictedBallPos.x - 0.5f, approachSpeed);
                
                // Saltar si la pelota está por encima
                shouldJump = predictedBallPos.y > myPos.y + 0.5f && this.getIsGrounded();
                isPressKick = false;
                break;
                
            case AIState.Kick:
                // Intentar patear
                horizontalInput = CalculateHorizontalInput(myPos.x, predictedBallPos.x, approachSpeed);
                isPressKick = true;
                shouldJump = false;
                break;
                
            case AIState.Retreat:
                // Volver a posición defensiva
                horizontalInput = CalculateHorizontalInput(myPos.x, defendPositionX, defendSpeed);
                isPressKick = false;
                shouldJump = false;
                break;
        }
    }

    private float CalculateHorizontalInput(float currentX, float targetX, float speedMultiplier)
    {
        float direction = Mathf.Sign(targetX - currentX);
        float distance = Mathf.Abs(targetX - currentX);
        
        // Si está muy cerca del objetivo, reducir la velocidad
        if (distance < 0.5f)
        {
            return direction * distance * speedMultiplier;
        }
        return direction * speedMultiplier;
    }

    private bool ShouldJumpForBall(Vector2 myPos, Vector2 ballPos)
    {
        // Saltar si la pelota está por encima y a una distancia alcanzable
        if (ballPos.y > myPos.y + 0.5f &&
            Mathf.Abs(ballPos.x - myPos.x) < jumpThreshold &&
            this.getIsGrounded())
        {
            return true;
        }
        return false;
    }

    public override void Kick()
    {
        base.Kick();
        // Después de patear, volvemos a intentar movernos
        StartCoroutine(ResetKick());
    }

    private IEnumerator ResetKick()
    {
        yield return new WaitForSeconds(kickDuration);
        isPressKick = false;
    }

    private void OnDrawGizmos()
    {
        // Visualización para debugging (en modo Editor)
        if (Application.isPlaying)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(this.GetBodyPosition(), kickDistance);
            
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(this.GetBodyPosition(), jumpThreshold);
            
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, new Vector3(defendPositionX, transform.position.y, 0));
            
            // Dibujar el estado actual
            Vector3 labelPosition = transform.position + Vector3.up * 2;
            UnityEditor.Handles.Label(labelPosition, currentState.ToString());
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Colisión CPU: " + collision.gameObject.name);
        if (collision.gameObject.CompareTag("Ball"))
        {
            // Si la pelota colisiona con el jugador, intentamos patear
            isPressKick = true;
        }
    }
}