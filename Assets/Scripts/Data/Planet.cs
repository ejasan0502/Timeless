using UnityEngine;
using System.Collections;

[System.Serializable]
public class Planet {

    public int radius = 60;
    public int crustDepth = 1;
    public int mantleDepth = 10;
    public int coreDepth = 35;

    public Planet(int radius, int crustDepth, int mantleDepth, int coreDepth){
        this.radius = radius;
        this.crustDepth = crustDepth;
        this.mantleDepth = mantleDepth;
        this.coreDepth = coreDepth;
    }

}
