using UnityEngine;
using System.Collections;

public interface UI {

    string Id {
        get;
    }
    MonoBehaviour Script {
        get;
    }
    void SetDisplay(bool b);

}
