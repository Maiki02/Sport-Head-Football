using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour, IMovable, IJumper, IKicker
{

    private Game game;
    private TeamSide teamSide;
    private Rigidbody2D bodyRb; // Rigidbody del cuerpo
    private Rigidbody2D feetRb; // Rigidbody del pie
    private HingeJoint2D feetHinge; // HingeJoint del pie
    private Dictionary<string, int> stats = new Dictionary<string, int>();

    [SerializeField] public float kickSpeed = 800f;  // Velocidad de patada
    [SerializeField] public float returnSpeed = 200f; // Velocidad de retroceso de la patada
    [SerializeField] private bool isCurrentlyKicking = false; // Variable para saber si el personaje está pateando
    [SerializeField] private float kickTimer = 0f; // Temporizador de la patada 
    [SerializeField] public float kickDuration = 0.2f; // Duración de la patada

    [SerializeField] public float speed = 5f; // Velocidad de movimiento
    [SerializeField] public float jumpForce = 550f; // Fuerza de salto
    [SerializeField] private bool isGrounded = false; // Variable para saber si el personaje está en el suelo

    const float GROUND_CHECK_DISTANCE = 0.6f; // Distancia para detectar el suelo
    [SerializeField] private LayerMask groundLayer; // LayerMask para detectar el suelo
    public virtual void Start()
    {
        bodyRb = transform.Find("Body").GetComponent<Rigidbody2D>();
        bodyRb.freezeRotation = true;

        feetHinge = transform.Find("Feet").GetComponent<HingeJoint2D>();
        feetHinge.useMotor = true;

        feetRb = transform.Find("Feet").GetComponent<Rigidbody2D>();
        
        game = GameObject.FindWithTag("GameController").GetComponent<Game>();
        
    }

    public void SetPosition(Vector2 position)
    {
        bodyRb.position = position;
        bodyRb.velocity = Vector2.zero;

        feetRb.position = position;
        feetRb.velocity = Vector2.zero;
    }
    public bool getIsGrounded()
    {
        return isGrounded;
    }

    public void setIsGrounded(bool value)
    {
        isGrounded = value;
    }

    public HingeJoint2D GetFeetHinge()
    {
        return feetHinge;
    }

    public Vector2 GetBodyPosition()
    {
        return bodyRb.position;
    }

    //Funcion de validación para seteaar si el Character está sobre el suelo o no
    protected void CheckIsGrounded()
    {
        this.setIsGrounded(Physics2D.Raycast(bodyRb.position, Vector2.down, GROUND_CHECK_DISTANCE, groundLayer));
    }

    //Si no está sobre el suelo,se le aplica una fuerza vertical que lo impulsa simulando el salto.
    public void Jump()
    {
        bodyRb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        this.AddStat("Saltos"); //Sumamos la estadística de salto
    }

    public virtual void Kick()
    {
        if (!isCurrentlyKicking)
        {
            isCurrentlyKicking = true;
            kickTimer = 0f;

            //TODO: Validar si quiero mantener la pierna arriba o no
            //Si quiero mantenerla, creo que no debo validar que esté en el limite maayor
            this.SetMotorSpeed(!IsAtMaxLimit() ? this.getKickSpeed() : 0f);

            this.AddStat("Patadas"); //Sumamos la estadística de patada
        }
    }

    public void Move(float horizontalInput)
    {
        bodyRb.velocity = new Vector2(horizontalInput * speed, bodyRb.velocity.y);
    }

    protected void UpdateKick(bool isPressKick)
    {
        kickTimer += Time.deltaTime;

        if (isPressKick)
        {
            Kick(); //Pateamos
        }

        if (kickTimer >= kickDuration)
        {
            //Devolvemos la patada
            this.ReturnedFeet();
        }
    }

    private void ReturnedFeet()
    {
        isCurrentlyKicking = false;

        //Si la patada llegó a su limite menor, seteamos la velocidad en 0.
        this.SetMotorSpeed(IsAtMinLimit() ? 0f : -this.getReturnSpeed());
    }

    public void SetMotorSpeed(float speed)
    {
        JointMotor2D motor = feetHinge.motor;
        motor.motorSpeed = speed;
        feetHinge.motor = motor;
    }

    public virtual float getReturnSpeed()
    {
        return returnSpeed;
    }

    public virtual float getKickSpeed()
    {
        return kickSpeed;
    }

    public virtual bool IsAtMaxLimit()
    {
        return feetHinge.jointAngle >= feetHinge.limits.max;
    }

    public virtual bool IsAtMinLimit()
    {
        return feetHinge.jointAngle <= feetHinge.limits.min;
    }

    public bool CanMove()
    {
        //Validamos que se pueda mover con el contador, porque está la variable para saber si se está jugando o no
        //Pero si no le permitimos moverse cuando NO se está jugando, no puede festejar el gol :(
        return this.game.GetTimeToStartCounter() <= 0f;
    }

//Dada una estadistica, se suma 1 a la misma. Si no existe, se crea y se le asigna el valor 1.
//Ejemplo: si el personaje salta, se le suma 1 a la estadistica "Jump"
    public void AddStat(string statName)
    {
        if (stats.ContainsKey(statName))
        {
            stats[statName] += 1;
        }
        else
        {
            stats.Add(statName, 1);
        }
    }

    public void ContactWithBall()
    {
        this.AddStat("Contactos con pelota"); //Sumamos la estadística de contacto con la pelota
    }

    public Dictionary<string, int> GetStats()
    {
        return stats;
    }

}