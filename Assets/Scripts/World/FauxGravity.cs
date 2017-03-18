using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
// Apply spherical gravity to this object
public class FauxGravity : MonoBehaviour {

    public GravityPull gravityPull;

    private Rigidbody rb;

    void Awake(){
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }
    void FixedUpdate(){
        if ( gravityPull ){
            gravityPull.Attract(rb);
        }
    }
}
