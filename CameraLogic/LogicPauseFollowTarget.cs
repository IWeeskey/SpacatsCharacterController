using UnityEngine;

namespace Spacats.CharacterCamera
{
    public class LogicPauseFollowTarget : MonoBehaviour
    {
       [SerializeField] private Rigidbody _rigidbody; 
       [SerializeField] private Collider _collider;

       public void EnablePhysics()
       {
           _collider.enabled = true;
           _rigidbody.isKinematic = false;
       }

       public void DisablePhysics()
       {
           _collider.enabled = false;
           if (!_rigidbody.isKinematic) _rigidbody.linearVelocity = Vector3.zero;
           _rigidbody.isKinematic = true;
       }

       public void SetVelocity(Vector3 velocity)
       {
           if (_rigidbody.isKinematic) return;
           _rigidbody.linearVelocity = velocity;
       }
    }
}
