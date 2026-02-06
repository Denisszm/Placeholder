using UnityEngine;

public class PlayerInput2 : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private Rigidbody2D rb;
    public int movespeed = 10;

    public float jumpForce = 20f;
    public Transform groundCheck; // Drag an empty GameObject here (at player's feet)
    public float checkRadius = 0.2f;
    public LayerMask whatIsGround; // Set this to "Ground" in the Inspector
    private bool isGrounded;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {


        transform.rotation = Quaternion.identity;
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

        if (Input.GetKey(KeyCode.RightArrow))
        {
            rb.linearVelocity = new Vector2(movespeed, rb.linearVelocity.y);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            rb.linearVelocity = new Vector2(movespeed * -1, rb.linearVelocity.y);
        }
        else
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) && isGrounded && rb.linearVelocity.y < 0.01f)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

        }

    }
}
