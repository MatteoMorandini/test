using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemyAI : MonoBehaviour
{
    public Transform player;

    public float detectionRange = 10f;
    public float attackRange = 2f;
    public float attackCooldown = 2f;
    public float rotationSpeed = 5f;
    public float normalSpeed = 3.5f;
    public float chaseSpeed = 10f;
    private float lastAttackTime = 0f;

    private bool hasSpottedPlayer = false;
    private bool hasEscaped = false;
    private bool isAttacking = false;

    private NavMeshAgent agent;
    private Animator animator;
    private Renderer[] renderers;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        agent.speed = normalSpeed;

        // Get all renderers and disable visibility
        renderers = GetComponentsInChildren<Renderer>();
        SetVisibility(false);
        hasEscaped = false;
    }

    void Update()
    {
        if (!hasSpottedPlayer)
            return;

        if (isAttacking)
            return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            hasEscaped = false;
        }
        else if (!hasEscaped)
        {
            hasEscaped = true;
            FindObjectOfType<PlayerVoice>()?.PlayEscapeDialogue();
        }

        if (distanceToPlayer <= attackRange)
        {
            AttackPlayer();
        }
        else
        {
            agent.speed = chaseSpeed;
            agent.destination = player.position;
            RotateTowardsPlayer();
        }
    }

    public void RevealEnemy()
    {
        if (!hasSpottedPlayer)
        {
            hasSpottedPlayer = true;
            SetVisibility(true);
            FindObjectOfType<PlayerVoice>()?.PlayRandomDialogue();
			
			GetComponent<PCPlayerController>()?.Sprint();
		}
    }

    private void SetVisibility(bool state)
    {
        foreach (Renderer r in renderers)
        {
            r.enabled = state;
        }
    }

    void AttackPlayer()
    {
        if (Time.time - lastAttackTime > attackCooldown)
        {
            agent.destination = transform.position;
            isAttacking = true;
            animator.SetTrigger("Attack");
            RotateTowardsPlayer();
            lastAttackTime = Time.time;
            Invoke(nameof(ResetAttack), 1.5f);
        }
    }

    void RotateTowardsPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }

    void ResetAttack()
    {
        isAttacking = false;
    }
}
