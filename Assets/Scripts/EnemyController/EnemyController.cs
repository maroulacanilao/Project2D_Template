using System;
using EnemyController.StateMachine;
using NaughtyAttributes;
using UnityEngine;

namespace EnemyController
{
    [RequireComponent(typeof(Rigidbody2D), typeof(CapsuleCollider2D))]
    public class EnemyController : UnitController
    {
        [field: SerializeField] public Transform[] patrolWaypoints { get; private set; }
        [field: SerializeField] public PlayerDetector playerDetector { get; private set; }
        [field: SerializeField] public float patrolSpeed { get; private set; } = 8;
        [field: SerializeField] public float chaseSpeed { get; private set; } = 8;
        
        [field: SerializeField] public float attackRange { get; private set; } = 1;
        [field: SerializeField] public float chaseDistance { get; private set; } = 8;

        #region Animation Hash

        [field: AnimatorParam("animator")] [field: Foldout("Animation Hash")]
        [field: SerializeField]  public string xSpeedHash { get; private set; }
    
        [field: AnimatorParam("animator")] [field: Foldout("Animation Hash")]
        [field: SerializeField]  public string groundedHash { get; private set; }
        
        [field: AnimatorParam("animator")] [field: Foldout("Animation Hash")]
        [field: SerializeField]  public string attackHash { get; private set; }

        [field: AnimatorParam("animator")] [field: Foldout("Animation Hash")]
        [field: SerializeField]  public string hitHash { get; private set; }
        
        [field: AnimatorParam("animator")] [field: Foldout("Animation Hash")]
        [field: SerializeField]  public string deathHash { get; private set; }
    
        #endregion

        private void Awake()
        {
            StateMachine = new EnemyStateMachine(this);
        }

        private void OnEnable()
        {
            StateMachine.Initialize();
        }
        
    }
}
