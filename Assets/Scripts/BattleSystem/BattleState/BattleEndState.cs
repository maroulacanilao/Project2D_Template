using System.Collections;
using UnityEngine;

namespace BattleSystem.BattleState
{
    public class BattleEndState : BattleStateBase
    {

        public BattleEndState(BattleManager battleManager_, BattleStateMachine stateMachine_) : base(battleManager_, stateMachine_)
        {
        }

        public override IEnumerator Enter()
        {
            Debug.Log($"Exit Logic");
            yield return Exit();
        }

        public override IEnumerator Exit()
        {
            BattleManager.End();
            yield break;
        }
    }
}