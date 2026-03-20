using UnityEngine;



public class PlayerInput : MonoBehaviour

{
    // Item variables

    private float bonusscoremult;
    private float dashcdreduction;
    private float bonusdef;
    private float bonusatk;
    private float bonusdashdist;
    private bool shield;
    private bool ragemode;
    private float ragecd;
    private float ragedur;

    [Header("ScoreStuff")]
    public float scoremultiplier;
    

    private Rigidbody2D rb;

    [Header("MoveSpeeds")]
    public float movespeed = 10f;

    public float jumpForce = 20f;

    

    private float jumpCutMultiplier = 0.5f;
    [Header("player bools")]

    public bool doublejump;
    public bool dash;
    public bool wallclimb;


    [Header("EnviormentSettings")]

    public Transform groundCheck;

    public Transform wallCheck;

    public float checkRadius = 0.2f;

    public LayerMask whatIsGround;

    public LayerMask whatIsEnemy;

    public LayerMask whatIsWall;

    public CapsuleCollider2D playerHitbox;





    private int jumpcount;

    private bool canClimb;

    private bool isGrounded;

    private float horizontalInput;

    private float originalGravity; //Original gravity scale, saves on launch

    private bool isClimbing;



    // DASH VARIABLES



    private float dashCD = 0f;

    private float dashDuration = 0.15f; //How long the dash lasts

    private float dashTimer = 0f;      //Internal timer for the dash

    public float dashMultiplier = 3f;  //How much faster the dash is than normal speed



    private KeyCode MoveLeft, MoveRight, Jump, Dash, Attack, Rage;

    public int PlayerNumber;



    [Header("AttackSettings")]

    public string WeaponType;

    private float MeleeDuration;

    public float AttackDamage;





    public Transform projectileSpawner;

    public GameObject MeleeAttack;

    public GameObject MageAttack;

    public GameObject ArcherAttack;

    private bool backtonormal;

    public float AttackCD;

    Quaternion rotation;



    void Start()

    {

        rb = GetComponent<Rigidbody2D>();

        rb.freezeRotation = true;

        originalGravity = rb.gravityScale;

        string itemtype = gameObject.GetComponent<ItemStatsScript>().Item.Item.ToString();

        if (itemtype == "Bag")
        {
            bonusscoremult =+ 0.5f;
        }
        else if (itemtype == "Book")
        {
            dashcdreduction = 0.1f;
        }
        else if (itemtype == "Boots")
        {
            doublejump = true;
        }
        else if (itemtype == "Helmet")
        {
            bonusdef = 0.5f;
        }
        else if (itemtype == "Poison")
        {
            bonusatk = 0.5f;
        }
        else if (itemtype == "Robe")
        {
            bonusdashdist = 0.5f;
        }
        else if (itemtype == "Shield")
        {
            shield = true;
        }
        else if (itemtype == "Skull")
        {
            ragemode = true;
        }

    }



    void Update()

    {
        ragemode = true;
        //GATHER INPUTS

        //Changes with playernumber

        string pNum = PlayerNumber.ToString();



        horizontalInput = Input.GetAxisRaw("Horizontal" + pNum);



        //HANDLE ROTATION & DIRECTION

        if (horizontalInput > 0.1f) //Moving Right

        {

            transform.eulerAngles = new Vector3(0, 0, 0);

            rotation = Quaternion.Euler(0, 0, 15);

        }

        else if (horizontalInput < -0.1f) //Moving Left

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
        if (ragecd > 0) ragecd -= Time.deltaTime;
        if (ragedur > 0) ragedur -= Time.deltaTime;



        //GROUND CHECK

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

        if (isGrounded && rb.linearVelocity.y <= 0.1f)

        {
            canClimb = true;
            jumpcount = doublejump ? 2 : 1;

        }

        isClimbing = Physics2D.OverlapCircle(wallCheck.position, 0.5f, whatIsWall);

        if (isClimbing && wallclimb && horizontalInput != 0 && canClimb)
        {

            rb.gravityScale = 0;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 10f);

        }
        else if(isClimbing && wallclimb && canClimb)
        {
            canClimb = false;
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

        if (Input.GetButtonDown("Dash" + pNum) && dashCD <= 0 && dash)

        {

            rb.gravityScale = 0;

            rb.linearVelocityY = 0f;

            dashTimer = dashDuration + bonusdashdist;

            dashCD = 2f - dashcdreduction;

        }

        //RAGE MODE LOGIC
        if (Input.GetButtonDown("Rage" + pNum) && ragemode && ragecd !> 0)
        {
            AttackDamage =+ 1f;
            movespeed =+ 1f;
            ragedur = 5f;

            backtonormal = true;
        }
        if (ragedur <= 0 && backtonormal)
        {
            AttackDamage -= 1f;
            movespeed -= 1f;
            backtonormal = false;
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



        Debug.Log($"speed = {movespeed} damage = {AttackDamage}");

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