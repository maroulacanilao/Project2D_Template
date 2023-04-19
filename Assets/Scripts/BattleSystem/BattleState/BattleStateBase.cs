using System.Collections;


public abstract class BattleStateBase
{
    protected BattleManager BattleManager;
    protected BattleStateMachine StateMachine;
    protected CharacterBase player;
    protected CharacterBase enemy;
    
    public BattleStateBase(BattleManager battleManager_, BattleStateMachine stateMachine_)
    {
        BattleManager = battleManager_;
        StateMachine = stateMachine_;
        player = battleManager_.player;
        enemy = battleManager_.enemy;
    }
    
    public abstract IEnumerator Enter();

    public abstract IEnumerator Exit();
}
