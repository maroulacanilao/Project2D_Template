using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class BattleCharacterController : MonoBehaviour
{
    [SerializeField] private Transform station;
    [SerializeField] private Transform oppositeStation;
    [SerializeField] private Animator _animator;

    public List<AnimationClip> anim;
    private NavMeshAgent agent;
    private Rigidbody rb;

    private void Start()
    {
        agent = GetComponentInChildren<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
    }

    private IEnumerator GoToPosition(Vector3 targetPosition)
    {
        agent.destination = targetPosition;
        var fixedUpdate = new WaitForFixedUpdate();
        while (Vector3.Distance(transform.position, targetPosition) > 2f)
        {
            yield return fixedUpdate;
        }
        agent.isStopped = true;
        yield break;
    }
}