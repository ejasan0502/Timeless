using UnityEngine;
using System.Collections;
using LibNoise.Unity.Generator;

[System.Serializable]
// Handle data for planets
public class Planet {

    [Header("-World Settings-")]
    public float gravity;
	public int gridSize;
	public float radius = 1f;
    public float height = 1f;
    public Perlin perlin;
    public RiggedMultifractal rmf;

    public Planet(float gravity, int gridSize, float radius, float height, Perlin perlin, RiggedMultifractal rmf){
        this.gravity = gravity;
        this.gridSize = gridSize;
        this.radius = radius;
        this.height = height;
        this.perlin = perlin;
        this.rmf = rmf;
    }

}
