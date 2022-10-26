using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public class ScalingProjectileAbility : ProjectileAbility
{
    [Min (0)]
    [SerializeField] private float m_timeToFullProjectileSize = 2f;
    [SerializeField] private AnimationCurve m_projectileSizeCurve = AnimationCurve.Linear (0, 0, 1, 1);

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
            
            ScaleProjectile ();
        }
        else if (_isCasting)
        {
            _isCasting = false;
            _timeSinceCastStart = 0;

            LaunchProjectile (_projectileInstance);
        }
    }

    private void ScaleProjectile ()
    {
        var t = NormalizedCastProgress ();
        var scale = m_projectileSizeCurve.Evaluate (t);
        
        _projectileInstance.transform.localScale = new Vector3 (scale, scale, scale);
    }

    private float NormalizedCastProgress ()
    {
        return m_timeToFullProjectileSize > 0 ? _timeSinceCastStart / m_timeToFullProjectileSize : 1f;
    }
    
    private void CreateNewProjectile ()
    {
        _projectileInstance = Object.Instantiate (m_projectile, _projectileSpawn);
        _projectileInstance.DisableProjectile ();
    }
}