public enum Status {

    burned=0, poisoned=0,                               // Damage over time
    frost=1,                                            // Effect on currentStats
    frozen=2,                                           // Unable to perform any action, Chance to instant death upon critical hit
    paralyzed=3, stunned=3, knockedDown=3, asleep=3,    // Unable to perform any action, Prone to assassination skills
    rooted=4,                                           // Unable to move
    silenced=5,                                         // Unable to cast

    invincible=6,                                       // Unable to receive damage
    airborne=7                                          // Unable to perform any action, Prone to airborne skills

}
