using UnityEngine;
using System.Collections;

public class PlayerCPU : Rival 
{
    // Referencias
    private Transform ball;

    private AIState currentState = AIState.DEFEND;

    // Configuraciónes
    [SerializeField] private float reactionTime = 0.2f;
    [SerializeField] private float kickDistance = 1.5f;
    [SerializeField] private float jumpThreshold = 2.0f;
    [SerializeField] private float defendPositionX = -10.0f;
    [SerializeField] private float retreatThreshold = 1.0f;
    [SerializeField] private float approachSpeed = 1.0f;
    [SerializeField] private float defendSpeed = 0.7f;
    [SerializeField] private float decisionUpdateInterval = 0.4f;

    private bool isPressKick = false;
    private float horizontalInput = 0f;
    private bool shouldJump = false;

    public override void Start()
    {
        base.Start();

        // Encontrar referencias de pelota y oponente
        ball = GameObject.FindWithTag("Ball").transform;

        // Iniciar la corrutina de IA
        //Como hay muchos calculos, decidí hacerlo en una corrutina, 
        // para que no se ejecute en todos los Updates()
        StartCoroutine(AIDecisionLoop());
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

    // En esta función, tomo parametros del juego y
    // se lo paso a una función para que obtenga que acción tiene que hacer el CPU
    private void UpdateAIState()
    {
        Vector2 ballPos = ball.position;
        Vector2 myPos = this.GetBodyPosition();
        float distanceToBall = Vector2.Distance(ballPos, myPos);
        
        // Verificar si la pelota está en nuestro lado del campo y un poco más del otro
        bool ballInOurHalf = ballPos.x < 2f;
        
        // Cambiamos el estado según la situación
        currentState = GetNewState(ballPos, myPos, distanceToBall, ballInOurHalf);
    }

    // Dado los parametros del juego como, la posiición de la pelota, la posición del jugador, 
    // la distancia a la pelota y si la pelota está en nuestro lado del campo,
    // se determina el nuevo estado de la IA
    private AIState GetNewState(Vector2 ballPos, Vector2 myPos, float distanceToBall, bool ballInOurHalf){
        
        // Si la pelota está lejos, volvemos a defender
        if(!ballInOurHalf) return Mathf.Abs(myPos.x - defendPositionX) > retreatThreshold? AIState.RETREAT : AIState.DEFEND;
        

        if(ballPos.y > myPos.y + 1.5f && ballPos.x > myPos.x + 1.5f){
            Debug.Log("Se cumple JUMP");
            return AIState.JUMP;
        }

        if (distanceToBall < kickDistance && ballPos.y <= myPos.y + 1.5f)
        {
            return AIState.KICK;
        }

        if (distanceToBall < jumpThreshold && ballPos.y > myPos.y)
        {
            return AIState.PREPARE_KICK;
        }
            
            return AIState.APPROACH;
    }


    // En base a unos parametros y el estado actual de la IA,
    // Aplica valores a horizontalInput, shouldJump e isPressKick
    // para que luego, la acción se aplique en el UPDATE()
    private void MakeDecisions()
    {
        Vector2 ballPos = ball.position;
        Vector2 myPos = this.GetBodyPosition();
        Vector2 ballVelocity = ball.GetComponent<Rigidbody2D>().velocity;
        
        // Predecir dónde estará la pelota
        Vector2 predictedBallPos = ballPos + ballVelocity * reactionTime;
        //Debug.Log("Current State: " + currentState);
        switch (currentState)
        {
            case AIState.DEFEND:
                // Mantener una posición defensiva
                horizontalInput = CalculateHorizontalInput(myPos.x, defendPositionX, defendSpeed);
                
                // Saltar si la pelota viene alta
                shouldJump = ShouldJumpForBall(myPos, predictedBallPos);
                isPressKick = false;
                break;
                
            case AIState.APPROACH:
                // Acercarse a la pelota
                horizontalInput = CalculateHorizontalInput(myPos.x, predictedBallPos.x, approachSpeed);
                
                // Saltar si es necesario para alcanzar la pelota
                shouldJump = ShouldJumpForBall(myPos, predictedBallPos);
                isPressKick = false;
                break;
                
            case AIState.PREPARE_KICK:
                // Posicionarse para patear
                horizontalInput = CalculateHorizontalInput(myPos.x, predictedBallPos.x - 0.5f, approachSpeed);
                // Saltar si la pelota está por encima
                shouldJump = predictedBallPos.y > myPos.y + 0.5f && this.getIsGrounded();
                isPressKick = false;
                break;
                
            case AIState.KICK:
                // Intentar patear
                horizontalInput = CalculateHorizontalInput(myPos.x, predictedBallPos.x, approachSpeed);
                isPressKick = true;
                shouldJump = false;
                break;
                
            case AIState.RETREAT:
                // Volver a posición defensiva
                horizontalInput = CalculateHorizontalInput(myPos.x, defendPositionX, defendSpeed);
                isPressKick = false;
                shouldJump = false;
                break;

            case AIState.JUMP:
                // Saltar para interceptar la pelota
                horizontalInput = CalculateHorizontalInput(myPos.x, predictedBallPos.x, approachSpeed);
                shouldJump = true;
                isPressKick = false;
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

}