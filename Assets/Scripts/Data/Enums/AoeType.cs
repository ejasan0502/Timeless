public enum AoeType {
    singleTarget,   // Used on current target or select a target upon cast
    multiTarget,    // Select targets upon cast, current target is selected by default
    withinRadius,   // Used on targets within range
    frontAoe,       // Used on targets within a circle area in front of the caster
    frontAoeRect,   // Used on targets within a rectangular area in front of the caster
    frontAoeCone,   // Used on targets within a cone area in front of the caster
    aoePoint        // Used on targets within a circle area around the mouse cursor
}
