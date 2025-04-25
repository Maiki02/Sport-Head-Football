public interface IKicker {
    void Kick();
    void SetMotorSpeed(float speed);
    float getKickSpeed();
    float getReturnSpeed();
    bool IsAtMaxLimit();
    bool IsAtMinLimit();
}