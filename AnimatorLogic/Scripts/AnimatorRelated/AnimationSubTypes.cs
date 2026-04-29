using UnityEngine;

namespace Spacats.CharacterController
{
    public enum AnimationSubTypes
    {
        Idle_Default = 0,

        Walk_Default = 100,

        Run_Default = 200,
        Run_Blend = 202,

        Gather_Default = 300,
        Gather_TreeCut = 301,
        Gather_Mine = 302,
        Gather_PickUp = 303,

        Interaction_Default = 400,
        Interaction_Chest = 401,

        AttackMelee_Default = 500,
        AttackMelee_1 = 501,
        AttackMelee_2 = 502,

        AttackRanged_Default = 600,
        AttackRanged_Pistol = 601,
        AttackRanged_AssaultRifle = 602,

        GetHit_Default = 700,
        GetHit_Left = 702,
        GetHit_Right = 703,

        Death_Default = 800,

        Other_Default = 900,
        Other_Stunned = 901,
        Other_Sitting = 902,
        Other_Dancing_0 = 903,
        Other_Dancing_1 = 904,
    }
}
