using UnityEngine;

public class MageAttacks : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public float speed = 10f;
    public float lifetime = 3f;
    public LayerMask whatisEnemy;
    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        

        if (((1 << collision.gameObject.layer) & whatisEnemy) != 0)
        {
            Debug.Log("Hit an enemy: " + collision.name);
            // Do damage here! 
        }
    }
}
