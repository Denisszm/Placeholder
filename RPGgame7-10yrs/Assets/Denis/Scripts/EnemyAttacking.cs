using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemyAttacking : MonoBehaviour
{
    private EnemyBase brain;
    public GameObject hitbox;
    [Header("Ranged Settings")]
    public GameObject arrowPrefab;
    public Transform shotPoint;
    void Start()
    {
        brain = GetComponent<EnemyBase>();
    }

    
    void Update()
    {
        if (brain == null)
        {
            brain = GetComponent<EnemyBase>();
            return;
        }
        if (brain.isAttacking)
        {
            brain.currentState = EnemyStates.Idle;
            StartCoroutine(Attack());
        }
    }
    IEnumerator Attack()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        GameObject closestPlayer = null;
        float closestDistance = Mathf.Infinity;
        foreach (GameObject player in players)
        {
            float dist = Vector2.Distance(transform.position, player.transform.position);
            if (dist < closestDistance)
            {
                closestDistance = dist;
                closestPlayer = player;
            }
        }
        if (closestPlayer != null)
        {
            brain.LookAt(closestPlayer.transform.position);
        }

        float actualWindUp = brain.attackWindUp;

        if ( gameObject.CompareTag("RangedEnemy"))
        {
            actualWindUp = brain.attackWindUp * 3;
        }
        yield return new WaitForSeconds(actualWindUp);
        if (gameObject.CompareTag("MeleeEnemy"))
        {
            ExecuteMelee();
        }
        else if (gameObject.CompareTag("RangedEnemy"))
        {
            ExecuteRanged(closestPlayer);
        }
        yield return new WaitForSeconds(brain.hitboxDuration);
        if (hitbox != null)
        {
            hitbox.SetActive(false);
        }
        brain.currentState = EnemyStates.Idle;
    }
    void ExecuteMelee()
    {
        if (hitbox != null)
        {
            hitbox.SetActive(true);
        }
    }
    void ExecuteRanged(GameObject target)
    {
        if ( target == null)
        {
            return;
        }
        GameObject arrow = Instantiate(arrowPrefab, shotPoint.transform.position, Quaternion.identity);
        Vector2 direction = (target.transform.position - shotPoint.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        arrow.transform.rotation = Quaternion.Euler(0, 0, angle);
        Rigidbody2D arrowRb = arrow.GetComponent<Rigidbody2D>();
        arrowRb.linearVelocity = direction * 10f;
    }
}
