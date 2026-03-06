using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private Animator playersprite;
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
    private float dashDuration = 0.15f; //How long the dash lasts
    private float dashTimer = 0f;      //Internal timer for the dash
    public float dashMultiplier = 3f;  //How much faster the dash is than normal speed

    private KeyCode MoveLeft, MoveRight, Jump, Dash, Attack;
    public int PlayerNumber;

    [Header("AttackSettings")]
    public string WeaponType;
    private float MeleeDuration;
    
   
    public Transform projectileSpawner;
    public GameObject MeleeAttack;
    public GameObject MageAttack;
    public GameObject ArcherAttack;
   
    public float AttackCD;
    Quaternion rotation;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playersprite = GetComponent<Animator>();
        rb.freezeRotation = true;
        originalGravity = rb.gravityScale;     
    }

    void Update()
    {
        // 1. GATHER INPUTS
        string pNum = PlayerNumber.ToString();
        // GetAxisRaw returns exactly -1, 0, or 1
        horizontalInput = Input.GetAxisRaw("Horizontal" + pNum);

        // 2. ANIMATION UPDATE (Moved this up for better tracking)
        // We use Mathf.Abs so that moving left (-1) still sends "1" to the Speed parameter
        float moveSpeedForAnimator = Mathf.Abs(horizontalInput);
        playersprite.SetFloat("Speed", moveSpeedForAnimator);

        // This will help you see if the code is actually "hearing" your keyboard
        if (moveSpeedForAnimator > 0)
        {
            Debug.Log("Player " + pNum + " is moving! Speed value: " + moveSpeedForAnimator);
        }

        // 3. HANDLE ROTATION & DIRECTION
        if (horizontalInput > 0.1f)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            rotation = Quaternion.Euler(0, 0, 15);
        }
        else if (horizontalInput < -0.1f)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
            rotation = Quaternion.Euler(0, 0, 165);
        }

        //COOLDOWNS
        if (dashCD > 0) dashCD -= Time.deltaTime;
        if (dashTimer > 0) dashTimer -= Time.deltaTime;
        if (AttackCD > 0) AttackCD -= Time.deltaTime;
        else AttackCD = 0;
        if (MeleeDuration > 0) MeleeDuration -= Time.deltaTime;

        //GROUND CHECK
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
        if (isGrounded && rb.linearVelocity.y <= 0.1f)
        {
            jumpcount = doublejump ? 2 : 1;
        }

        //JUMP LOGIC
        if (Input.GetButtonDown("Jump" + pNum) && jumpcount > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumpcount--;
        }

        if (Input.GetButtonUp("Jump" + pNum) && rb.linearVelocity.y > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * jumpCutMultiplier);
        }

        //DASH LOGIC
        if (Input.GetButtonDown("Dash" + pNum) && dashCD <= 0)
        {
            rb.gravityScale = 0;
            rb.linearVelocityY = 0f;
            dashTimer = dashDuration;
            dashCD = 2f;
        }

        //ATTACK LOGIC
        if (Input.GetButtonDown("Attack" + pNum) && AttackCD <= 0)
        {
            if (WeaponType == "melee")
            {
                MeleeDuration = 0.1f;
                AttackCD = 0.2f;
                MeleeAttack.SetActive(true);
            }
            else if (WeaponType == "archer")
            {
                if (ArcherAttack != null && projectileSpawner != null)
                {
                    AttackCD = 0.3f;
                    Instantiate(ArcherAttack, projectileSpawner.position, rotation);
                }
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
        playersprite.SetFloat("Speed", Mathf.Abs(horizontalInput));


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & whatIsEnemy) != 0)
        {
            Debug.Log("Hit an enemy: " + collision.name);
            //Do damage here! 
        }
    }

    void FixedUpdate()
    {
        //If we are currently in the a dash
        if (dashTimer > 0)
        {
            //Lock movement to dash speed
            rb.linearVelocity = new Vector2(horizontalInput * movespeed * dashMultiplier, rb.linearVelocity.y);
        }
        else
        {
            //Normal movement & gravity
            rb.gravityScale = originalGravity;
            rb.linearVelocity = new Vector2(horizontalInput * movespeed, rb.linearVelocity.y);
        }
    }
}