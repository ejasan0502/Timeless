using UnityEngine;
using System.Collections;

public class InitTest : MonoBehaviour {
	
    public string baseModel;
    public string id;

    void Awake(){
        GameObject o = (GameObject) Instantiate(Resources.Load(baseModel));
        o.transform.SetParent(transform);
        o.transform.localPosition = new Vector3(0f,-1f,0f);
        //o.AddComponent<PlayerWriteSync>();

        Animator anim = o.GetComponent<Animator>();
        CharacterModel charModel = o.GetComponent<CharacterModel>();
        GetComponent<Equipment>().SetCharModel(charModel);
        GetComponent<Equipment>().SetAnim(anim);
        GetComponent<Character>().SetAnim(anim);
        GetComponent<Character>().id = id;

        gameObject.AddComponent<FPSMovement>().anim = anim;

        Camera.main.transform.position = new Vector3(0f,1.7f,-0.164f);
        Camera.main.gameObject.AddComponent<FPSCameraControl>().Initialize(o.transform);

        GameObject.Find("LimbsCamera").GetComponent<CameraFollow>().Initialize(charModel.nodes[2],charModel.nodes[(int)EquipType.primary]);

        charModel.limbsMesh.layer = LayerMask.NameToLayer("Limbs");
        foreach (GameObject go in charModel.meshes){
            go.layer = LayerMask.NameToLayer("Self");
        }
    }

}
