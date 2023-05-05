using Character;
using CustomHelpers;
using UnityEngine;

namespace EnemyController.StateMachine
{
    public abstract class EnemyState : UnitState
    {
        protected readonly EnemyController controller;
        protected readonly EnemyStateMachine stateMachine;
        protected readonly int animEndHash;
        
        public EnemyState(EnemyStateMachine stateMachine_, string animEndId_)
        {
            stateMachine = stateMachine_;
            controller = stateMachine.controller;
            animEndHash = animEndId_.ToHash();
            controller.animationEventInvoker.OnAnimationEvent.AddListener(AnimEnd);
        }


        protected void AnimParamUpdate()
        {
            var _isIdle = controller.rb.velocity.x.IsApproximatelyTo(0);
            
            controller.animator.SetFloat(controller.xSpeedHash, Mathf.Abs( controller.rb.velocity.x));

            var _prevFacingLeft = stateMachine.isFacingLeft;
            stateMachine.isFacingLeft = _isIdle ? stateMachine.isFacingLeft : controller.rb.velocity.x < 0;
        
            if(_prevFacingLeft == stateMachine.isFacingLeft) return;
        
            var _scale = controller.transform.localScale;
            _scale.x = Mathf.Abs(_scale.x);
        
            controller.transform.localScale = stateMachine.isFacingLeft ? 
                new Vector3(-_scale.x, _scale.y, _scale.z) 
                : new Vector3(_scale.x, _scale.y, _scale.z);
        }

        protected void GoToDirection(Vector3 targetPosition_)
        {
            var _direction = (targetPosition_ - controller.transform.position).normalized;
            _direction.y = 0;
            controller.rb.velocity = _direction * controller.patrolSpeed;
        }
        
        protected void OnHit(HealthComponent hpComponent_, DamageInfo damageInfo_)
        {
            if (hpComponent_.IsDead)
            {
                return;
            }
            ToHitState(this);
        }
        
        protected void ToHitState(EnemyState prevState_)
        {
            stateMachine.hitState.prevState = prevState_;
            stateMachine.ChangeState(stateMachine.hitState);
        }
        
        protected void AnimEnd(string eventId_)
        {
            if(eventId_.ToHash() != animEndHash) return;
            OnAnimEnd();
        }
        
        protected virtual void OnAnimEnd() {}
    }
}
