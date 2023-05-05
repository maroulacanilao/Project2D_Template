using Character;
using UnityEngine;

namespace EnemyController.StateMachine
{
    public class ChaseEnemyState : EnemyState
    {
        Transform playerTransform;
        private readonly HealthComponent hpComponent;
        private Vector3 playerPosition => playerTransform.position;
        
        public ChaseEnemyState(EnemyStateMachine stateMachine_, string animEndId_ ="") : base(stateMachine_, animEndId_)
        {
            hpComponent = controller.healthComponent;
        }

        public override void Enter()
        {
            base.Enter();
            playerTransform = controller.playerDetector.player.transform;
            hpComponent.OnTakeDamage.AddListener(OnHit);
        }
        
        public override void LogicUpdate()
        {
            if (!controller.playerDetector.isPlayerReachable)
            {
                stateMachine.ChangeState(stateMachine.patrolState);
                return;
            }
            
            // if (Vector3.Distance(controller.transform.position, playerPosition) <= controller.attackRange)
            // {
            //     controller.ChangeState(controller.attackState);
            //     return;
            // }
            
            AnimParamUpdate();
        }
        
        public override void PhysicsUpdate()
        {
            GoToDirection(playerPosition);
        }

        public override void Exit()
        {
            hpComponent.OnTakeDamage.AddListener(OnHit);
        }
    }
}