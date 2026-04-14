using UnityEngine;

public class FlySwarming : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 8f;
    public float swarmRadius = 2f;
    public float randomPointInterval = 0.5f;

    [Header("Combat")]
    public float damageInterval = 0.02f;

    private Transform targetPlayer;
    private Vector3 currentRandomOffset;
    private float nextRandomTime;
    private float nextDamageTime;
    void Start()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.sortingOrder = 10;
            PickNewRandomPoint();
        }
    }
    void Update()
    {
        FindFarthestPlayer();

        if (targetPlayer != null)
        {
            if (Time.time > nextRandomTime)
            {
                PickNewRandomPoint();
            }
            Vector3 targetPos = targetPlayer.position + currentRandomOffset;
            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
        }
    }
    void PickNewRandomPoint()
    {
        currentRandomOffset = Random.insideUnitCircle * swarmRadius;
        nextRandomTime = Time.time + randomPointInterval;
    }
    void FindFarthestPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        float maxDist = -1;
        Transform farthest = null;

        foreach (GameObject p in players)
        {
            float dist = Vector2.Distance(transform.position, p.transform.position);
            if (dist > maxDist)
            {
                maxDist = dist;
                farthest = p.transform;
            }
        }
        targetPlayer = farthest;
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (Time.time > nextRandomTime)
            {
                currentRandomOffset = -currentRandomOffset.normalized * (swarmRadius * 1.5f);
                nextRandomTime = Time.time + 0.3f;
            }
        }
    }
}
