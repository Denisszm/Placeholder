using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Timeline;
public enum EnemyStates
{
    Patrolling,
    Dead,
    Attacking,
    Idle,
}
public class EnemyBase : MonoBehaviour
{
    [Header("State Bools")]
    public bool isPlayerInRange;
    public bool isAttacking => currentState == EnemyStates.Attacking;
    public bool isDead => currentState == EnemyStates.Dead;
    public bool isPatrolling => currentState == EnemyStates.Patrolling;
    public bool canAttack()
    {
        float totalTime = (float)Time.time;
        if ( totalTime > lastAttack + attackCooldown)
        {
            lastAttack = totalTime;
            return true;
        }
        else
        {
            return false;
        }
    }

    [Header("EnemyStats")]
    [SerializeField] public float speed = 5f;
    [SerializeField] public float attackRange = 2f;
    [SerializeField] public int HP = 10;
    public float lastAttack = 0;
    [SerializeField] public float attackCooldown = 10;
    [SerializeField] public float attackWindUp = 0.2f;
    [SerializeField] public float hitboxDuration = 0.1f;

    [Header("StateSettings")]
    public EnemyStates currentState;

    void Start()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }

    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
        }
    }
    void Update()
    {
        if (isPlayerInRange && canAttack())
        {
            currentState = EnemyStates.Attacking;
        }
        if (!isPlayerInRange)
        {
            currentState = EnemyStates.Patrolling;
        }

    }
}
