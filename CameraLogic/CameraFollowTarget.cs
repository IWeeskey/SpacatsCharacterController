using UnityEngine;

namespace Spacats.CharacterCamera
{
    public class CameraFollowTarget : MonoBehaviour
    {
        public float Scale = 1f;
        public Vector3 FixedFollowOffset = Vector3.zero;
        //public Vector3 FixedFollowOffset = Vector3.zero;
        
        
        public Vector3 GetFollowPosition()
        {
            Vector3 startPosition = gameObject.transform.position;
            Vector3 offsetPosition = FixedFollowOffset * Scale;
            
            return startPosition + offsetPosition;
        }
    }
}
