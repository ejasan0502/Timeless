using UnityEngine;
using System.Collections;

// Place this on camera pivot object
public class Camera_ThirdPerson : MonoBehaviour {

    [Header("Positions")]
    public bool onLeft = false;
    public Vector3 camOffsetLeft;
    public Vector3 camOffsetRight;

    [Header("Speeds")]
    public float camSpd = 5f;
    public float zoomSpd = 5f;

    [Header("Settings")]
    public float sensitivity = 5f;
    public float minX = -75f, maxX = 90f;
    public float zoomFov = 30f;
    public float hideMeshThreshold = 1f;

    [Header("Object References")]
    public GameObject target;
    public Transform spine;

    private float rotX = 0f, rotY = 0f;
    private WeaponHandler weaponHandler;
    private bool aiming = false;

    void Start(){
        weaponHandler = this.GetSelf().GetComponent<WeaponHandler>();
    }
    void Update(){
        if ( !GameManager.instance.ignoreControlsInput ){
            rotY = Input.GetAxis("Mouse X") * sensitivity;

            rotX += Input.GetAxis("Mouse Y") * sensitivity;
            rotX = Mathf.Clamp(rotX, minX, maxX);
        
            target.transform.Rotate(Vector3.up * rotY);
            transform.localEulerAngles = Vector3.left * rotX;
        }

        CheckWall();
        CheckMesh();
    }
    void LateUpdate(){
        if ( weaponHandler.currentWeapon != null ){
            Vector3 lookRot = Quaternion.LookRotation(Camera.main.transform.forward*10f).eulerAngles;

            spine.RotateAround(transform.position,-transform.parent.right, -lookRot.x);
            //spine.Rotate(weaponHandler.currentWeapon.spineRotOffset);
        }
    }

    // Check if camera is clipping thru wall and re-position it
    private void CheckWall(){
        if ( aiming ) return;

        RaycastHit hit;

        float dist = onLeft ? camOffsetLeft.z : camOffsetRight.z;
        Vector3 dir = Camera.main.transform.position - transform.position;

        if ( Physics.SphereCast(transform.position, Camera.main.nearClipPlane, dir, out hit, dist, 1 << LayerMask.NameToLayer("Environment")) ){
            Camera.main.transform.position = transform.position + (dir.normalized * hit.distance);
        } else {
            Camera.main.transform.localPosition = onLeft ? Vector3.Lerp(Camera.main.transform.localPosition, camOffsetLeft, Time.deltaTime*camSpd) :
                                                           Vector3.Lerp(Camera.main.transform.localPosition, camOffsetRight, Time.deltaTime*camSpd);
        }
    }
    // Check if camera is too close to character model
    private void CheckMesh(){
        if ( !target ) return;

        SkinnedMeshRenderer[] meshes = target.GetComponentsInChildren<SkinnedMeshRenderer>(true);

        if ( meshes.Length > 0 ){
            float dist = Vector3.Distance(Camera.main.transform.position, target.transform.position);
            foreach (SkinnedMeshRenderer mesh in meshes){
                if ( dist <= hideMeshThreshold ){
                    mesh.enabled = false;
                } else {
                    mesh.enabled = true;
                }
            }
        }
    }

    // Aim with given firearm
    public void Aim(Firearm weapon, bool isAiming){
        aiming = isAiming;

        if ( aiming ){
            Camera.main.fieldOfView = Settings.instance.default_fov/2.00f;
            Camera.main.transform.SetParent(weapon.transform);
            Camera.main.transform.localPosition = weapon.aimPos;
            Camera.main.transform.localEulerAngles = weapon.aimRot;
        } else {
            Camera.main.fieldOfView = Settings.instance.default_fov;
            Camera.main.transform.SetParent(transform);
            Camera.main.transform.localPosition = onLeft ? camOffsetLeft : camOffsetRight;
            Camera.main.transform.localEulerAngles = Vector3.zero;
        }
    }
}
