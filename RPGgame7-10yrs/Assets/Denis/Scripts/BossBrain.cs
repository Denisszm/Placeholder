using Unity.VisualScripting;
using UnityEngine;

public class BossBrain : MonoBehaviour
{
    [Header("Confined Space")]
    public float minX = -10f;
    public float maxX = 10f;

    private float initialScaleX;
    public Collider2D Collider;
    [Header("BossStats")]
    [SerializeField] public float speed = 5f;
    [SerializeField] public float attackRange = 2f;
    [SerializeField] public int HP = 10000;
    public BossStates currentBossState;
    public BossPhases currentBossPhase;
    public float lastAttack = 0;
    [SerializeField] public float attackCooldown = 10;
    [SerializeField] public float tranistionAttackCooldown = 0.5f;
    [SerializeField] public float attackWindUp = 0.2f;
    [SerializeField] public float hitboxDuration = 0.1f;
    public int totalJumpAttacks = 0;
    public int jumpAttacksBeforeTongue = 3;
    public float jumpHeight = 8f;
    [Header("BossStates & Bools")]
    public bool startBossfight;
    public bool areThereFlies;
    public bool istTransitionFinished;
    public bool isIdle => currentBossState == BossStates.Idle;
    public bool isJumping => currentBossState == BossStates.Jumping;
    public bool isLanding => currentBossState == BossStates.Landing;
    public bool isTongueAttacking => currentBossState == BossStates.TongoueAttack;
    public bool isEating => currentBossState == BossStates.Eating;
    public bool isPhaseTransition => currentBossState == BossStates.PhaseTransition;
    public bool isDead => currentBossState == BossStates.Dead;
    public bool inPhase1 => currentBossPhase == BossPhases.Phase1;
    public bool inPhase2 => currentBossPhase == BossPhases.Phase2;
    public enum BossStates
    {
        Idle,
        Jumping,
        Landing,
        TongoueAttack,
        Eating,
        PhaseTransition,
        Dead
    }
    public enum BossPhases
    {
        Phase1,
        Phase2
    }
    void Start()
    {
        initialScaleX = transform.localScale.x;
        currentBossState = BossStates.Idle;
        currentBossPhase = BossPhases.Phase1;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color32(170, 74, 68, 255);
        Vector3 center = new Vector3((minX + maxX) / 2, transform.position.y, 0);
        Vector3 size = new Vector3(maxX - minX, 1, 0);
        Gizmos.DrawWireCube(center, size);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            startBossfight = true;
            currentBossPhase = BossPhases.Phase1;
            Debug.Log("Player in range!");
        }

    }
    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Ground"))
    //    {
    //        currentBossState = BossStates.Landing;
    //    }
    //}
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
    public bool canAttack()
    {
        return Time.time > lastAttack + attackCooldown;
    }
    void CheckForFlies()
    {
        GameObject[] flies = GameObject.FindGameObjectsWithTag("Fly");
        if (flies.Length > 0)
        {
            areThereFlies = true;
        }
        else
        {
            areThereFlies = false;
        }
    }
    public void TakeDamage(int amount)
    {
        if (isDead) return;

        HP -= amount;
        Debug.Log(gameObject.name + " took damage! Current HP: " + HP);

        if (HP <= 0)
        {
            HP = 0;
            currentBossState = BossStates.Dead;
            Destroy(gameObject);
        }
    }
    void Update()
    {
        if (!startBossfight || isDead)
        {
            return;
        }
        if (startBossfight)
        {
            if (HP <= 0)
            {
                currentBossState = BossStates.Dead;
                return;

            }
            if (istTransitionFinished && currentBossPhase != BossPhases.Phase2)
            {
                currentBossPhase = BossPhases.Phase2;
                attackCooldown = 10f;
                lastAttack = Time.time;
            }
            if (HP <= 3000 && inPhase1 && !istTransitionFinished)
            {
                if (!isPhaseTransition)
                {
                    currentBossState = BossStates.PhaseTransition;
                    attackCooldown = tranistionAttackCooldown;
                }
                return;
            }
            if (isIdle && canAttack())
            {
                ExecuteNextMove();
            }
        }
    }
    void ExecuteNextMove()
    {
        lastAttack = Time.time;

        if (inPhase1)
        {
            currentBossState = BossStates.Jumping;
        }
        else if (inPhase2)
        {
            CheckForFlies();
            if (totalJumpAttacks < jumpAttacksBeforeTongue)
            {
                currentBossState = BossStates.Jumping;
                totalJumpAttacks++;
            }
            else if (areThereFlies)
            {
               
                currentBossState = BossStates.Eating;
                totalJumpAttacks = 0;
            }
            else
            {
                
                currentBossState = BossStates.TongoueAttack;
                totalJumpAttacks = 0;
            }
        }
    }
}
