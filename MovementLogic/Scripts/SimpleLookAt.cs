using UnityEngine;

namespace Spacats.CharacterController
{
    public class SimpleLookAt : MonoBehaviour
    {
        public Transform _lookAtTarget;
        
        void Update()
        {
            if (_lookAtTarget==null) return;
            gameObject.transform.LookAt(_lookAtTarget);
        }
    }
}
