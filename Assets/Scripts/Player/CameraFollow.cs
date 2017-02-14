using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {
	
    public Transform weaponParent;
    public Transform orgParent;
    public Vector3 aimPos;
    public Vector3 aimRot;
    public Vector3 orgPos { get; private set; }
    public Vector3 orgRot { get; private set; }

    public void Initialize(Transform p, Transform wp){
        orgParent = p;
        weaponParent = wp;
        orgPos = transform.localPosition;
        orgRot = transform.localEulerAngles;

        transform.SetParent(orgParent);
        transform.localPosition = orgPos;
        transform.localEulerAngles = orgRot;
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
