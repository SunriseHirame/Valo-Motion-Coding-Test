using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerInput m_playerInput;
    [SerializeField] private PlayerMovement m_playerMovement;
    [SerializeField] private PlayerAbilities m_playerAbilities;
    [SerializeField] private Camera m_playerCamera;

    private void FixedUpdate ()
    {
        var movementDirection = m_playerInput.actions["Movement"].ReadValue<Vector2> ();
        if (movementDirection.sqrMagnitude > 1f) movementDirection.Normalize ();

        var lookDirection = ResolveLookDirection ();

        m_playerMovement.MoveAndRotate (movementDirection, lookDirection);
        m_playerAbilities.OnUpdate (m_playerInput);
    }

    private Vector3 ResolveLookDirection ()
    {
        var mouseScreenPosition = Mouse.current.position.ReadValue ();
        var raycastPlane = new Plane (Vector3.up, m_playerAbilities.ProjectileSpawnPosition);
        var mouseScreenRay = m_playerCamera.ScreenPointToRay (mouseScreenPosition);
        
        if (raycastPlane.Raycast (mouseScreenRay, out var hitDistance))
        {
            var mouseWorldPoint = mouseScreenRay.origin + mouseScreenRay.direction * hitDistance;
            var lookDirection = (mouseWorldPoint - transform.position).normalized;
            
            return lookDirection;
        }
        else
        {
            return transform.position + transform.forward;
        }
    }

    private void Reset ()
    {
        m_playerInput = GetComponent<PlayerInput> ();
    }
}