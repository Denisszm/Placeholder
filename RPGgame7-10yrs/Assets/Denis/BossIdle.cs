using UnityEngine;

public class BossIdle : MonoBehaviour
{
    public GameObject pointA;
    public GameObject pointB;
    public GameObject enemy;
    private Rigidbody2D rb;
    private Animator anim;
    private Transform currentPoint;
    private BossBrain brain;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentPoint = pointB.transform;
        anim.SetBool("isIdle", true);
        brain = GetComponent<BossBrain>();
    }
    void Update()
    {
        if (brain == null)
        {
            brain = GetComponent<BossBrain>();
            return;
        }
        if (brain.isIdle)
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

            if (Vector2.Distance(transform.position, currentPoint.position) < 3f && currentPoint == pointB.transform)
            {
                currentPoint = pointA.transform;
            }
            brain.LookAt(currentPoint.position);
            if (Vector2.Distance(transform.position, currentPoint.position) < 3f && currentPoint == pointA.transform)
            {
                currentPoint = pointB.transform;
            }
            brain.LookAt(currentPoint.position);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.purple;
        Gizmos.DrawWireSphere(pointA.transform.position, 3f);
        Gizmos.DrawWireSphere(pointB.transform.position, 3f);
        Gizmos.DrawLine(pointA.transform.position, pointB.transform.position);
        Gizmos.color = Color.forestGreen;


    }
}
