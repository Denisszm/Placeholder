using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private Rigidbody2D rb;
    private Transform boom;
    public int movespeed = 10;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
      //  boom.rotation = new Vector3(0, 0,0);
      
        if (Input.GetKey(KeyCode.D))
        {
            rb.linearVelocity = new Vector2(movespeed, rb.linearVelocity.y);
        }
        if (Input.GetKey(KeyCode.A))
        {
            rb.linearVelocity = new Vector2(movespeed * -1, rb.linearVelocity.y);
        }
        else
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }

    }
}
