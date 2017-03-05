using UnityEngine;
using System.Collections;

// Update main camera based on this transform and camera controls
public class CameraUpdate : MonoBehaviour {

    public Transform cameraControlX;
    public Transform cameraControlY;

    void Update(){
        Camera.main.transform.position = transform.position;
        Camera.main.transform.rotation = Quaternion.Euler(new Vector3(cameraControlX.transform.eulerAngles.x, cameraControlY.transform.eulerAngles.y, 0f));
    }

}
