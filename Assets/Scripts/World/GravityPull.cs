using UnityEngine;
using System.Collections;

// Pull objects with FauxGravity component to self
public class GravityPull : MonoBehaviour {

    public float gravity;

    void OnTriggerEnter(Collider other){
        if ( other.GetComponent<FauxGravity>() != null ){
            other.GetComponent<FauxGravity>().gravityPull = this;
        }
    }

    // Bring object towards center and/or rotate object according to pull direction
    public void Attract(Rigidbody body, float gravityMultiplier){
        Vector3 targetDir = (body.position - transform.position).normalized;
        Vector3 bodyUp = body.transform.up;

        body.rotation = Quaternion.FromToRotation(bodyUp, targetDir) * body.rotation;
        body.AddForce(targetDir * -gravity*gravityMultiplier);
    }
}
