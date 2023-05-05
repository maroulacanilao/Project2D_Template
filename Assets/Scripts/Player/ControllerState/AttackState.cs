using CustomHelpers;
using Managers;
using UnityEngine;

namespace Player.ControllerState
{
    public class AttackState : PlayerState
    {
        private readonly int attackAnimEndHash;
        private readonly int weaponAttackAnimEventHash;
        private float animTimer;
        private bool willAttackAgain;
        private bool isMoving;
    
        public AttackState(PlayerStateMachine stateMachine_, string animEndEventId_ = null) : base(stateMachine_, animEndEventId_)
        {
            attackAnimEndHash = "AttackAnimEnd".ToHash();
            weaponAttackAnimEventHash = "WeaponAttack".ToHash();
            controller.animationEventInvoker.OnAnimationEvent.AddListener(AnimEvent);
        }
    
        public override void Enter()
        {
            base.Enter();
            animTimer = 0;
            willAttackAgain = false;
            isMoving = !StateMachine.input.x.IsApproximatelyTo(0);
            if(isMoving) controller.animator.SetTrigger(controller.runningAttackHash);
            else controller.animator.SetTrigger(controller.attackHash);
        }

        public override void HandleInput()
        {
            if(InputManager.AttackAction.triggered) willAttackAgain = true;
        }
    
        public override void LogicUpdate()
        {
            animTimer += Time.deltaTime;
            if(animTimer < 1f) return;
            StateMachine.ChangeState(StateMachine.GroundedState);
        }
    
        public override void PhysicsUpdate()
        {
            if(IsPlayerFalling()) return;
        
            rb.SetVelocity(Vector2.zero);
        }

        public override void Exit()
        {
            animTimer = 0;
            willAttackAgain = false;
            controller.animator.ResetTrigger(controller.attackHash);
            isMoving = false;
        }
    
        private void AnimEvent(string eventId_)
        {
            var _eventHash = eventId_.ToHash();
            if (_eventHash == weaponAttackAnimEventHash)
            {
                rb.SetVelocity(Vector2.zero);
                isMoving = false;
                return;
            }
            if(_eventHash != attackAnimEndHash) return;
        
            if(IsPlayerFalling()) return;
        
            if(willAttackAgain) StateMachine.ChangeState(StateMachine.AttackState);
            else StateMachine.ChangeState(StateMachine.GroundedState);
        }
    }
}
