using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using UnityEngine;

public class Character : MonoBehaviour
{
    Rigidbody2D bodyRb; // Rigidbody del cuerpo
    Rigidbody2D feetRb; // Rigidbody del pie
    HingeJoint2D feetHinge; // HingeJoint del pie


    [SerializeField] public float kickSpeed = 800f;  // Velocidad de patada
    [SerializeField] public float returnSpeed = 200f; // Velocidad de retroceso de la patada
    private bool isCurrentlyKicking = false; // Variable para saber si el personaje está pateando
    private float kickTimer = 0f; // Temporizador de la patada 
    [SerializeField] public float kickDuration = 0.02f; // Duración de la patada

    [SerializeField] public float speed = 4.5f; // Velocidad de movimiento
    [SerializeField] public float jumpForce = 200f; // Fuerza de salto
    [SerializeField] private bool isGrounded = false; // Variable para saber si el personaje está en el suelo

    const float GROUND_CHECK_DISTANCE = 0.6f; // Distancia para detectar el suelo
    [SerializeField] private LayerMask groundLayer; // LayerMask para detectar el suelo



    public bool getIsGrounded()
    {
        return isGrounded;
    }

    public void setIsGrounded(bool value)
    {
        isGrounded = value;
    }

    void Start()
    {
        bodyRb = transform.Find("Body").GetComponent<Rigidbody2D>();
        bodyRb.freezeRotation = true;

        feetRb = transform.Find("Feet").GetComponent<Rigidbody2D>();
        //feetRb.freezeRotation = true;

        feetHinge = transform.Find("Feet").GetComponent<HingeJoint2D>();
        feetHinge.useMotor = true;
    }

    void Update()
    {
        // Mejora la detección de suelo
        this.setIsGrounded(Physics2D.Raycast(bodyRb.position, Vector2.down, GROUND_CHECK_DISTANCE, groundLayer));

        float horizontalInput = Input.GetAxis("Horizontal");
        bool isJumping = Input.GetKeyDown(KeyCode.UpArrow);
        bodyRb.velocity = new Vector2(horizontalInput * speed, bodyRb.velocity.y);

        if (isJumping && isGrounded)
        {
            bodyRb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        bool isPressKick = Input.GetKeyDown(KeyCode.Space);


        kickTimer += Time.deltaTime;
        // Lógica de patada con temporizador
        if (isPressKick)
        {
            //Pateamos
            StartKicking();
        }

        if(kickTimer >= kickDuration){
            //Devolvemos la patada
            this.ReturnedFeet();
        }

    }

     public void StartKicking()
    {
        if (!isCurrentlyKicking)
        {
            isCurrentlyKicking = true;
            kickTimer = 0f;

            JointMotor2D motor = feetHinge.motor;
            //Validar si quiero mantener la pierna arriba o no
            //Si quiero mantenerla, creo que no debo validar que esté en el limite izquierdo
            motor.motorSpeed = !IsInLeftLimit() ? kickSpeed : 0f;
            feetHinge.motor = motor;
        } 
    }

    private void ReturnedFeet()
    {
        isCurrentlyKicking = false;
        
        JointMotor2D motor = feetHinge.motor;
        motor.motorSpeed = IsInRightLimit() ? 0f: -returnSpeed; //Si la patada llegó a su limite, seteamos la velocidad en 0.
        feetHinge.motor = motor;
    }

    private bool IsInLeftLimit()
    {
        return feetHinge.jointAngle >= feetHinge.limits.max;
    }

    private bool IsInRightLimit()
    {
        return feetHinge.jointAngle <= feetHinge.limits.min;
    }

}