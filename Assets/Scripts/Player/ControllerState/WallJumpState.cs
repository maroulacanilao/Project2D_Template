using CustomHelpers;
using UnityEngine;

namespace Player.ControllerState
{
    public class WallJump : PlayerState
    {
        public WallJump(PlayerStateMachine stateMachine_, string animEndEventId_ = null) : base(stateMachine_, animEndEventId_)
        {
        }
        
        public override void Enter()
        {
            base.Enter();
            controller.animator.SetTrigger(controller.jumpHash);
            var _xVel = StateMachine.facingLeft ? controller.wallJumpForce.x : -controller.wallJumpForce.x;
            rb.SetVelocity(_xVel , controller.wallJumpForce.y);
        }
        
        public override void HandleInput()
        {
            
        }

        public override void LogicUpdate()
        {
            AnimParamUpdate();
        }
        
        public override void PhysicsUpdate()
        {
            if(IsPlayerGrounded()) return;
            
            if (!StateMachine.isGrounded && rb.velocity.y < 0 )
            {
                StateMachine.ChangeState(StateMachine.FallState);
                return;
            }
            MovementUpdate();
            FallUpdate();
        }
    }
}