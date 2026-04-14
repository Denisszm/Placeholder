using System.Collections;
using System.Linq;
using UnityEngine;

public class BossJumpingAttack : MonoBehaviour
{
    private BossBrain brain;
    public Rigidbody2D rb;
    private bool isExecutingSequence = false;
    private Animator anim;
    public Transform floor;

    [Header("Sequence Settings")]
    [SerializeField] public float launchUpSpeedY = 30f;
    [SerializeField] public float launchUpSpeedX = 5f;
    [SerializeField] public float slamGravity = 10f;
    [SerializeField] public float reactionTime = 0.4f;
    [SerializeField] public float suspence = 2f;
    [SerializeField] public float transitionSuspence = 0f;

    public int jumpCount = 1;

    [Header("Prefabs")]
    public GameObject warningPrefab;
    public GameObject shockwavePrefab;
    void Start()
    {
        brain = GetComponent<BossBrain>();
        anim = GetComponent<Animator>();
    }
    void Update()
    {
        if( brain == null)
        {
            brain = GetComponent<BossBrain>();
            rb = GetComponent<Rigidbody2D>();
            anim = GetComponent<Animator>();
        }
        if ( brain.isJumping && !isExecutingSequence || brain.isPhaseTransition && !isExecutingSequence)
        {
            isExecutingSequence = true;
            rb.linearVelocity = Vector3.zero;
            StartCoroutine(JumpSequence());
        }
    }
    IEnumerator JumpSequence()
    {
        bool wasTranisition = brain.isPhaseTransition;
        isExecutingSequence = true;
        if (wasTranisition)
        {
            jumpCount = 12;
        }
        else
        {
            jumpCount = 1;
        }
        for (int i = 0; i < jumpCount; i++)
        {

            anim.SetBool("doJump", true);
            anim.SetBool("doIdle", false);
            anim.SetBool("doLand", false);
            float targetx = 0;
            if (brain.inPhase2)
            {
                GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
                GameObject player = players[Random.Range(0, players.Length)];
                targetx = player.transform.position.x + Random.Range(-15f, 15f);
            }
            else
            {
                targetx = Random.Range(brain.minX, brain.maxX);
            }
            targetx = Mathf.Clamp(targetx, brain.minX, brain.maxX);
            rb.linearVelocity = new Vector2(0, launchUpSpeedY);
            yield return new WaitForSeconds(0.667f);
            float cameraTopEdge = Camera.main.transform.position.y + Camera.main.orthographicSize;
            float offScreenTarget = cameraTopEdge + 2f;
            while (transform.position.y < offScreenTarget)
            {
                yield return null;
            }
            float dropHeight = cameraTopEdge + 50f;
            transform.position = new Vector3(targetx, dropHeight, 0);
            rb.linearVelocity = Vector2.zero;
            rb.gravityScale = 0.0f;
            if (wasTranisition)
            {
                yield return new WaitForSeconds(suspence / 1000);
            }
            else
            {
                yield return new WaitForSeconds(suspence);
            }

            Vector3 warningPos = new Vector3(targetx, floor.position.y + 2.6f, 0);
            GameObject warning = Instantiate(warningPrefab, warningPos, Quaternion.identity);
            if (wasTranisition)
            {
                yield return new WaitForSeconds(reactionTime / 4);
                warning.SetActive(false);
                yield return new WaitForSeconds(reactionTime / 8);
                warning.SetActive(true);
                yield return new WaitForSeconds(0.05f);
            }
            else
            {
                yield return new WaitForSeconds(reactionTime);
                warning.SetActive(false);
                yield return new WaitForSeconds(reactionTime / 2);
                warning.SetActive(true);
                yield return new WaitForSeconds(0.2f);
            }
            Destroy(warning);
            rb.gravityScale = slamGravity;
            yield return new WaitForSeconds(0.2f);
            float lastY = transform.position.y;
            bool grounded = false;
            while (!grounded)
            {
                yield return new WaitForSeconds(0.05f);
                if (Mathf.Abs(transform.position.y - lastY) < 0.01f)
                {
                    grounded = true;
                }
                lastY = transform.position.y;
            }
            anim.SetBool("doJump", false);
            anim.SetBool("doIdle", false);
            anim.SetBool("doLand", true);
            brain.currentBossState = BossBrain.BossStates.Landing;
            rb.gravityScale = 3f;
            rb.linearVelocity = Vector2.zero;
            GameObject shockwave = Instantiate(shockwavePrefab, transform.position - new Vector3(0f, 1.6f), Quaternion.identity);
            yield return new WaitForSeconds(0.3f);
            anim.SetBool("doJump", false);
            anim.SetBool("doLand", false);
            anim.SetBool("doIdle", true);
        }
        yield return new WaitForSeconds(0.5f);
        if (wasTranisition)
        {
            brain.istTransitionFinished = true;
        }
        anim.SetBool("doIdle", true);
        brain.currentBossState = BossBrain.BossStates.Idle;
        isExecutingSequence = false;
    }
}
