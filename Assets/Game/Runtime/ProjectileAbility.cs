using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public abstract class ProjectileAbility
{
    [SerializeField] protected string m_castInputActionName = "cast";
    [SerializeField] protected Projectile m_projectile;
    
    
    protected Transform _projectileSpawn;
    protected GameObject _owner;
    
    protected bool _isCasting;
    protected float _timeSinceCastStart;
    protected Projectile _projectileInstance;

    public virtual void Bind (GameObject owner, Transform spawnPoint)
    {
        _projectileSpawn = spawnPoint;
        _owner = owner;
    }

    public abstract void OnUpdate (PlayerInput playerInput);
    
    protected void LaunchProjectile (Projectile projectile)
    {
        projectile.Launch (_projectileSpawn.forward, _owner);
        projectile = default;
    }
    
}