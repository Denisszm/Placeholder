using UnityEngine;

public class DoTheDamage : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] public bool canTakeDamage = true;
    [SerializeField] public int Damage = 100;

    private void OnTriggerEnter2D(Collider2D other)
    {
        HandleHit(other.gameObject);
    }
    private void OnTriggerColliderr2D(Collider2D other)
    {
        HandleHit(other.gameObject);
    }
    private void HandleHit(GameObject hitObject)
    {
        DoTheDamage myId = GetComponentInParent<DoTheDamage>();
        DoTheDamage victimId = hitObject.GetComponentInParent<DoTheDamage>(); // :)

        if ( victimId == null || !canTakeDamage)
        {
            return;
        }
        bool iAmPlayer = GetComponentInParent<PlayerInput>() != null;
        bool theyArePlayer = GetComponentInParent<PlayerInput>() != null;

        if ( iAmPlayer == theyArePlayer)
        {
            return;
        }

        hitObject.SendMessageUpwards("TakeDamage", Damage, SendMessageOptions.DontRequireReceiver);
    }

    void Update()
    {
        
    }
    
    
}
