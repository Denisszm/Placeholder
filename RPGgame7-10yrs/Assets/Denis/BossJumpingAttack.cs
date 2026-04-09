using System.Collections;
using System.Linq;
using UnityEngine;

public class BossJumpingAttack : MonoBehaviour
{
    private BossBrain brain;
    public Rigidbody2D rb;
    private bool isExecutingSequence = false;
    public Transform floor;

    [Header("Sequence Settings")]
    [SerializeField] public float launchUpSpeedY = 30f;
    [SerializeField] public float launchUpSpeedX = 5f;
    [SerializeField] public float slamGravity = 10f;
    [SerializeField] public float reactionTime = 0.4f;
    [SerializeField] public float suspence = 4;
    [SerializeField] public float transitionSuspence = 0f;

    public int jumpCount = 1;

    [Header("Prefabs")]
    public GameObject warningPrefab;
    public GameObject shockwavePrefab;
    void Start()
    {
        brain = GetComponent<BossBrain>();
    }
    void Update()
    {
        if( brain == null)
        {
            brain = GetComponent<BossBrain>();
            rb = GetComponent<Rigidbody2D>();
        }
        if ( brain.isJumping && !isExecutingSequence || brain.isPhaseTransition && !isExecutingSequence)
        {
            StartCoroutine(JumpSequence());
            Debug.Log("I am gonna jump");
        }
    }
    IEnumerator JumpSequence()
    {
        bool wasTranisition = brain.isPhaseTransition;
        Debug.Log("i set tranision");
        isExecutingSequence = true;
        if (wasTranisition)
        {
            Debug.Log("I have twenty jumps");
            jumpCount = 5;
        }
        else
        {
            Debug.Log("i have one jump");
            jumpCount = 1;
        }
        for ( int i = 0; i < jumpCount; i++)
        {
           
            Debug.Log("I am jumping");
            brain.currentBossState = BossBrain.BossStates.Jumping;
            float targetx = 0;
            if (brain.inPhase2)
            {
                Debug.Log("I am finding player");
                GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
                GameObject player = players[Random.Range(0, players.Length)];
                targetx = player.transform.position.x + Random.Range(-2.5f, 2.5f);
            }
            else
            {
                Debug.Log("I am jumping randomly");
                targetx = Random.Range(brain.minX, brain.maxX);
            }
            targetx = Mathf.Clamp(targetx, brain.minX, brain.maxX);

            rb.linearVelocity = new Vector2(0, launchUpSpeedY);
            Debug.Log("I am jumping over camera");
            float cameraTopEdge = Camera.main.transform.position.y + Camera.main.orthographicSize;
            float offScreenTarget = cameraTopEdge + 2f;
            while (transform.position.y < offScreenTarget)
            {
                Debug.Log("I am under Camera");
                yield return null;
            }
            float dropHeight = cameraTopEdge + 50f;
            Debug.Log("I am teleporting");
            transform.position = new Vector3(targetx, dropHeight, 0);
            rb.linearVelocity = Vector2.zero;
            rb.gravityScale = 0.0f;
            if (wasTranisition)
            {
                Debug.Log("I am faster");
                
            }
            else
            {
                Debug.Log("I am slower");
                yield return new WaitForSeconds(suspence);
            }

            Debug.Log("I am spawning warning");
            Vector3 warningPos = new Vector3(targetx, floor.position.y + 1f, 0);
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
            Debug.Log("I destroyed warning");
            rb.gravityScale = slamGravity;
            Debug.Log("I am heavy");
            brain.currentBossState = BossBrain.BossStates.Jumping;
            yield return new WaitForSeconds(0.2f);
            float lastY = transform.position.y;
            bool grounded = false;
            while (!grounded)
            {
                yield return new WaitForSeconds(0.05f); // Check every few frames
                if (Mathf.Abs(transform.position.y - lastY) < 0.01f)
                {
                    grounded = true;
                }
                lastY = transform.position.y;
            }
            Debug.Log("I landed");
            brain.currentBossState = BossBrain.BossStates.Landing;
            rb.gravityScale = 3f;
            rb.linearVelocity = Vector2.zero;
            GameObject shockwave = Instantiate(shockwavePrefab, transform.position - new Vector3(0f, 1.6f), Quaternion.identity);
            yield return new WaitForSeconds(0.2f);
        }
        yield return new WaitForSeconds(0.5f);
        if (wasTranisition)
        {
            brain.istTransitionFinished = true;
        }
        Debug.Log("I am done for now");
        brain.currentBossState = BossBrain.BossStates.Idle;
        isExecutingSequence = false;
    }
}
