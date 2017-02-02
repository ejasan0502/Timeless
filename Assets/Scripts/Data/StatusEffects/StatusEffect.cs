using UnityEngine;
using System.Collections;

public class StatusEffect : MonoBehaviour {

    private float startTime;
    private float duration;

    public Status status { get; private set; }
    public bool IsFinished {
        get {
            return Time.time - startTime >= duration;
        }
    }

    
}
