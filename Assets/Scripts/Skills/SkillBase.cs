using System.Collections;
using Character;
using CustomEvent;
using UnityEngine;

namespace Skills
{
	public enum SkillState
	{
		Standby,
		Active,
		Cooldown,
	}

	public abstract class SkillBase : MonoBehaviour
	{
		[field: Header("Variables")]
		[field: SerializeField] public bool isPassive { get; protected set; }
		[field: SerializeField] public int manaCost { get; protected set; }
		[field: SerializeField] public Sprite icon { get; protected set; }
		
		[field: Header("Damage Properties")]
		[field: SerializeField] public DamageType damageType { get; protected set; }
		[field: SerializeField] public int baseDamage { get; protected set; }
		[field: SerializeField] public float cooldownDuration { get; protected set; }
		
		private CharacterBase character;
		
		public float cooldownTimer { get; protected set; }
		public SkillState skillState { get; protected set; }
		public SkillUser user { get; protected set; }

		public readonly Evt<SkillBase, SkillState> OnChangeState = new Evt<SkillBase, SkillState>();

		public SkillBase Initialize(SkillUser user_)
		{
			user = user_;
			character = user.character;
			return this;
		}

		protected abstract IEnumerator OnActivate();

		// Deactivating without cooldown
		protected abstract IEnumerator OnDeactivate();

		protected abstract IEnumerator OnAbilityAttack();

		protected abstract void OnRemoveSkill();
		
		protected virtual void OnTick() { }
		
		// for using the ability
		public IEnumerator Activate()
		{
			if (skillState != SkillState.Standby) yield break;
			
			skillState = SkillState.Active;
			OnChangeState.Invoke(this,skillState);
			yield return OnActivate();
		}

		// for starting cooldown
		public IEnumerator Deactivate()
		{
			character.manaComponent.UseMana(manaCost);
			yield return OnDeactivate(); 
			StartCoroutine(StartCooldown());
		}
		
		private IEnumerator StartCooldown()
		{
			skillState = SkillState.Cooldown;
			OnChangeState.Invoke(this,skillState);
			
			cooldownTimer = 0;

			while (cooldownTimer <= cooldownDuration)
			{
				yield return new WaitForSeconds(0.1f);
				cooldownTimer+= 0.1f;
			}
        
			skillState = SkillState.Standby;
			OnChangeState.Invoke(this,skillState);
		}

		public void RemoveSkill()
		{
			OnRemoveSkill();
			Destroy(gameObject);
		}
	}
}