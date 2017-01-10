using UnityEngine;
using System.Collections;
using MassiveNet;

public class RequestHandler : MonoBehaviour {

    private NetView view;
    private Character character;

    void Awake(){
        view = GetComponent<NetView>();
        character = GetComponent<Character>();
    }

    [NetRPC]
    public void SkillAddRequest(string skillId){
        character.AddSkill(skillId);
        view.SendReliable("AddSkill", RpcTarget.All, skillId);
    }
}
