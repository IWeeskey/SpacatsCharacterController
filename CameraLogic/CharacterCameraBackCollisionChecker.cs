using System;
using System.Collections.Generic;
using UnityEngine;

namespace Spacats.CharacterCamera
{
    [Serializable]
    public class CharacterCameraBackCollisionChecker
    {
        public LayerMask CollisionLayers;
        public float Distance = 0;
        public RaycastHit RHit = new RaycastHit();
        public Ray Ray = new Ray();
        
        private List<Vector3> _vectors = new List<Vector3>();
        
        public float GetHitDistance(Vector3 backwardVector, Vector3 rightVector, Vector3 upVector, Vector3 startPosition)
        {
            Ray.origin = startPosition;
            Distance = 999f;

            if (_vectors.Count < 5)
            {
                _vectors.Add(Vector3.zero);
                _vectors.Add(Vector3.zero);
                _vectors.Add(Vector3.zero);
                _vectors.Add(Vector3.zero);
                _vectors.Add(Vector3.zero);
            }

            float gap = 0.15f;
            float multiplier = 1000f;
            _vectors[0] = backwardVector*multiplier;//backward
            _vectors[1] = Vector3.Lerp(backwardVector, rightVector, gap)*multiplier;//right
            _vectors[2] = Vector3.Lerp(backwardVector, rightVector*-1, gap)*multiplier;//left
            _vectors[3] = Vector3.Lerp(backwardVector, upVector, gap)*multiplier;//up
            _vectors[4] = Vector3.Lerp(backwardVector, upVector*-1, gap)*multiplier;//down

            for (int i = 0; i < 5; i++)
            {
                Vector3 endPoint = _vectors[i];
                //Ray.direction = direction;
                
                if (Physics.Linecast(Ray.origin, endPoint , out RHit, CollisionLayers))
                {
                    // Calculate the distance from the original estimated position to the collision location, 
                    // subtracting out a safety "offset" distance from the object we hit.  The offset will help 
                    // keep the camera from being right on top of the surface we hit, which usually shows up as 
                    // the surface geometry getting partially clipped by the camera's front clipping plane. 
                    float localDist = Vector3.Distance(Ray.origin, RHit.point);
                    if (localDist < Distance) Distance = localDist;
                    Debug.DrawLine(Ray.origin, RHit.point, Color.crimson);
                   
                }
                else
                {
                    Debug.DrawLine(Ray.origin, endPoint, Color.cornflowerBlue);
                }

                
                //  Physics.Raycast(Ray, out RHit, Mathf.Infinity, CollisionLayers);
               //  if (RHit.collider == null)
               //  {
               //      continue;
               //  }
               //
               //  Vector3 collisionPoint = RHit.point;
               //  float localDist = RHit.distance;
               //  //localDist += backwardVector * 2.5f;
               //  if (RHit.distance < Distance) Distance = RHit.distance;
               // Debug.DrawLine(Ray.origin, RHit.point);
            }

            return Distance-1f;
        }
    }
}
