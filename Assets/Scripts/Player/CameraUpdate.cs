using UnityEngine;
using System.Collections;

// Update main camera based on this transform and camera controls
public class CameraUpdate : MonoBehaviour {

    public Transform cameraControlX;
    public Transform cameraControlY;

    void Update(){
        Camera.main.transform.position = transform.position;
        Camera.main.transform.localRotation = Quaternion.Euler(new Vector3(cameraControlX.transform.localEulerAngles.x, cameraControlY.transform.localEulerAngles.y, 0f));
    }

}
