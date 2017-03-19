using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
// Apply spherical gravity to this object
public class FauxGravity : MonoBehaviour {

    public GravityPull gravityPull;

    private Rigidbody rb;
    private CharacterMovement charMovt;

    void Awake(){
        charMovt = GetComponent<CharacterMovement>();
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }
    void FixedUpdate(){
        if ( gravityPull ){
            gravityPull.Attract(rb, charMovt != null && charMovt.IsUnderWater ? 0.01f : 1f);
        }
    }
}
