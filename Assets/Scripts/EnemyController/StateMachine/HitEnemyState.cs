using CustomHelpers;

namespace EnemyController.StateMachine
{
    public class HitEnemyState : EnemyState
    {
        public EnemyState prevState;

        public HitEnemyState(EnemyStateMachine stateMachine_, string animEndId_) : base(stateMachine_, animEndId_)
        {
        }
        
        public override void Enter()
        {
            base.Enter();
            controller.animator.SetTrigger(controller.hitHash);
            controller.rb.ResetVelocity();
        }
        
        public override void Exit()
        {
            base.Exit();
            prevState = null;
        }

        protected override void OnAnimEnd()
        {
            if (prevState == null)
            {
                prevState = stateMachine.patrolState;
            }
            stateMachine.ChangeState(prevState);
        }
    }
}