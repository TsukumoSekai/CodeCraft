namespace OfflineFantasy.GameCraft.Design.FSM
{
    public class BaseState : IState
    {
        public virtual void Initialize() { }

        public virtual void Enter() { }

        public virtual void Update() { }

        public virtual void FixedUpdate() { }

        public virtual void LateUpdate() { }

        public virtual void Exit() { }
    }
}