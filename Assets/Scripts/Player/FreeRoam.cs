using UnityEngine;
using System.Collections;

// Free roam camera and movement
// Attach to main camera
public class FreeRoam : MonoBehaviour {

    public float speed = 5f;
    private float sensitivity = 5f;
    private float minX = -75f, maxX = 90f;
    private float rotX = 0f, rotY = 0f;

    void Update(){
        rotY += Input.GetAxis("Mouse X") * sensitivity;
        rotX += Input.GetAxis("Mouse Y") * sensitivity;
        rotX = Mathf.Clamp(rotX, minX, maxX);
        
        transform.localEulerAngles = new Vector3(-rotX, rotY, 0f);

        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        input *= speed * Time.deltaTime;
        transform.position += transform.TransformDirection(input);
    }

}