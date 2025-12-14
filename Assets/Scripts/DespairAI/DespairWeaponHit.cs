using UnityEngine;

public class DespairWeaponHit : MonoBehaviour
{
    public float damage = 30f;
    public string targetTag = "Player";

    private Collider hitCollider;
    private bool canHit;

    void Awake()
    {
        hitCollider = GetComponent<Collider>();
        hitCollider.isTrigger = true;
        hitCollider.enabled = false; // off by default
    }

    // Called by animation event at the start of the strike
    public void ActivateHit()
    {
        canHit = true;
        hitCollider.enabled = true;
    }

    // Called by animation event at the end of the strike
    public void DeactivateHit()
    {
        canHit = false;
        hitCollider.enabled = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!canHit) return;
        if (!other.CompareTag(targetTag)) return;

        DespairSceneHealth health = other.GetComponent<DespairSceneHealth>();
        if (health != null)
        {
            health.TakeDamage(damage);
            // damage only once per collision
        }
    }
}
