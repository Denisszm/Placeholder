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
    private float initialScaleX;
    public Collider2D Collider;
    [Header("State Bools")]
    public bool isPlayerInRange;
    public bool isAttacking => currentState == EnemyStates.Attacking;
    public bool isDead => currentState == EnemyStates.Dead;
    public bool isPatrolling => currentState == EnemyStates.Patrolling;
    public bool canAttack()
    {
        return Time.time > lastAttack + attackCooldown;
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
        initialScaleX = transform.localScale.x;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            Debug.Log("Player in range!");
        }

    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            Debug.Log("Player in range!");
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            Debug.Log("Player in not range!");
        }
    }
    public void LookAt(Vector2 targetPosition)
    {
        if (targetPosition.x > transform.position.x)
        {
            transform.localScale = new Vector3(-initialScaleX, transform.localScale.y, 1);
        }
        else
        {
            transform.localScale = new Vector3(initialScaleX, transform.localScale.y, 1);
        }
    }
    void Update()
    {
        
        if (HP <= 0)
        {
            currentState = EnemyStates.Dead;
        }
        else if (isPlayerInRange)
        {
            if ( !isAttacking)
            {
                if (canAttack())
                {
                    currentState = EnemyStates.Attacking;
                    lastAttack = Time.time;
                }
                else
                {
                    currentState = EnemyStates.Idle;
                }
            }
        }
        else
        {
            currentState = EnemyStates.Patrolling;
        }

    }
}
