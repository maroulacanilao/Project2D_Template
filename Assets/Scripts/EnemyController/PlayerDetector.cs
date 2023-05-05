using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CustomHelpers;
using NaughtyAttributes;
using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    [SerializeField] private Vector3 offset;
    [SerializeField] private float radius;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] [Tag] private string playerTag;

    public bool isPlayerInRange { get; private set; }
    public bool isPlayerReachable { get; private set; }
    
    private int playerTagHash;
    private Transform owner => transform.parent;
    
    private Vector3 ownerPos => owner.position;
    public GameObject player { get; private set; }
    private void Start()
    {
        playerTagHash = playerTag.ToHash();
    }
    
    private void Update()
    {
        OverlapCircleMethod();
        
    }

    private void OverlapCircleMethod()
    {
        if (!IsPlayerInRange(out var _playerCol))
        {
            isPlayerInRange = false;
            isPlayerReachable = false;
            return;
        }

        isPlayerInRange = true;

        isPlayerReachable = isPlayerInRange; //IsPlayerReachable(_playerCol.transform);
    }

    public bool IsPlayerInRange(out Collider2D colInfo_)
    {
        colInfo_ = Physics2D.OverlapCircle(transform.position + offset, radius, playerLayer);

        if (!colInfo_) return false;

        if (colInfo_.CompareTagHash(playerTagHash))
        {
            player = colInfo_.gameObject;
            return true;
        }
        player = null;
        return false;
    }

    public bool IsPlayerReachable(Transform playerTransform_)
    {
        Physics2D.queriesHitTriggers = false;
        var _direction = playerTransform_.transform.position - ownerPos;

        var _hit = Physics2D.Raycast(transform.position + GetOffsetOrientation(offset), _direction, _direction.magnitude);

        if (!_hit)
        {
            player = null;
            return false;
        }
        if (_hit.transform == playerTransform_)
        {
            player = playerTransform_.transform.gameObject;
            return true;
        }
        player = null;
        return false;
    }
    
    Vector3 GetOffsetOrientation(Vector3 _offset)
    {
        var xScaleModifier = Mathf.Sign(owner.localScale.x);
        _offset.x *= xScaleModifier;
        return _offset;
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + GetOffsetOrientation(offset), radius);
        // Gizmos.color = Color.red;
        // Gizmos.DrawRay(transform.position + offset, Vector2.right * distance);
    }
}
