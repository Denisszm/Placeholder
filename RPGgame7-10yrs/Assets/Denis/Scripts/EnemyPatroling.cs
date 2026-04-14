using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;

public class EnemyPatroling : MonoBehaviour
{
    public GameObject pointA;
    public GameObject pointB;
    public GameObject enemy;
    private Rigidbody2D rb;
    private Animator anim;
    private Transform currentPoint;
    private EnemyBase brain;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentPoint = pointB.transform;
        anim.SetBool("isWalking", true);
        brain  = GetComponent<EnemyBase>();
    }

    
    void Update()
    {
        if (brain == null)
        {
            brain = GetComponent<EnemyBase>();
            return;
        }
        if ( brain.isPatrolling)
        {
            
            Vector2 point = currentPoint.position - transform.position;
            if (currentPoint == pointB.transform)
            {
                rb.linearVelocity = new Vector2(brain.speed, 0);
            }
            else
            {
                rb.linearVelocity = new Vector2(-brain.speed, 0);
            }

            if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == pointB.transform)
            {
                currentPoint = pointA.transform;
                

            }
            if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == pointA.transform)
            {
                currentPoint = pointB.transform;
            }
            brain.LookAt(currentPoint.position);
        }
        if (!brain.isPatrolling)
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.purple;
        Gizmos.DrawWireSphere(pointA.transform.position, 0.5f);
        Gizmos.DrawWireSphere(pointB.transform.position, 0.5f);
        Gizmos.DrawLine(pointA.transform.position, pointB.transform.position);
        Gizmos.color = Color.forestGreen;
       
        
    }
}
