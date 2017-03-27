using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MeshData {

    public List<Vector3> vertices;
    public List<Vector2> uvs;
    public List<int> triangles;

    public MeshData(){
        vertices = new List<Vector3>();
        uvs = new List<Vector2>();
        triangles = new List<int>();
    }

    // Create a mesh out of current vertices, triangles, and uvs
    public Mesh CreateMesh(string name){
        Mesh m = new Mesh();
        m.name = name;

        m.vertices = vertices.ToArray();
        m.triangles = triangles.ToArray();

        m.RecalculateNormals();

        return m;
    }

}
