using Character;
using UnityEngine;

public abstract class UnitController : MonoBehaviour
{
    [field: Header("Components")]
    [field: SerializeField] public Rigidbody2D rb { get; protected set; }
    [field: SerializeField] public Animator animator { get; protected set; }
    [field: SerializeField] public SpriteRenderer spriteRenderer { get; protected set; }
    [field: SerializeField] public MovementCollisionDetector collisionDetector { get; private set; }
    [field: SerializeField] public AnimationEventInvoker animationEventInvoker { get; private set; }
    
    [field: SerializeField] public HealthComponent healthComponent { get; private set; }
    
    protected UnitStateMachine StateMachine;
    
    protected virtual void Update()
    {
        StateMachine.StateUpdate();
    }
    
    protected virtual void FixedUpdate()
    {
        StateMachine.StateFixedUpdate();
    }
}
