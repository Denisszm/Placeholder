using System.Collections;
using UnityEngine;

public class EnemyAttacking : MonoBehaviour
{
    private EnemyBase brain;
    private GameObject hitbox;
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
            StartCoroutine(Attack());
            brain.currentState = EnemyStates.Idle;
        }
    }
    IEnumerator Attack()
    {
        yield return new WaitForSeconds(brain.attackWindUp);
        hitbox.SetActive(true);
        yield return new WaitForSeconds(brain.hitboxDuration);
        hitbox.SetActive(false);
    }
    
}
