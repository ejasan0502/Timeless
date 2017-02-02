using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Block {
    public Vector3 center;
    public Vector3 posInChunk;
    public float size;
    public List<Vector3> vertices;
    public List<int> triangles;
    public List<Vector2> uvs;
    public Block[] neighbors;

    public Block(Vector3 center, float size, Vector3 pos){
        neighbors = new Block[6];
        triangles = new List<int>();

        this.center = center;
        this.size = size;
        this.posInChunk = pos;

        float halfSize = size/2.00f;
        vertices = new List<Vector3>();
        vertices.Add(new Vector3(-1, 1,-1)*halfSize);
        vertices.Add(new Vector3( 1, 1,-1)*halfSize);
        vertices.Add(new Vector3( 1,-1,-1)*halfSize);
        vertices.Add(new Vector3(-1,-1,-1)*halfSize);
        vertices.Add(new Vector3(-1, 1, 1)*halfSize);
        vertices.Add(new Vector3( 1, 1, 1)*halfSize);
        vertices.Add(new Vector3( 1,-1, 1)*halfSize);
        vertices.Add(new Vector3(-1,-1, 1)*halfSize);

        uvs = new List<Vector2>();
        uvs.Add(new Vector2(0,1));
        uvs.Add(new Vector2(1,1));
        uvs.Add(new Vector2(1,0));
        uvs.Add(new Vector2(0,0));
        uvs.Add(new Vector2(1,1));
        uvs.Add(new Vector2(0,1));
        uvs.Add(new Vector2(1,0));
        uvs.Add(new Vector2(0,0));
    }

    public void Create(Face face){
        switch (face){
        case Face.front:
            triangles.Add(0);
            triangles.Add(1);
            triangles.Add(3);
            triangles.Add(1);
            triangles.Add(2);
            triangles.Add(3); 
            break;
        case Face.top:
            triangles.Add(4);
            triangles.Add(5);
            triangles.Add(0);
            triangles.Add(5);
            triangles.Add(1);
            triangles.Add(0); 
            break;
        case Face.bottom:
            triangles.Add(3);
            triangles.Add(2);
            triangles.Add(7);
            triangles.Add(2);
            triangles.Add(6);
            triangles.Add(7); 
            break;
        case Face.left:
            triangles.Add(4);
            triangles.Add(0);
            triangles.Add(7);
            triangles.Add(0);
            triangles.Add(3);
            triangles.Add(7); 
            break;
        case Face.right:
            triangles.Add(1);
            triangles.Add(5);
            triangles.Add(2);
            triangles.Add(5);
            triangles.Add(6);
            triangles.Add(2); 
            break;
        case Face.back:
            triangles.Add(5);
            triangles.Add(4);
            triangles.Add(6);
            triangles.Add(4);
            triangles.Add(7);
            triangles.Add(6); 
            break;
        }
    }
    public Mesh Mesh {
        get {
            Mesh m = new Mesh();
            m.vertices = vertices.ToArray();
            m.triangles = triangles.ToArray();
            m.uv = uvs.ToArray();

            return m;
        }
    }
}
