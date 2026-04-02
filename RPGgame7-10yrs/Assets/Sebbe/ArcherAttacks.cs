using UnityEngine;

public class ArcherAttacks : MonoBehaviour
{

    public float speed = 10f;
    public float lifetime = 3f;
    public LayerMask whatIsEnemy;
    private Rigidbody2D rb;


    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        Destroy(gameObject, lifetime);
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = transform.right * speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (rb.linearVelocity.sqrMagnitude > 0.1f) // Only rotate if it's actually moving
        {
            float angle = Mathf.Atan2(rb.linearVelocity.y, rb.linearVelocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {


        if (((1 << collision.gameObject.layer) & whatIsEnemy) != 0)
        {
            Debug.Log("Hit an enemy: " + collision.name);
            Destroy(gameObject);
            // Do damage here! 
        }
    }
}
