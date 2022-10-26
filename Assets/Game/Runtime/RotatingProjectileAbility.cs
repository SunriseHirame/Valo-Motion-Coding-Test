using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Object = UnityEngine.Object;

[Serializable]
public class RotatingProjectileAbility : ProjectileAbility
{
    [SerializeField] private float m_rotationRate = 360f;

    private float _distanceFromOwner;
    
    public override void Bind (GameObject owner, Transform spawnPoint)
    {
        base.Bind (owner, spawnPoint);

        _distanceFromOwner = (owner.transform.position - spawnPoint.position).magnitude;
    }

    public override void OnUpdate (PlayerInput playerInput)
    {
        var castInputPressed = playerInput.actions[m_castInputActionName].IsPressed ();

        if (castInputPressed)
        {
            _timeSinceCastStart += Time.deltaTime;

            if (!_isCasting)
            {
                CreateNewProjectile ();
                _isCasting = true;
            }
            
            RotateProjectileAroundOwner (_projectileInstance, Time.deltaTime);
        }
        else if (_isCasting)
        {
            _isCasting = false;

            playerInput.StartCoroutine (HandleDelayedLaunch (_projectileInstance));

            _projectileInstance = default;
            _timeSinceCastStart = 0;
        }
    }

    private static readonly WaitForFixedUpdate s_waitForFixedUpdate = new WaitForFixedUpdate ();
    private IEnumerator HandleDelayedLaunch (Projectile projectile)
    {
        var previousAngle = float.MinValue;
        
        while (true)
        {
            RotateProjectileAroundOwner (projectile, Time.deltaTime);
            
            var launchDirection = _projectileSpawn.right;
            var projectileDirectionFromOwner = (projectile.transform.position - _owner.transform.position).normalized;
            
            var angle = Vector3.SignedAngle (launchDirection, projectileDirectionFromOwner, Vector3.up);
            
            // Detect the case when the projectile loops around.
            if (previousAngle > 0 && angle < 0f)
            {
                break;
            }

            previousAngle = angle;

            // I added wait for fixed so the position is handled properly
            yield return s_waitForFixedUpdate;
        }

        var launchPosition = _owner.transform.position - _projectileSpawn.right * _distanceFromOwner;
        launchPosition.y = _projectileSpawn.position.y;
        
        projectile.SetPosition (launchPosition);
        LaunchProjectile (projectile);
    }
    
    private void RotateProjectileAroundOwner (Projectile projectile, float deltaTime)
    {
        // I changed the way the projectiles rotation is applied. This makes it easier to handle cases where the world
        // relative start offset changes. Applying it as delta rotation removes the need to track the original offset.
        
        // var phase = -(m_rotationRate * timeSinceCastStart) * Mathf.Deg2Rad;
        // var direction = new Vector3 (Mathf.Cos (phase), 0, Mathf.Sin (phase));
        var ownerPosition = _owner.transform.position;
        var currentDirectionFromOwner = (projectile.transform.position - ownerPosition).normalized;

        var frameRotation = Quaternion.Euler (0, m_rotationRate * deltaTime, 0);
        var newDirectionFromOwner = frameRotation * currentDirectionFromOwner;
        var newPosition = newDirectionFromOwner * _distanceFromOwner;
        
        projectile.SetPosition (ownerPosition + newPosition);
    }
    
    private void CreateNewProjectile ()
    {
        _projectileInstance = Object.Instantiate (m_projectile, _projectileSpawn.position, _projectileSpawn.rotation);
        _projectileInstance.DisableProjectile ();
    }
}