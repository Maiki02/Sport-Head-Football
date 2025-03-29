

using UnityEngine;

public class PlayerOne : Character {
    
    void Update()
    {
        if(!this.CanMove()) return;
        // Checkear si el personaje está en el suelo
        this.CheckIsGrounded();

        // Le aplicamos fuerza al personaje para que se mueva
        float horizontalInput = Input.GetAxis("Horizontal");
        this.Move(horizontalInput);

        // Lógica de salto
        bool isPressJump = Input.GetKeyDown(KeyCode.UpArrow);
        if (isPressJump && this.getIsGrounded())
        {
            this.Jump();
        }

        //Logica de la patada y tiempo de la patada
        bool isPressKick = Input.GetKey(KeyCode.Space);
        this.UpdateKick(isPressKick);

    }
}