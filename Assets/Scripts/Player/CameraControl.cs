using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Handles camera movement with user input
public class CameraControl : MonoBehaviour {

    public bool rotXAxis, rotYAxis;

    private float sensitivity = 1f;
    private float minX, maxX;
    private float rotX = 0f, rotY = 0f;

    void Awake(){
        sensitivity = Settings.instance.cam_sensitivity;
        minX = Settings.instance.cam_minRotX;
        maxX = Settings.instance.cam_maxRotX;
    }
    void Update(){
        if ( Cursor.visible ) return;

        if ( rotYAxis ){
            rotY += Input.GetAxis("Mouse X") * sensitivity;
        }
        if ( rotXAxis ){
            rotX += Input.GetAxis("Mouse Y") * sensitivity;
            rotX = Mathf.Clamp(rotX, minX, maxX);
        }
    }
    void LateUpdate(){
        transform.eulerAngles = new Vector3(-rotX, rotY, 0f);
    }

}
