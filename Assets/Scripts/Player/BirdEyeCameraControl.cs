using UnityEngine;
using System.Collections;

// BirdEyeCameraControl.cs
// Allows the player to control the camera in a 3rd person bird's eye view.
// *Place this script on the camera with a perspective view.
public class BirdEyeCameraControl : MonoBehaviour {
    public float followSpeed = 10f;     // Movement of camera to follow an object
    public float sensitivity = 5f;      // Sensitivity of the camera controls
    public Transform objectToFollow;    // *Object to follow

    private Vector3 prevMousePos;       // Remember last mouse position
    private Vector3 offset;             // Offset between the camera and the object
    private bool rotating = false;      // Receiving player input

    void Update(){
        if ( objectToFollow == null ) return;

        // Check for player input, apply rotation to movement
        if ( !rotating )
            transform.position = Vector3.Lerp(transform.position,objectToFollow.position + offset,followSpeed*Time.deltaTime);
        // Always look at the object to follow
        transform.LookAt(objectToFollow,Vector3.up);

        // PC input
        #if UNITY_STANDALONE || UNITY_EDITOR || UNITY_WEBPLAYER
        if ( Input.GetMouseButtonDown(1) ){
            if ( UIManager.instance.InDeadZone(Input.mousePosition) ) return;
            // On mouse begin, save mouse position and player input is true
            prevMousePos = Input.mousePosition;
            rotating = true;
        }
        if ( Input.GetMouseButton(1) ){
            // On mouse hold, have camera rotate around object based on mouse input
            if ( rotating ){
                transform.RotateAround(objectToFollow.position,Vector3.right,-(Input.mousePosition-prevMousePos).normalized.y*sensitivity);
                transform.RotateAround(objectToFollow.position,Vector3.up,(Input.mousePosition-prevMousePos).normalized.x*sensitivity);
            }
        }
        if ( Input.GetMouseButtonUp(1) ){
            // On mouse end, save new offset of camera and set player input to false
            offset = transform.position - objectToFollow.position;
            rotating = false;
        }
        // Mobile input
        #elif UNITY_ANDROID || UNITY_IOS || UNITY_WP8 || UNITY_WINRT
        // Only move camera when we receive three touch inputs
        if ( Input.touchCount == 3 ){
            Touch touch = Input.touches[0];
            if ( touch.phase == TouchPhase.Began ){
                // On touch begin, save mouse position and player input is true
                prevMousePos = Input.mousePosition;
                rotating = true;
            }
            if ( touch.phase == TouchPhase.Moved ){
                // On touch move, have camera rotate around object based on first touch input
                transform.RotateAround(objectToFollow.position,Vector3.right,-(Input.mousePosition-prevMousePos).normalized.y*sensitivity);
                transform.RotateAround(objectToFollow.position,Vector3.up,(Input.mousePosition-prevMousePos).normalized.x*sensitivity);
            }
            if ( touch.phase == TouchPhase.Ended ){
                // On touch end, save new offset of camera and set player input to false
                offset = transform.position - objectToFollow.position;
                rotating = false;
            }
        }
        #endif

    }

    public void SetFollow(Transform t){
        objectToFollow = t;

        // Set initial rotation of the camera
        transform.LookAt(objectToFollow);
        // Save initial offset of camera
        offset = transform.position - objectToFollow.position;
    }
}
