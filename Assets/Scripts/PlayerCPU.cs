

using UnityEngine;

public class PlayerCPU : Rival {

    Transform ball;
    const float xThreshold = 2.0f;

    bool isPressKick = false;
    public override void Start(){
        base.Start();

        ball = GameObject.Find("Ball").transform;
    }
    void Update()
    {
        // Checkear si el personaje esta en el suelo
        //this.CheckIsGrounded();

        // Le aplicamos fuerza al personaje para que se mueva en función de la bola y su posición en el campo
        this.MoveCPU();

        this.JumpCPU(xThreshold);
        
        this.UpdateKick(isPressKick);
    }


    //Movemos el CPU en función de donde está la bola con un horizontalInput minimo y máximo de -1 a 1
    void MoveCPU()
    {
        float horizontalInput = Mathf.Sign(ball.position.x - transform.position.x);
        this.Move(horizontalInput);
    }

    void JumpCPU(float distanceBall){
        bool isBallNear = ball.position.y > transform.position.y && Mathf.Abs(ball.position.x - transform.position.x) <= xThreshold;

        if(isBallNear){
            this.Jump();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            this.isPressKick = true;
            this.Kick();

        }
    }

    public override void Kick()
    {
        base.Kick();
        this.isPressKick = false;
    }






}