using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LibNoise.Unity;
using LibNoise.Unity.Generator;

public class WorldGenerator : MonoBehaviour {

    [Header("World Settings")]
    public int width = 100;
    public int height = 100;
    public int length = 100;
    public float sizePerUnit = 1f;
    public float radius = 100;
    public Material material;

    [Header("-Debugging-")]
    public bool debug = false;
    private List<Vector3> debugVertices = new List<Vector3>();

    void Start(){
        Mesh m = GenerateSpherifiedPlane(width, length, sizePerUnit, radius);

        gameObject.AddComponent<MeshFilter>().mesh = m;
        gameObject.AddComponent<MeshRenderer>().material = material;
        gameObject.AddComponent<MeshCollider>().sharedMesh = m;
    }
    void OnDrawGizmos(){
        if ( debugVertices.Count > 0 ){
            Gizmos.color = Color.black;
            foreach (Vector3 v in debugVertices){
                Gizmos.DrawSphere(v,0.1f);
            }
        }
    }

    private Mesh GeneratePlane(int width, int length, float sizePerUnit){
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();

        float widthSize = width*sizePerUnit;
        float lengthSize = length*sizePerUnit;
        for (float x = -widthSize/2.00f+sizePerUnit/2.00f; x < widthSize/2.00f+sizePerUnit/2.00f; x += sizePerUnit){
            for (float z = -lengthSize/2.00f+sizePerUnit/2.00f; z < length/2.00f+sizePerUnit/2.00f; z += sizePerUnit){
                vertices.Add(new Vector3(x,0f,z));
            }
        }

        for (int i = 0; i < vertices.Count; i++){
            if ( i != 0 && (i+1)%length == 0 ){
                continue;
            } else if ( i >= vertices.Count-length ){
                break;
            }

            triangles.Add(i);
            triangles.Add(i+1);
            triangles.Add(i+length);

            triangles.Add(i+length);
            triangles.Add(i+1);
            triangles.Add(i+length+1);
        }

        if ( debug ) {
            Debug.Log(string.Format("Vertices: {0} Triangles: {1}",vertices.Count,triangles.Count));
            debugVertices = vertices;
        }

        Mesh m = new Mesh();
        m.name = "Plane";
        m.vertices = vertices.ToArray();
        m.triangles = triangles.ToArray();
        m.RecalculateNormals();

        return m;
    }
    private Mesh GenerateTerrainPlane(Vector3 origin, int width, int height, int length, float sizePerUnit){
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();

        float widthSize = width*sizePerUnit;
        float lengthSize = length*sizePerUnit;
        RiggedMultifractal rmf = new RiggedMultifractal();
        Perlin perlin = new Perlin();
        for (float x = -widthSize/2.00f+sizePerUnit/2.00f; x < widthSize/2.00f+sizePerUnit/2.00f; x += sizePerUnit){
            for (float z = -lengthSize/2.00f+sizePerUnit/2.00f; z < length/2.00f+sizePerUnit/2.00f; z += sizePerUnit){
                float noise = (float)rmf.GetValue(origin.x+x/width,0f,origin.z+z/length)*(float)perlin.GetValue(origin.x+x/width,0f,origin.z+z/length);
                vertices.Add(new Vector3(x,noise*height*sizePerUnit,z));
            }
        }

        for (int i = 0; i < vertices.Count; i++){
            if ( i != 0 && (i+1)%length == 0 ){
                continue;
            } else if ( i >= vertices.Count-length ){
                break;
            }

            triangles.Add(i);
            triangles.Add(i+1);
            triangles.Add(i+length);

            triangles.Add(i+length);
            triangles.Add(i+1);
            triangles.Add(i+length+1);
        }

        if ( debug ) {
            Debug.Log(string.Format("Vertices: {0} Triangles: {1}",vertices.Count,triangles.Count));
            debugVertices = vertices;
        }

        Mesh m = new Mesh();
        m.name = "Terrain Plane";
        m.vertices = vertices.ToArray();
        m.triangles = triangles.ToArray();
        m.RecalculateNormals();

        return m;
    }
    private Mesh GenerateSpherifiedPlane(int width, int length, float sizePerUnit, float radius){
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();

        float widthSize = width*sizePerUnit;
        float lengthSize = length*sizePerUnit;
        for (float x = -widthSize/2.00f+sizePerUnit/2.00f; x < widthSize/2.00f+sizePerUnit/2.00f; x += sizePerUnit){
            for (float z = -lengthSize/2.00f+sizePerUnit/2.00f; z < length/2.00f+sizePerUnit/2.00f; z += sizePerUnit){
		        Vector3 v = new Vector3(x, radius, z).normalized;
		        float x2 = v.x * v.x;
		        float y2 = v.y * v.y;
		        float z2 = v.z * v.z;
		        Vector3 s;
		        s.x = v.x * Mathf.Sqrt(1f - y2 / 2f - z2 / 2f + y2 * z2 / 3f);
		        s.y = v.y * Mathf.Sqrt(1f - x2 / 2f - z2 / 2f + x2 * z2 / 3f);
		        s.z = v.z * Mathf.Sqrt(1f - x2 / 2f - y2 / 2f + x2 * y2 / 3f);
                vertices.Add( s*radius );
            }
        }

        for (int i = 0; i < vertices.Count; i++){
            if ( i != 0 && (i+1)%length == 0 ){
                continue;
            } else if ( i >= vertices.Count-length ){
                break;
            }

            triangles.Add(i);
            triangles.Add(i+1);
            triangles.Add(i+length);

            triangles.Add(i+length);
            triangles.Add(i+1);
            triangles.Add(i+length+1);
        }

        if ( debug ) {
            Debug.Log(string.Format("Vertices: {0} Triangles: {1}",vertices.Count,triangles.Count));
            debugVertices = vertices;
        }

        Mesh m = new Mesh();
        m.name = "Spherified Plane";
        m.vertices = vertices.ToArray();
        m.triangles = triangles.ToArray();
        m.RecalculateNormals();

        return m;
    }
}
