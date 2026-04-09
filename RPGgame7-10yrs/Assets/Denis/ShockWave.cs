using UnityEngine;
using UnityEngine.Rendering;

public class ShockWave : MonoBehaviour
{
    [Header("ShockwaveSettings")]
    [SerializeField] public float shockwaveSpeed = 20f;
    [SerializeField] public float shockwaveDeceleratio = 1.2f;
    [SerializeField] public float maxRange = 12f;
    [SerializeField] public int shockwaveDamage = 200;

    private float currentSpeed;
    private SpriteRenderer sr;
    public bool isShockwaveStopped = false;
    void Start()
    {
        currentSpeed = shockwaveSpeed;
        sr = GetComponent<SpriteRenderer>();
        transform.localScale = new Vector3(0, 1, 1);
    }
    void Update()
    {
        currentSpeed = Mathf.Max(currentSpeed - (shockwaveDeceleratio * Time.deltaTime));
        float growth = currentSpeed * Time.deltaTime;
        transform.localScale += new Vector3(growth, 0, 0);

        if ( sr != null)
        {
            float alpha = 1f - (transform.localScale.x / maxRange);
            Color c = sr.color;
            c.a = alpha;
            sr.color = c;
        }
        if (transform.localScale.x > maxRange || isShockwaveStopped)
        {
            Destroy(gameObject);
        }
    }
    void OnCollisonEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isShockwaveStopped = true;
        }
    }
}
