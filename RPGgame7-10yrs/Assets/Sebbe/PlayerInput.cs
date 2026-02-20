using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private Rigidbody2D rb;
    public float movespeed = 10f;
    public float jumpForce = 20f;
    public bool doublejump;
    private float jumpCutMultiplier = 0.5f;

    public Transform groundCheck;
    public float checkRadius = 0.2f;
    public LayerMask whatIsGround;

    private int jumpcount;
    private bool isGrounded;
    private float horizontalInput;
    private bool dashCD = true;

    private KeyCode MoveLeft, MoveRight, Jump, Dash;
    public int PlayerNumber;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // Fryser rotation så spelaren inte trillar framåt
        rb.freezeRotation = true;

        if (PlayerNumber == 1)
        {
            MoveLeft = KeyCode.A; MoveRight = KeyCode.D; Jump = KeyCode.W; Dash = KeyCode.LeftShift;
        }
        else
        {
            MoveLeft = KeyCode.J; MoveRight = KeyCode.L; Jump = KeyCode.I; Dash = KeyCode.RightShift;
        }
    }

    void Update()
    {
        // 1. Samla input i Update
        horizontalInput = 0;
        if (Input.GetKey(MoveRight)) horizontalInput = 1;
        if (Input.GetKey(MoveLeft)) horizontalInput = -1;
        

        // 2. Markkontroll
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

        if (isGrounded && rb.linearVelocity.y <= 0.1f)
        {
            jumpcount = doublejump ? 2 : 1;
        }

        // 3. Hopp (GetKeyDown måste vara i Update för att inte missas)
        if (Input.GetKeyDown(Jump) && jumpcount > 0)
        {
            
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumpcount--;
        }
        if (Input.GetKeyUp(Jump))
        {
            // Om vi rör oss uppåt (velocity.y > 0)
            if (rb.linearVelocity.y > 0)
            {
                // Multiplicera nuvarande Y-fart med t.ex. 0.5. 
                // Detta gör att man "tappar suget" uppåt och börjar falla tidigare.
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * jumpCutMultiplier);
            }
        }
        if (dashCD)
        {
            if (Input.GetKeyDown(Dash))
            {
                rb.linearVelocityX = rb.linearVelocityX * 2;
                dashCD = false;
                System.Threading.Thread.Sleep(2000);
                dashCD = true;
            }
        }

    }

    void FixedUpdate()
    {
        // 4. Applicera rörelse i FixedUpdate (Fysik-vänligt)
        // Om ingen knapp trycks, sätt X till 0 direkt för att eliminera "back-momentum"
        if (horizontalInput == 0)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }
        else
        {
            rb.linearVelocity = new Vector2(horizontalInput * movespeed, rb.linearVelocity.y);
        }
        
    }

    
}