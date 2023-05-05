using System;
using System.Collections;
using System.Collections.Generic;
using BattleSystem.BattleState;
using Character;
using UnityEngine;

public class BattleManager : Singleton<BattleManager>
{
    [field: SerializeField] public CharacterBase player { get; private set; }
    [field: SerializeField] public CharacterBase enemy { get; private set; }
    [field: SerializeField] public BattleData battleData { get; private set; }

    public BattleStateMachine BattleStateMachine { get; private set; }

    public BattleStartState StartState { get; private set; }
    public PlayerTurnState PlayerTurnState { get; private set; }
    public EnemyTurnState EnemyTurnState { get; private set; }
    public BattleEndState EndState { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        SetUp();
        BattleStateMachine = new BattleStateMachine();

        StartState = new BattleStartState(this, BattleStateMachine);
        PlayerTurnState = new PlayerTurnState(this, BattleStateMachine);
        EnemyTurnState = new EnemyTurnState(this, BattleStateMachine);
        EndState = new BattleEndState(this, BattleStateMachine);
    }

    private void SetUp()
    {

    }

    private void Start()
    {
        StartCoroutine(BattleStateMachine.Initialize(this, StartState));
    }

    public void End()
    {
        Debug.Log("Battle End");
        StopAllCoroutines();
    }
}