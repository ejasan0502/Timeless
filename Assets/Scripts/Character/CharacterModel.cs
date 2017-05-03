using UnityEngine;
using System.Collections;

// Save transforms to use for equipment
public class CharacterModel : MonoBehaviour {

    public Transform leftHand;
    public Transform rightHand;
    public Transform spine1;

    public Transform leftHolster;
    public Transform rightHolster;
    public Transform backHolster;

    public Vector3 originalPos { get; private set; }
    public Vector3 originalRot { get; private set; }

    void Awake(){
        foreach (Transform t in GetComponentsInChildren<Transform>()){
            if ( !rightHand && t.name.Contains("RightHand") ){
                rightHand = t;
            } else if ( !leftHand && t.name.Contains("LeftHand") ){
                leftHand = t;
            } else if ( !leftHolster && t.name.Contains("LeftHolster") ){
                leftHolster = t;
            } else if ( !rightHolster && t.name.Contains("RightHolster") ){
                rightHolster = t;
            } else if ( !backHolster && t.name.Contains("BackHolster") ){
                backHolster = t;
            } else if ( !spine1 && t.name.Contains("Spine") ){
                spine1 = t;
            }
        }
    }
    void Start(){
        originalPos = transform.localPosition;
        originalRot = transform.localEulerAngles;
    }
}
