using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {
	
    public Transform followPos;
    public Transform followRot;

    private Vector3 offset;
    public Vector3 orgOffset { get; private set; }

    void Awake(){
        if ( followPos != null ) offset = transform.position - followPos.transform.position;
    }
    void Update(){
        if ( followPos == null || followRot == null ) return;

        transform.position = followPos.transform.position + offset;
        transform.localEulerAngles = followRot.transform.localEulerAngles;
    }

    public void Initialize(Transform followPos, Transform followRot){
        transform.position = new Vector3(0.05f,2f,0f);

        this.followPos = followPos;
        this.followRot = followRot;

        offset = transform.position - followPos.transform.position;
        orgOffset = offset;
    }
    public void SetOffset(Vector3 offset){
        this.offset = offset;
    }
}
