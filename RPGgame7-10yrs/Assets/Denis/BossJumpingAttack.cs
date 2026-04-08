using System.Collections;
using System.Linq;
using UnityEngine;

public class BossJumpingAttack : MonoBehaviour
{
    private BossBrain brain;
    public GameObject Shockwave;
    public Rigidbody2D rb;
    private bool isExecutingSequence = false;
    public Transform floor;

    [Header("Sequence Settings")]
    [SerializeField] public float launchUpSpeedY = 30f;
    [SerializeField] public float launchUpSpeedX = 5f;
    [SerializeField] public float slamGravity = 10f;
    [SerializeField] public float reactionTime = 0.4f;
    [SerializeField] public float suspence = 4;

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
        if ( brain.isJumping && !isExecutingSequence)
        {
            StartCoroutine(JumpSequence());
        }
    }
    IEnumerator JumpSequence()
    {
        isExecutingSequence = true;
        float targetx = 0;
        if ( brain.inPhase2)
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            GameObject player = players[Random.Range(0, players.Length)];
            targetx = player.transform.position.x + Random.Range(-20, 20);
        }
        else
        {
            targetx = Random.Range(brain.minX, brain.maxX);
        }
        targetx = Mathf.Clamp(targetx, brain.minX, brain.maxX);
        rb.linearVelocity = new Vector2(launchUpSpeedY, launchUpSpeedX);
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
        yield return new WaitForSeconds(suspence);

        Vector3 warningPos = new Vector3(targetx, floor.position.y + 4f, 0);
        GameObject warning = Instantiate(warningPrefab, warningPos, Quaternion.identity);
        yield return new WaitForSeconds(reactionTime);

        Destroy(warning);
        rb.gravityScale = slamGravity;
        while (!brain.isLanding)
        {
            yield return null;
        }
        if (brain.isLanding)
        {
            Instantiate(shockwavePrefab, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(0.5f);

            rb.gravityScale = 3f;
            rb.linearVelocity = Vector2.zero;
            brain.currentBossState = BossBrain.BossStates.Idle;
            isExecutingSequence = false;
        }



    }
}
