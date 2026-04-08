using Unity.VisualScripting;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public bool jumpingistrue;
    public bool attackingistrue;
    public bool groundedistrue;
    public bool climbingistrue;
    private float atkdur;

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
    private float originalGravity;
    private bool isClimbing;

    private float dashCD = 0f;
    private float dashDuration = 0.15f;
    private float dashTimer = 0f;
    public float dashMultiplier = 3f;

    private KeyCode MoveLeft, MoveRight, Jump, Dash, Attack, Rage;
    public int PlayerNumber;

    [Header("AttackSettings")]
    public string WeaponType;
    private float MeleeDuration;
    public float AttackDamage;

    public Transform projectileSpawner;
    public GameObject MeleeAttack;
    public GameObject HeavyAttack;
    public GameObject MageAttack;
    public GameObject ArcherAttack;
    private bool backtonormal;
    public float AttackCD;

    Quaternion rotation;
    public enum ItemEnum { Bag, Book, Boots, Helmet, Poison, Robe, Shield, Skull }
    public ItemEnum Item;
    private string itemtype;
    private string playerClass;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        originalGravity = rb.gravityScale;

        playerClass = gameObject.GetComponent<PlayerStatsScript>().stats.Class.ToString();
        if (playerClass == "Swordsman") WeaponType = "melee";
        else if (playerClass == "Archer") WeaponType = "archer";
        else if (playerClass == "Brute") WeaponType = "heavy";
        else if (playerClass == "Sorcerer") WeaponType = "mage";

        string itemtype = gameObject.GetComponent<ItemStatsScript>().Item.Item.ToString();

        if (itemtype == "Bag") bonusscoremult = 0.5f;
        else if (itemtype == "Book") dashcdreduction = 0.1f;
        else if (itemtype == "Boots") doublejump = true;
        else if (itemtype == "Helmet") bonusdef = 0.5f;
        else if (itemtype == "Poison") bonusatk = 0.5f;
        else if (itemtype == "Robe") bonusdashdist = 0.5f;
        else if (itemtype == "Shield") shield = true;
        else if (itemtype == "Skull") ragemode = true;
    }

    void Update()
    {
        string pNum = PlayerNumber.ToString();
        horizontalInput = Input.GetAxisRaw("Horizontal" + pNum);

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

        if (dashCD > 0) dashCD -= Time.deltaTime;
        if (dashTimer > 0) dashTimer -= Time.deltaTime;
        if (AttackCD > 0) AttackCD -= Time.deltaTime;
        if (MeleeDuration > 0) MeleeDuration -= Time.deltaTime;
        if (atkdur > 0) atkdur -= Time.deltaTime;
        if (ragecd > 0) ragecd -= Time.deltaTime;
        if (ragedur > 0) ragedur -= Time.deltaTime;

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
        groundedistrue = isGrounded;

        if (isGrounded && rb.linearVelocity.y <= 0.1f)
        {
            canClimb = true;
            jumpcount = doublejump ? 2 : 1;
        }

        isClimbing = Physics2D.OverlapCircle(wallCheck.position, 0.5f, whatIsWall);

        if (isClimbing && wallclimb && horizontalInput != 0 && canClimb)
        {
            climbingistrue = true;
            rb.gravityScale = 0;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 10f);
        }
        else if (isClimbing && wallclimb && canClimb)
        {
            climbingistrue = false;
            canClimb = false;
        }
        else if (!isClimbing)
        {
            climbingistrue = false;
        }

        if (Input.GetButtonDown("Jump" + pNum) && jumpcount > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumpcount--;
        }

        jumpingistrue = !isGrounded && rb.linearVelocity.y > 0.1f;

        if (Input.GetButtonUp("Jump" + pNum) && rb.linearVelocity.y > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * jumpCutMultiplier);
        }

        if (Input.GetButtonDown("Dash" + pNum) && dashCD <= 0 && dash)
        {
            rb.gravityScale = 0;
            rb.linearVelocityY = 0f;
            dashTimer = dashDuration + bonusdashdist;
            dashCD = 2f - dashcdreduction;
        }

        if (Input.GetButtonDown("Rage" + pNum) && ragemode && ragecd <= 0)
        {
            ragedur = 5f;
            ragecd = 5;
            AttackDamage += 1f;
            movespeed += 1f;
            backtonormal = true;
        }

        if (ragedur <= 0 && backtonormal)
        {
            AttackDamage -= 1f;
            movespeed -= 1f;
            backtonormal = false;
        }

        if (Input.GetButtonDown("Attack" + pNum) && AttackCD <= 0)
        {
            attackingistrue = true;
            if (WeaponType == "melee")
            {
                MeleeDuration = 0.1f;
                AttackCD = 0.2f;
                MeleeAttack.SetActive(true);
            }
            else if (WeaponType == "heavy")
            {
                MeleeDuration = 0.4f;
                AttackCD = 0.4f;
                HeavyAttack.SetActive(true);
            }
            else if (WeaponType == "archer")
            {
                atkdur = 0.2f;
                AttackCD = 0.3f;
                Instantiate(ArcherAttack, projectileSpawner.position, rotation);
            }
            else if (WeaponType == "mage")
            {
                atkdur = 0.2f;
                AttackCD = 0.5f;
                Instantiate(MageAttack, projectileSpawner.position, projectileSpawner.rotation);
            }
        }

        if (MeleeDuration <= 0 && atkdur <= 0)
        {
            attackingistrue = false;
            MeleeAttack.SetActive(false);
            HeavyAttack.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & whatIsEnemy) != 0)
        {
            Debug.Log("Hit an enemy: " + collision.name);
        }
    }

    void FixedUpdate()
    {
        if (dashTimer > 0)
        {
            rb.linearVelocity = new Vector2(horizontalInput * movespeed * dashMultiplier, rb.linearVelocity.y);
        }
        else
        {
            rb.gravityScale = originalGravity;
            rb.linearVelocity = new Vector2(horizontalInput * movespeed, rb.linearVelocity.y);
        }
    }
}