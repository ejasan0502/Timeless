using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(Camera_ThirdPerson))]
public class Editor_Camera_ThirdPerson : Editor {
    
    private Camera_ThirdPerson cam;

    public override void OnInspectorGUI(){
        base.OnInspectorGUI();

        cam = (Camera_ThirdPerson) target;
        
        if ( GUILayout.Button("Save camOffsetLeft") ){
            cam.camOffsetLeft = Camera.main.transform.localPosition;
        }
        if ( GUILayout.Button("Save camOffsetRight") ){
            cam.camOffsetRight = Camera.main.transform.localPosition;
        }
    }

}
