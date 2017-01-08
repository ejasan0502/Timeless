using UnityEngine;
using System.Collections;

public class FaceCamera : MonoBehaviour {

    public Transform lookAt;

    void Awake(){
        if ( lookAt == null ){
            lookAt = Camera.main.transform;
        }
    }
    void FixedUpdate(){
        if ( lookAt != null ){
            transform.LookAt(lookAt);
        }
    }

}
