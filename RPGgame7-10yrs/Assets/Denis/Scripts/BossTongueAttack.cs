using UnityEngine;
using System.Collections;
using System.Threading;
using UnityEngine.InputSystem.Controls;
public class BossTongueAttack : MonoBehaviour
{
    private BossBrain brain;
    private bool isAttacking = false;

    [Header("TongueSettings")]
    [SerializeField] public float tongueSpeed = 20f;
    [SerializeField] public float incriment = 1.2f;
    [SerializeField] public float maxLenght = 25f;
    [SerializeField] public float stayTime = 0.5f;
    [SerializeField] public int damage = 150;
    [SerializeField] public int flyAmmount = 3;

    [Header("Prefabs")]
    [SerializeField] public GameObject tonguePrefab;
    [SerializeField] public GameObject annoyingFly;
    private Rigidbody2D rb;
    private Animator anim;

    public Transform tongueOrigin;
    void Start()
    {
        brain = GetComponent<BossBrain>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

   
    void Update()
    {
        if ((brain.isTongueAttacking || brain.isEating) && !isAttacking)
        {
            rb.linearVelocity = Vector3.zero;
            StartCoroutine(TongueSequence());
        }
    }

    IEnumerator TongueSequence()
    {
        anim.SetBool("doJump", false);
        anim.SetBool("doLand", false);
        anim.SetBool("doIdle", true);
        isAttacking = true;
        Transform target = null;
        if ( brain.isEating)
        {
            GameObject[] flies = GameObject.FindGameObjectsWithTag("Fly");
            target = GetClosest(flies);
        }
        else
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            target = GetClosest(players);
        }
        if (target == null)
        {
            isAttacking = false;
            yield break;
        }
        if ( brain.isEating)
        {
            if (target.GetComponent<FlySwarming>())
            {
                target.GetComponent<FlySwarming>().enabled = false;
            }
        }
        Vector3 direction = (target.position - tongueOrigin.position).normalized;
        brain.LookAt(target.position);
        anim.SetBool("doAttack", true);
        anim.SetBool("doIdle", false);
        GameObject tongue = Instantiate(tonguePrefab, tongueOrigin.position, Quaternion.identity);
        if ( brain.isEating)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            tongue.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
        else
        {
            float horizontalDir = 180;
            if (target.position.x > transform.position.x)
            {
                horizontalDir = 0;
            }
            tongue.transform.rotation = Quaternion.Euler(0, 0, horizontalDir);
        }

        float timer = 0;
        float targetDistance = Vector2.Distance(tongueOrigin.position, target.position);
        targetDistance += 3f;

        if (targetDistance > maxLenght)
        {
            targetDistance = maxLenght;
        }
        if(!brain.isEating)
        {
            for (int i = 0; i < flyAmmount; i++)
            {

                GameObject fly = Instantiate(annoyingFly, tongueOrigin.position, Quaternion.identity);
                Rigidbody2D flyRb = fly.GetComponent<Rigidbody2D>();
                if (flyRb != null)
                {
                    Vector2 popDir = new Vector2(Random.Range(-1f, 1f), Random.Range(0.5f, 1.5f)).normalized;
                    flyRb.AddForce(popDir * 5f, ForceMode2D.Impulse);
                }
            }
        }
        while (timer < 0.2f)
        {
            timer += Time.deltaTime;
            float progress = timer / 0.2f;

            float currentX = Mathf.Lerp(0, targetDistance, progress);
            tongue.transform.localScale = new Vector3(currentX, 1, 1);
            if (!brain.isEating)
            {
            }
            yield return null;
        }

        if (brain.isEating && target != null)
        {
            target.SetParent(tongue.transform);
        }

        while (timer > 0)
        {
            timer -= Time.deltaTime * 2f; 
            float progress = timer / 0.2f;
            float currentX = Mathf.Lerp(0, targetDistance, progress);
            tongue.transform.localScale = new Vector3(currentX, 1, 1);
            yield return null;
        }

        if (brain.isEating)
        {
            if (target != null)
            {
                Destroy(target.gameObject);
            }
        }

        Destroy(tongue);
        anim.SetBool("doAttack", false);
        anim.SetBool("doIdle", true);
        if (!brain.isEating)
        {
            brain.currentBossState = BossBrain.BossStates.Idle;
            isAttacking = false;
        }
        else
        {
            brain.currentBossState = BossBrain.BossStates.Idle;
            isAttacking = false;
        }
            
    }
    Transform GetClosest(GameObject[] objects)
    {
        Transform bestTarget = null;
        float closestDist = Mathf.Infinity;
        foreach (GameObject obj in objects)
        {
            float dist = Vector2.Distance(transform.position, obj.transform.position);
            if (dist < closestDist)
            {
                closestDist = dist;
                bestTarget = obj.transform;
            }
        }
        return bestTarget;
    }
}
