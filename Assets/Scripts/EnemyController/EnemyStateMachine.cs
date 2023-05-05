using EnemyController.StateMachine;

namespace EnemyController
{
    public class EnemyStateMachine : UnitStateMachine
    {
        #region States

        public PatrolEnemyState patrolState { get; private set; }
        public ChaseEnemyState chaseState { get; private set; }
        
        public AttackEnemyState attackState { get; private set; }
        
        public HitEnemyState hitState { get; private set; }

        #endregion

        public readonly EnemyController controller;
        public bool isFacingLeft { get; set; } 
        
        public EnemyStateMachine(EnemyController controller_)
        {
            controller = controller_;
        }
        
        public override void Initialize()
        {
            patrolState = new PatrolEnemyState(this);
            chaseState = new ChaseEnemyState(this);
            attackState = new AttackEnemyState(this, "WeaponAttack");
            hitState = new HitEnemyState(this, "HitAnimEnd");
            
            CurrentState = patrolState;
            CurrentState.Enter();
        }
        
    }
}