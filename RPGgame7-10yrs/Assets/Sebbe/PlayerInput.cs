using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private Rigidbody2D rb;
    public float movespeed = 10f;
    public float jumpForce = 20f;
    public bool doublejump;
    private float jumpCutMultiplier = 0.5f;


    [Header("GroundSettings")]
    public Transform groundCheck;
    public float checkRadius = 0.2f;
    public LayerMask whatIsGround;
    public LayerMask whatIsEnemy;
   

    private int jumpcount;
    private bool isGrounded;
    private float horizontalInput;
    private float originalGravity; //Original gravity scale, saves on launch

    // DASH VARIABLES
   
    private float dashCD = 0f;
    private float dashDuration = 0.15f; // How long the dash lasts
    private float dashTimer = 0f;      //Internal timer for the burst
    public float dashMultiplier = 3f;  //How much faster the dash is

    private KeyCode MoveLeft, MoveRight, Jump, Dash, Attack;
    public int PlayerNumber;

    [Header("AttackSettings")]
    public string WeaponType;
    private float MeleeDuration;
    
   
    public Transform projectileSpawner;
    public GameObject MeleeAttack;
    public GameObject MageAttack;
   
    public float AttackCD;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        originalGravity = rb.gravityScale;

        if (PlayerNumber == 1)
        {
            MoveLeft = KeyCode.A; MoveRight = KeyCode.D; Jump = KeyCode.W; Dash = KeyCode.LeftShift; Attack = KeyCode.Q;
        }
        else
        {
            MoveLeft = KeyCode.J; MoveRight = KeyCode.L; Jump = KeyCode.I; Dash = KeyCode.RightShift; Attack = KeyCode.O;
        }

        if (WeaponType == "melee")
        {

        }
        else if (WeaponType == "archer")
        {

        }
        else if (WeaponType == "mage")
        {

        }
    }

    void Update()
    {
        // Gather Input
       
        horizontalInput = 0;

        if (Input.GetKey(MoveRight))
        {
            horizontalInput = 1;
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else if (Input.GetKey(MoveLeft))
        {
            horizontalInput = -1;
            transform.eulerAngles = new Vector3(0, 180, 0);
        }

        //Cooldown Timers
        if (dashCD > 0) dashCD -= Time.deltaTime;
        if (dashTimer > 0) dashTimer -= Time.deltaTime;
        if (AttackCD > 0)
        {
            AttackCD -= Time.deltaTime;
        }
        else
        {
            AttackCD = 0; // Keep it at exactly 0 when finished
        }
        if (MeleeDuration > 0) MeleeDuration -= Time.deltaTime;

        //Ground Check
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
        if (isGrounded && rb.linearVelocity.y <= 0.1f)
        {
            jumpcount = doublejump ? 2 : 1;
        }

        //Jump Logic
        if (Input.GetKeyDown(Jump) && jumpcount > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumpcount--;
        }
        //If player only taps jump, falls faster
        if (Input.GetKeyUp(Jump) && rb.linearVelocity.y > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * jumpCutMultiplier);
        }

        //Trigger Dash
        if (Input.GetKeyDown(Dash) && dashCD <= 0)
        {
            rb.gravityScale = 0;
            rb.linearVelocityY = 0f;
            dashTimer = dashDuration; // Start the burst
            dashCD = 2f;    // Reset the 2s cooldown
            
        }
        //Attack logic
        if (Input.GetKeyDown(Attack) && AttackCD <= 0)
        {
            if (WeaponType == "melee")
            {
                MeleeDuration = 0.1f;
                AttackCD = 0.2f;
                MeleeAttack.SetActive(true);
                



            }
            else if (WeaponType == "archer")
            {

            }
            else if (WeaponType == "mage")
            {
                if (MageAttack != null && projectileSpawner != null)
                {
                    AttackCD = 0.5f;
                    Instantiate(MageAttack, projectileSpawner.position, projectileSpawner.rotation);
                }
            }
        }
        if (MeleeDuration <= 0)
        {
            MeleeAttack.SetActive(false);
        }
       


       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & whatIsEnemy) != 0)
        {
            Debug.Log("Hit an enemy: " + collision.name);
            // Do damage here! 
        }
    }

    void FixedUpdate()
    {
        // If we are currently in the a dash
        if (dashTimer > 0)
        {
            // Lock movement to dash speed
            rb.linearVelocity = new Vector2(horizontalInput * movespeed * dashMultiplier, rb.linearVelocity.y);
        }
        else
        {
            // Normal movement & gravity
            rb.gravityScale = originalGravity;
            rb.linearVelocity = new Vector2(horizontalInput * movespeed, rb.linearVelocity.y);
        }
    }
}