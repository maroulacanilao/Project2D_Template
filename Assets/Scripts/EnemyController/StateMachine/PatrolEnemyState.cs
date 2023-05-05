using UnityEngine;

namespace EnemyController.StateMachine
{
    [System.Serializable]
    public class PatrolEnemyState : EnemyState
    {
        int currentWaypointIndex;
        private float startingDistToWaypoint;
        
        public PatrolEnemyState(EnemyStateMachine stateMachine_, string animEndId_ = "") : base(stateMachine_, animEndId_)
        {
            currentWaypointIndex = 0;
        }

        public override void LogicUpdate()
        {
            if(controller.playerDetector.isPlayerReachable)
            {
                stateMachine.ChangeState(stateMachine.chaseState);
                return;
            }
            if(GetDistanceToWaypoint() <= 0.3f)
            { 
                GetNextWaypointIndex();
                return;
            }
            AnimParamUpdate();
        }

        public override void PhysicsUpdate()
        {
            GoToDirection(controller.patrolWaypoints[currentWaypointIndex].position);
        }
        
        private void GetRandomWaypointIndex()
        {
            currentWaypointIndex = UnityEngine.Random.Range(0, controller.patrolWaypoints.Length);
            startingDistToWaypoint = GetDistanceToWaypoint();
        }
        
        private void GetNextWaypointIndex()
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % controller.patrolWaypoints.Length;
            startingDistToWaypoint = GetDistanceToWaypoint();
        }

        private float GetDistanceToWaypoint()
        {
            return Vector2.Distance(controller.transform.position,
                controller.patrolWaypoints[currentWaypointIndex].position);
        }
    }
}