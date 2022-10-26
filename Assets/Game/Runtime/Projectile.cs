using UnityEngine;
using UnityEngine.Events;

[RequireComponent (typeof (Rigidbody))]
public class Projectile : MonoBehaviour
{
    [SerializeField] private Rigidbody m_rigidbody;
    [SerializeField] private Collider m_hitTrigger;
    [SerializeField] private float m_projectileSpeed = 20f;

    [SerializeField] private UnityEvent m_onLaunched;
    
    private GameObject _owner;
    
    public void Launch (Vector3 direction, GameObject owner)
    {
        _owner = owner;

        var normalizedLaunchDirection = direction.normalized;

        // Face the launch direction.
        transform.forward = normalizedLaunchDirection;
        
        m_rigidbody.isKinematic = false;
        m_hitTrigger.enabled = true;
        m_rigidbody.velocity = normalizedLaunchDirection * m_projectileSpeed;
        
        m_onLaunched.Invoke ();
    }
    
    public void SetPosition (Vector3 worldSpacePosition)
    {
        m_rigidbody.position = worldSpacePosition;
    }

    public void SetPositionAndRotation (Vector3 position, Quaternion rotation)
    {
        m_rigidbody.position = position;
        m_rigidbody.rotation = rotation;
    }
    
    public void DisableProjectile ()
    {
        m_rigidbody.isKinematic = true;
        m_hitTrigger.enabled = false;
    }
    
    private void OnTriggerEnter (Collider other)
    {
        Debug.Log ($"hit: {other}");
        if (other.attachedRigidbody && other.attachedRigidbody.gameObject == _owner) return;
        
        // TODO Add hit logic
    }

    private void Reset ()
    {
        m_rigidbody = GetComponent<Rigidbody> ();
    }
}
