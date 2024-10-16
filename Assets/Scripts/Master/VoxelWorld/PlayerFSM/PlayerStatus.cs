namespace VoxelWorld
{
    public abstract class PlayerStatus
    {
        public abstract bool EnableInput { get; set; }
        public abstract void OnEnter(GameCassette data);
        public abstract void OnExit(GameCassette data);
        public virtual void Update() { }
        public virtual void FixedUpdate() { }
        public virtual void LateUpdate() { }
    }
}