using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {

    void Update(){
        transform.LookAt(transform.parent);
    }

}
