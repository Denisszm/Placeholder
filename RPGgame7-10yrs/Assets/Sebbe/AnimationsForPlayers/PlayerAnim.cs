using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer; // This was missing

    [Header("References")]
    public PlayerInput playerInputScript;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // This was missing

        if (playerInputScript == null)
            playerInputScript = GetComponent<PlayerInput>();
    }

    void Update()
    {
        if (playerInputScript == null) return;

        // 1. Movement Speed
        // We create 'horizontalVelocity' here so the flip code below can use it
        float horizontalVelocity = rb.linearVelocity.x;
        float horizontalSpeed = Mathf.Abs(horizontalVelocity);
        anim.SetFloat("Speed", horizontalSpeed);

        

        // 2. State Bools
        anim.SetBool("Grounded", playerInputScript.groundedistrue);
        anim.SetBool("Attacking", playerInputScript.attackingistrue);
        anim.SetBool("Climbing", playerInputScript.climbingistrue);
        anim.SetBool("Jumping", playerInputScript.jumpingistrue);
    }
}