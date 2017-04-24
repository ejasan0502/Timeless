using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Handles camera movement with user input
public class CameraControl : MonoBehaviour {

    public bool rotXAxis, rotYAxis;

    private float sensitivity = 5f;
    private float minX = -75f, maxX = 90f;
    private float rotX = 0f, rotY = 0f;

    void Update(){
        if ( !Cursor.visible ){
            if ( rotYAxis ){
                rotY = Input.GetAxis("Mouse X") * sensitivity;
            }
            if ( rotXAxis ){
                rotX += Input.GetAxis("Mouse Y") * sensitivity;
                rotX = Mathf.Clamp(rotX, minX, maxX);
            }
        }
        
        if ( rotYAxis )
            transform.Rotate(Vector3.up * rotY);
        if ( rotXAxis )
            transform.localEulerAngles = Vector3.left * rotX;
    }

}
