using System.Collections;
using UnityEngine;

namespace BattleSystem.BattleState
{
    public class EnemyTurnState : TurnBaseState
    {
        public EnemyTurnState(BattleManager battleManager_, BattleStateMachine stateMachine_) : base(battleManager_, stateMachine_)
        {
        }

        public override IEnumerator Enter()
        {
            Debug.Log($"{enemy.gameObject}'s Start");
            yield return new WaitForSeconds(0.5f);
            yield return StartTurn();
        }

        public override IEnumerator StartTurn()
        {
            Debug.Log($"{enemy.gameObject}'s Start Turn");
            yield return new WaitForSeconds(0.2f);
            yield return TurnLogic();
        }

        public override IEnumerator TurnLogic()
        {
            Debug.Log($"{enemy.gameObject}'s Turn Logic");
            yield return new WaitForSeconds(0.2f);
            var _damageInfo = new DamageInfo(10, enemy.gameObject, DamageType.Physical);
            yield return new WaitForSeconds(0.2f);
            yield return EndTurn();
        }

        public override IEnumerator EndTurn()
        {
            Debug.Log($"{enemy.gameObject}'s End Turn Logic");
            if (player.healthComponent.CurrentHp > 0) yield return StateMachine.ChangeState(BattleManager.PlayerTurnState);
            else yield return StateMachine.ChangeState(BattleManager.EndState);
        }

        public override IEnumerator Exit()
        {
            Debug.Log($"{enemy.gameObject}'s Exit State");
            yield break;
        }
    }
}