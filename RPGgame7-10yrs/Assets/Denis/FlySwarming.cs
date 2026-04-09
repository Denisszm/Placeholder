using UnityEngine;

public class FlySwarming : MonoBehaviour
{
    public float speed = 6f;
    public float orbitRadius = 1.5f;
    public float orbitSpeed = 5f;
    public int health = 3;

    private Transform targetPlayer;
    private float angle;
    void Start()
    {
        
    }
    void Update()
    {
        FindFarthestPlayer();

        if (targetPlayer != null)
        {
            angle += Time.deltaTime * orbitSpeed;
            Vector3 offset = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * orbitRadius;
            Vector3 swarmPoint = targetPlayer.position + offset;

            transform.position = Vector2.MoveTowards(transform.position, swarmPoint, speed * Time.deltaTime);
        }
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
}
