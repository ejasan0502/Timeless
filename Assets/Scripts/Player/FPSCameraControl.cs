using UnityEngine;
using System.Collections;

public class FPSCameraControl : MonoBehaviour {
	
    public Transform follow;
    public float sensitivity = 5f;
    public float minimumY = -60F;
    public float maximumY = 60F;

    private float rotationY = 0F;
    private float rotationX = 0F;
    private Vector3 offset;

    void Awake(){
        offset = follow.transform.position - transform.position;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    void Update(){
        transform.position = follow.transform.position + offset;
        if ( !Cursor.visible ){
            rotationX += Input.GetAxis("Mouse X") * sensitivity;
         
            rotationY += Input.GetAxis("Mouse Y") * sensitivity;
            rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
        }

        transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);

        if ( Input.GetKeyDown(KeyCode.LeftAlt) || Input.GetKeyDown(KeyCode.Escape) ){
            Cursor.visible = !Cursor.visible;
            if ( !Cursor.visible ) Cursor.lockState = CursorLockMode.None;
        }
    }
}