using UnityEngine;
using System.Collections;

public class Elevate : MonoBehaviour {

    public float speed = 1f;

    void FixedUpdate(){
        transform.position += new Vector3(0f,speed,0f);
    }

}
