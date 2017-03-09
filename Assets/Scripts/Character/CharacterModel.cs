using UnityEngine;
using System.Collections;

// Save transforms to use for equipment
public class CharacterModel : MonoBehaviour {

    public Transform rightHand;

    public Transform leftHolster;
    public Transform rightHolster;
    public Transform backHolster;

    public Vector3 originalPos { get; private set; }
    public Vector3 originalRot { get; private set; }

    void Start(){
        originalPos = transform.localPosition;
        originalRot = transform.localEulerAngles;
    }
}
