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
                
                if (Physics.Linecast(Ray.origin, endPoint , out RHit, CollisionLayers))
                {
                    float localDist = Vector3.Distance(Ray.origin, RHit.point);
                    if (localDist < Distance) Distance = localDist;
                    Debug.DrawLine(Ray.origin, RHit.point, Color.crimson);
                }
                else
                {
                    Debug.DrawLine(Ray.origin, endPoint, Color.cornflowerBlue);
                }
            }

            return Distance-1f;
        }
    }
}
