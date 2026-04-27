using UnityEngine;

namespace Spacats.Input
{
    public enum MoveDirections
    {
        Idle = 0,
        
        Forward = 1,
        Backward = 2,
        Left = 3,
        Right = 4,
        
        ForwardLeft = 5,
        ForwardRight = 6,
        BackwardLeft = 7,
        BackwardRight = 8,
    }

    public class MoveDirectionsHelper
    {
        public static Vector2[] DirectionVectors = new Vector2[]
        {
            
            new Vector2(0, 0),// Idle = 0,
            
            new Vector2(0, 1),// Forward = 1,
            new Vector2(0, -1),// Backward = 2,
            new Vector2(-1, 0),// Left = 3,
            new Vector2(1, 0),// Right = 4,
            
            new Vector2(-0.7f, 0.7f),// ForwardLeft = 5,
            new Vector2(0.7f, 0.7f),// ForwardRight = 6,
            new Vector2(-0.7f, -0.7f),// BackwardLeft = 7,
            new Vector2(0.7f, -0.7f),// BackwardRight = 8,
        };
    }

    
}
