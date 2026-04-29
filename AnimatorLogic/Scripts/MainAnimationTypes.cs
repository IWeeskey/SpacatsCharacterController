using UnityEngine;


namespace Spacats.CharacterController
{
    /// <summary>
    /// Means lower body locomotion or lowerbody+upperbody
    /// </summary>
    public enum MainAnimationTypes
    {
        Idle = 0,
        
        WalkForward = 1,
        WalkBackward = 2,
        
        RunForward = 3,
        RunBackward = 4,
        
        SprintForward = 5,
        SprintBackward = 6,
        
        CrouchForward = 7,
        CrouchBackward = 8,
        
        SwimForward = 9,
        SwimBackward = 10,
        
        ClimbForward = 11,
        ClimbBackward = 12,
        
    }
}
