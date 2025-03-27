

using UnityEngine;

public class PlayerTwo : Character {

    void Update()
    {
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
        bool isPressKick = Input.GetKeyDown(KeyCode.Space);
        this.UpdateKick(isPressKick);

    }

    //Intercambiamos las funciones porque el personaje 2 está dado vuelta
    public override bool IsAtMaxLimit()
    {
        return GetFeetHinge().jointAngle <= GetFeetHinge().limits.max;
    }

    public override bool IsAtMinLimit()
    {
        return GetFeetHinge().jointAngle >= GetFeetHinge().limits.min;
    }

    //También intercambiamos las velocidades de patada porque el personaje 2 está dado vuelta
    public override float getKickSpeed()
    {
        return -base.getKickSpeed();
    }

    public override float getReturnSpeed()
    {
        return -base.getReturnSpeed();
    }
}