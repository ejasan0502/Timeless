using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {
	
    public Transform weaponParent;
    public Transform orgParent;
    public Vector3 aimPos;
    public Vector3 aimRot;
    public Vector3 orgPos { get; private set; }
    public Vector3 orgRot { get; private set; }

    void Awake(){
        orgParent = transform.parent;
        orgPos = transform.localPosition;
        orgRot = transform.localEulerAngles;
    }

    public void Aim(){
        transform.SetParent(weaponParent.GetChild(0));
        transform.localPosition = aimPos;
        transform.localEulerAngles = aimRot;
    }
    public void Reset(){
        transform.SetParent(orgParent);
        transform.localPosition = orgPos;
        transform.localEulerAngles = orgRot;
    }
}
