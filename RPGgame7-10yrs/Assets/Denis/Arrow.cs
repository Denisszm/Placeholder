using UnityEngine;

public class Arrow : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool hasHit = false;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, 20f);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasHit)
        {
            return;
        }
        if (other.CompareTag("Ground") || other.CompareTag("Wall"))
        {
            StopArrow();
        }
        else if (other.CompareTag("Player"))
        {
            //Sebbe fixa damage logik?
            Destroy(gameObject);
        }
    }
    void StopArrow()
    {
        hasHit = true;
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Static;
        GetComponent<Collider2D>().enabled = false;
        transform.SetParent(null); 
    }

    void Update()
    {
        
    }
}
