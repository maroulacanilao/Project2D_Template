using UnityEngine;

namespace EnemyController.StateMachine
{
    public class FollowPlayerEnemyState : EnemyState
    {
        Transform playerTransform;
        
        private Vector3 playerPosition => playerTransform.position;
        public FollowPlayerEnemyState(EnemyController controller_) : base(controller_)
        {
            
        }
        
        public override void Enter()
        {
            base.Enter();
            playerTransform = controller.playerDetector.player.transform;
        }
        
        public override void LogicUpdate()
        {
            if (!controller.playerDetector.isPlayerReachable)
            {
                controller.ChangeState(controller.patrolState);
                return;
            }
            
            if (Vector3.Distance(controller.transform.position, playerPosition) <= controller.attackRange)
            {
                controller.ChangeState(controller.attackState);
                return;
            }
            
            AnimParamUpdate();
        }
    }
}