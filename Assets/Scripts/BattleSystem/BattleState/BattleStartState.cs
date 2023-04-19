using System.Collections;
using UnityEngine;
using CustomHelpers;

namespace BattleSystem.BattleState
{
    public class BattleStartState : BattleStateBase
    {

        public BattleStartState(BattleManager battleManager_, BattleStateMachine stateMachine_) : base(battleManager_, stateMachine_)
        {
            
        }
        
        public override IEnumerator Enter()
        {
            Debug.Log("Start Battle");
            yield return new WaitForSeconds(1f);
            yield return StateMachine.ChangeState(BattleManager.PlayerTurnState);
        }
        
        public override IEnumerator Exit()
        {
            yield break;
        }
    }
}