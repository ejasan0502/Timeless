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
    public GameObject target;

    private float rotX = 0f, rotY = 0f;
    private float orgFov;
    private CharacterModel charModel;
    private WeaponHandler weaponHandler;

    void Awake(){
        orgFov = Camera.main.fieldOfView;
        charModel = transform.parent.GetComponentInChildren<CharacterModel>();
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
        //transform.localEulerAngles = new Vector3(-rotX, transform.localEulerAngles.y, transform.localEulerAngles.z);
        //target.transform.localEulerAngles = new Vector3(target.transform.localEulerAngles.x, rotY, target.transform.localEulerAngles.z);

        CheckWall();
        CheckMesh();
    }
    void LateUpdate(){
        if ( weaponHandler.currentWeapon != null )
            charModel.spine1.RotateAround(transform.position,-transform.parent.right, rotX);
    }

    // Check if camera is clipping thru wall and re-position it
    private void CheckWall(){
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

    // Zoom in/out camera
    public void Zoom(bool isZooming){
        if ( GameManager.instance.ignoreControlsInput ) return;

        float fov = isZooming ? Mathf.Lerp(Camera.main.fieldOfView, zoomFov, Time.deltaTime*zoomSpd) :
                                Mathf.Lerp(Camera.main.fieldOfView, orgFov, Time.deltaTime*zoomSpd);

        Camera.main.fieldOfView = fov;
    }
}
