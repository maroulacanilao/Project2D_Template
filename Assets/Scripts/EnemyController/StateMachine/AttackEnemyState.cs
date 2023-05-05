using Character;

namespace EnemyController.StateMachine
{
    public class AttackEnemyState : EnemyState
    {
        private readonly HealthComponent hpComponent;
        public AttackEnemyState(EnemyStateMachine stateMachine_, string animEndId_) : base(stateMachine_, animEndId_)
        {
            hpComponent = controller.healthComponent;
        }
        
        public override void Enter()
        {
            base.Enter();
            controller.animator.SetTrigger(controller.attackHash);
            hpComponent.OnTakeDamage.AddListener(OnHit);
        }
        
        
        public override void Exit()
        {
            hpComponent.OnTakeDamage.RemoveListener(OnHit);
        }
    }
}