using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleData : ScriptableObject
{
    public CharacterBase playerCharacter { get; private set; }
    public CharacterBase enemyCharacter { get; private set; }
    public int playerHp { get; private set; }
    public int playerMana { get; private set; }
    
    public bool IsPlayerFirst { get; private set; }

    private void ResetData()
    {
        playerCharacter = null;
        enemyCharacter = null;
    }

    public void EnterBattle(CharacterBase player_, CharacterBase enemy_, bool isPlayerFirst_)
    {
        if (player_ == null)
        {
            Debug.LogWarning("Player is null");
            return;
        }
        if (enemy_ == null)
        {
            Debug.LogWarning("Enemy is null");
        }

        playerCharacter = player_;
        enemyCharacter = enemy_;
        playerHp = player_.healthComponent.CurrentHp;
        playerMana = player_.manaComponent.CurrentMana;
        IsPlayerFirst = isPlayerFirst_;
    }
}
