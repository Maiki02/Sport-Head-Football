using UnityEngine;

public class PlayerTwo : Rival {

    void Update()
    {
        
        if(!this.CanMove()) return;
        
        // Checkear si el personaje está en el suelo
        this.CheckIsGrounded();

        // Le aplicamos fuerza al personaje para que se mueva
        float horizontalInput = Input.GetAxis("Horizontal2");
        this.Move(horizontalInput);

        // Lógica de salto
        bool isPressJump = Input.GetKeyDown(KeyCode.W);
        if (isPressJump && this.getIsGrounded())
        {
            this.Jump();
        }

        //Logica de la patada y tiempo de la patada
        bool isPressKick = Input.GetKey(KeyCode.P);
        this.UpdateKick(isPressKick);

    }
}