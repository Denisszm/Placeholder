using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private Rigidbody2D rb;
    public int movespeed = 10;
    public bool doublejump = true;
    public int jumpcount;
    public float jumpForce = 20f;
    public Transform groundCheck; // Drag an empty GameObject here (at player's feet)
    public float checkRadius = 0.2f;
    public LayerMask whatIsGround; // Set this to "Ground" in the Inspector
    private bool isGrounded;
    public KeyCode MoveLeft;
    public KeyCode MoveRight;
    public KeyCode Jump;
    public int PlayerNumber;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (PlayerNumber == 1)
        {
            MoveLeft = KeyCode.A;
            MoveRight = KeyCode.D;
            Jump = KeyCode.Space;
        }
        if (PlayerNumber == 2)
        {
            MoveLeft = KeyCode.LeftArrow;
            MoveRight = KeyCode.RightArrow;
            Jump = KeyCode.UpArrow;
        }
       
    }

    // Update is called once per frame
    void Update()
    {

        
        transform.rotation = Quaternion.identity;
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

        if (Input.GetKey(MoveRight))
        {
            rb.linearVelocity = new Vector2(movespeed, rb.linearVelocity.y);
        }
        else if (Input.GetKey(MoveLeft))
        {
            rb.linearVelocity = new Vector2(movespeed * -1, rb.linearVelocity.y);
        }
        else
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }
        
            if (isGrounded && rb.linearVelocity.y <= 0.01f)
            {
                if (doublejump)
                {
                  jumpcount = 2;

               
            }
                else
                {
                  jumpcount = 1;
                }
            }

        if (Input.GetKeyDown(Jump) && jumpcount > 0)
            {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);

            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

            jumpcount--;

        }
        
        

            

    }

    private void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            // If isGrounded is true, draw Green. Otherwise, draw Red.
            Gizmos.color = isGrounded ? Color.green : Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
        }
    }
}
