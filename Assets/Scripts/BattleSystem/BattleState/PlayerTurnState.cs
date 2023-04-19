using System.Collections;
using UnityEngine;

namespace BattleSystem.BattleState
{
    public class PlayerTurnState : TurnBaseState
    {
        public PlayerTurnState(BattleManager battleManager_, BattleStateMachine stateMachine_) : base(battleManager_, stateMachine_)
        { }
        
        public override IEnumerator Enter()
        {
            Debug.Log($"{player.gameObject}'s Start");
            yield return new WaitForSeconds(0.5f);
            yield return StartTurn();
        }
        
        public override IEnumerator StartTurn()
        {
            Debug.Log($"{player.gameObject}'s Start Turn");
            yield return new WaitForSeconds(0.2f);
            yield return TurnLogic();
        }
        
        public override IEnumerator TurnLogic()
        {
            Debug.Log($"{player.gameObject}'s Turn Logic");
            yield return new WaitForSeconds(0.2f);
            DamageInfo _damageInfo = new DamageInfo(45, player.gameObject, DamageType.Physical);
            enemy.OnDamage.Invoke(_damageInfo);
            yield return new WaitForSeconds(0.2f);
            yield return EndTurn();
        }
        
        public override IEnumerator EndTurn()
        { 
            Debug.Log($"{player.gameObject}'s End Turn Logic");
            if (enemy.healthComponent.CurrentHp > 0) yield return StateMachine.ChangeState(BattleManager.EnemyTurnState);
            else yield return StateMachine.ChangeState(BattleManager.EndState);
        }

        public override IEnumerator Exit()
        {
            Debug.Log($"{player.gameObject}'s Exit State");
            yield break;
        }
    }
}