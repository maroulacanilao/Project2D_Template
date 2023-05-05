using CustomHelpers;
using NaughtyAttributes;
using UnityEngine;

public class MovementCollisionDetector : MonoBehaviour
{
    [Header("Ground")]
    [SerializeField] private Vector2 groundOffset;
    [SerializeField] private float groundRadius;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] [Tag] private string movingPlatformTag;
    [SerializeField] private bool updateGrounded = true;

    [Header("Edge")]
    [SerializeField] private Vector2 edgeGroundOffset;
    [SerializeField] private float groundCheckDist;
    [SerializeField] private bool updateEdge = true;

    [Header("Wall")]
    [SerializeField] private Vector2 wallOffset;
    [SerializeField] private float wallCheckDist;
    [SerializeField] private bool useExcludeLayer = true;
    [SerializeField] [ShowIf("useExcludeLayer")] LayerMask wallExcludeLayer;
    [SerializeField] [HideIf("useExcludeLayer")] LayerMask wallLayer;
    [SerializeField] private bool updateWall = true;

    [Header("DrawGizmo")]
    public bool showGizmo = true;

    private int movingPlatformTagHash;

    public Transform GroundTransform { get; private set; }
    public bool isOnEdge { get; private set; }
    public bool isTouchingWall { get; private set; }
    public bool isGrounded { get; private set; }
    public bool isOnMovingPlatform { get; private set; }

    private float xScaleModifier => Mathf.Sign(transform.parent.localScale.x);
    
    private void Awake()
    {
        movingPlatformTagHash = movingPlatformTag.ToHash();
        
    }

    private void Update()
    {
        Physics2D.queriesHitTriggers = false;
        if (updateGrounded) isGrounded = IsGrounded();
        if (updateWall) isTouchingWall = IsTouchingWall();
        if (updateEdge) isOnEdge = IsOnEdge();
    }

    Vector3 GetOffsetOrientation(Vector3 _offset)
    {
        _offset.x *= xScaleModifier;
        return _offset;
    }

    public bool IsOnEdge()
    {
        var _offset = edgeGroundOffset;
        _offset.x *= xScaleModifier;
        return !Physics2D.Raycast(transform.position + GetOffsetOrientation(edgeGroundOffset), Vector2.down, groundCheckDist, groundLayer);
    }

    public bool IsTouchingWall()
    {
        if (useExcludeLayer)
        {
            return Physics2D.Raycast(transform.position + GetOffsetOrientation(wallOffset), 
                Vector2.right, wallCheckDist, ~wallExcludeLayer);
        }
        
        return Physics2D.Raycast(transform.position + GetOffsetOrientation(wallOffset), 
            Vector2.right, wallCheckDist, wallLayer);
    }

    public bool IsGrounded()
    {
        var _col = Physics2D.OverlapCircle(transform.position + (Vector3) groundOffset, groundRadius, groundLayer);
        if (!_col)
        {
            isGrounded = false;
            isOnMovingPlatform = false;
            return false;
        }
        isGrounded = true;
        GroundTransform = _col.transform;
        isOnMovingPlatform = _col.CompareTagHash(movingPlatformTagHash);
        return true;
    }

    private void OnDrawGizmos()
    {
        if (!showGizmo) return;
        var _pos = transform.position;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_pos + (Vector3) groundOffset, groundRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawRay(_pos + GetOffsetOrientation(edgeGroundOffset), Vector2.down * groundCheckDist);

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(_pos + GetOffsetOrientation(wallOffset), Vector2.right * wallCheckDist);
    }
}
