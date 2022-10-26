using UnityEngine;

[RequireComponent (typeof (Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float m_movementSpeed = 5f;
    [SerializeField] private float m_acceleration = 35f;
    
    [SerializeField] private Rigidbody m_rigidbody;

    public void MoveAndRotate (Vector3 movementDirection, Vector3 lookDirection)
    {
        RotateCharacter (lookDirection);
        
        var frameVelocity = CalculateMovementSpeed (movementDirection);
        m_rigidbody.velocity = frameVelocity;
    }

    private void RotateCharacter (Vector3 lookDirection)
    {
        if (lookDirection == Vector3.zero) return;
        
        lookDirection.y = 0;
        m_rigidbody.MoveRotation (Quaternion.LookRotation (lookDirection));
    }

    private Vector3 CalculateMovementSpeed (Vector3 movementDirection)
    {
        var currentVelocity = m_rigidbody.velocity;
        var targetVelocity = new Vector3 (movementDirection.x, 0, movementDirection.y) * m_movementSpeed;

        var velocityMaxDelta = Time.deltaTime * m_acceleration;
        var frameVelocity = Vector3.MoveTowards (currentVelocity, targetVelocity, velocityMaxDelta);
        return frameVelocity;
    }
}