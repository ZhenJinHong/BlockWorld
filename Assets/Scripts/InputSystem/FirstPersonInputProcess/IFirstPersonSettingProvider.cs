namespace CatFramework.InputMiao
{
    public interface IFirstPersonSettingProvider
    {
        float ViewTopLimit { get; }
        float ViewBottomLimit { get; }
        float MovementSpeed { get; }
        float RunSpeed { get; }
        float MovementChangeRate { get; }
        float JumpSpeed { get; }
        float RotationSpeed { get; }
        float RotationThreshold { get; }
    }
}
