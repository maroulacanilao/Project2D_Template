using System.Collections;

public enum TurnPhase 
{
    Start,
    Turn,
    End,
}
namespace BattleSystem.BattleState
{
    public abstract class TurnBaseState : BattleStateBase
    {
        public TurnBaseState(BattleManager battleManager_, BattleStateMachine stateMachine_) : base(battleManager_, stateMachine_)
        {
            
        }
        public override abstract IEnumerator Enter();
        public abstract IEnumerator StartTurn();
        public abstract IEnumerator TurnLogic();
        public abstract IEnumerator EndTurn();
        public override abstract IEnumerator Exit();
    }
}