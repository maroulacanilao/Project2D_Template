using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleStateMachine
{
    public BattleManager battleManager;
    public BattleStateBase CurrentBattleStateBase;
    
    public IEnumerator Initialize(BattleManager battleManager_, BattleStateBase startBattleStateBase_)
    {
        CurrentBattleStateBase = startBattleStateBase_;
        battleManager = battleManager_;
        yield return startBattleStateBase_.Enter();
    }
    
    public IEnumerator ChangeState(BattleStateBase newStateBase_)
    {
        yield return CurrentBattleStateBase.Exit();
        CurrentBattleStateBase = newStateBase_;
        yield return newStateBase_.Enter();
    }
}
