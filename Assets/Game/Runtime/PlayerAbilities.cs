using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAbilities : MonoBehaviour
{
    [SerializeField] private Transform m_projectileSpawnPoint;
    
    [SerializeField] private ScalingProjectileAbility m_scalingProjectileAbility;
    [SerializeField] private RotatingProjectileAbility m_rotatingProjectileAbility;

    public Vector3 ProjectileSpawnPosition => m_projectileSpawnPoint.position;
    
    private void Awake ()
    {
        m_scalingProjectileAbility.Bind (gameObject, m_projectileSpawnPoint);
        m_rotatingProjectileAbility.Bind (gameObject, m_projectileSpawnPoint);
    }

    public void OnUpdate (PlayerInput playerInput)
    {
        m_scalingProjectileAbility.OnUpdate (playerInput);
        m_rotatingProjectileAbility.OnUpdate (playerInput);
    }

}
