using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {
	
    public Transform followPos;
    public Transform followRot;

    private Vector3 offset;

    void Awake(){
        offset = transform.position - followPos.transform.position;
    }
    void Update(){
        transform.position = followPos.transform.position + offset;
        transform.localEulerAngles = followRot.transform.localEulerAngles;
    }
}
