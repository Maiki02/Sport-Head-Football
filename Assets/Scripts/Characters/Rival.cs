

using UnityEngine;

public class Rival : Character {

    //Intercambiamos las funciones porque el rival está dado vuelta
    public override bool IsAtMaxLimit()
    {
        return GetFeetHinge().jointAngle <= GetFeetHinge().limits.max;
    }

    public override bool IsAtMinLimit()
    {
        return GetFeetHinge().jointAngle >= GetFeetHinge().limits.min;
    }

    //También intercambiamos las velocidades de patada porque el rival está dado vuelta
    public override float getKickSpeed()
    {
        return -base.getKickSpeed();
    }

    public override float getReturnSpeed()
    {
        return -base.getReturnSpeed();
    }
}