using UnityEngine;

public class DualPlayerRope : MonoBehaviour
{
    public Transform player2;
    public float maxDistance;
    private Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player2 == null) return;

        float currentDistance = Vector2.Distance(rb.position, player2.position);

        if (currentDistance > maxDistance)
        {
            Vector2 direction = (rb.position - (Vector2)player2.position).normalized;
            rb.position = (Vector2)player2.position + (direction * maxDistance);
        }
    }
}
