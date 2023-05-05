using System;
using System.Collections;
using System.Collections.Generic;
using Character;
using CustomEvent;
using Skills;
using UnityEngine;

public class SkillUser : MonoBehaviour
{
    [field: SerializeField] public int maxSkillSlot = 4; 
    private List<SkillBase> skills;
    private SkillBase activeSkill;

    private Transform skillContainer;
    
    public CharacterBase character { get; private set; }
        
    private readonly Evt<SkillUser, SkillBase> OnAddSkill = new Evt<SkillUser, SkillBase>();

    private void Awake()
    {
        character = GetComponent<CharacterBase>();
        skills = new List<SkillBase>(maxSkillSlot);
        skillContainer = new GameObject("SkillContainer").transform;
        skillContainer.SetParent(transform);
        skillContainer.localPosition = Vector3.zero;
    }

    public IEnumerator UseSkill(int index_)
    {
        if(skills[index_] == null) yield break;
        
        var _skill = skills[index_];
        
        if(_skill.manaCost > character.manaComponent.CurrentMana) yield break;

        yield return _skill.Activate();
    }

    public void AddSkill(SkillBase skill_, int slot_)
    {
        if (skills[slot_] == null)
        {
            Destroy(skills[slot_].gameObject);
        }

        skills[slot_] = Instantiate(skill_).Initialize(this);
        OnAddSkill.Invoke(this,skills[slot_]);
        if(!skills[slot_].isPassive) return;
        skills[slot_].StartCoroutine(skills[slot_].Activate());
    }

    public void RemoveSkill(int slot_)
    {
        skills[slot_].RemoveSkill();
        skills[slot_] = null;
    }
    
    public void RemoveSkill(SkillBase skill_)
    {
        int _slot = skills.IndexOf(skill_);
        RemoveSkill(_slot);
    }
}
