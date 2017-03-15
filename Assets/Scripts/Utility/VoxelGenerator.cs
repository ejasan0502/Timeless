using UnityEngine;
using System.Collections;
using Cubiquity;

// Uses Cubiquity to create voxel terrain
public class VoxelGenerator : MonoBehaviour {

    public Planet testPlanet;

    void Start(){
        CreatePlanet(testPlanet);
    }

    private void CreatePlanet(Planet planet){
		Region volumeBounds = new Region(-planet.radius, -planet.radius, -planet.radius, planet.radius, planet.radius, planet.radius);		
		TerrainVolumeData data = VolumeData.CreateEmptyVolumeData<TerrainVolumeData>(volumeBounds);
			
		TerrainVolumeGenerator.GeneratePlanet(data, planet.radius, planet.radius - planet.crustDepth, planet.radius - planet.mantleDepth, planet.radius - planet.coreDepth);

        TerrainVolume terrainVol = gameObject.AddComponent<TerrainVolume>();
        terrainVol.data = data;

        gameObject.AddComponent<TerrainVolumeRenderer>();
        gameObject.AddComponent<TerrainVolumeCollider>();
    }
}
