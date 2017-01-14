public enum TargetType {
    self,
    singleTarget,   // Select a target upon cast, current target is selected by default
    withinRadius,   // Used on targets within range
    frontAoe,       // Used on targets within a circle area in front of the caster
    frontAoeRect,   // Used on targets within a rectangular area in front of the caster
    aoePoint        // Used on targets within a circle area around the mouse cursor
}
