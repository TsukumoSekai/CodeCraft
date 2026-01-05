namespace OfflineFantasy.GameCraft.Design.FSM
{
    public interface IState
    {
        public void Initialize();
        public void Enter();
        public void Update();
        public void FixedUpdate();
        public void LateUpdate();
        public void Exit();
    }
}