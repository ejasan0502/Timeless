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

    public float tUnitSize;
    public Vector2 tPos;

    public Block(Vector3 center, float size, Vector3 pos, Vector2 tPos, float tUnitSize){
        neighbors = new Block[6];
        triangles = new List<int>();

        this.center = center;
        this.size = size;
        this.posInChunk = pos;
        this.tPos = tPos;
        this.tUnitSize = tUnitSize;

        vertices = new List<Vector3>();
        uvs = new List<Vector2>();
    }

    public void Create(Face face){
        int startVert = vertices.Count;
        switch (face){
        case Face.front:
            vertices.Add(center+new Vector3(-1, 1,-1)*size/2.00f); // 0
            vertices.Add(center+new Vector3( 1, 1,-1)*size/2.00f); // 1
            vertices.Add(center+new Vector3( 1,-1,-1)*size/2.00f); // 2
            vertices.Add(center+new Vector3(-1,-1,-1)*size/2.00f); // 3
            triangles.Add(startVert+0);
            triangles.Add(startVert+1);
            triangles.Add(startVert+3);
            triangles.Add(startVert+1);
            triangles.Add(startVert+2);
            triangles.Add(startVert+3); 
            uvs.Add(new Vector2(tUnitSize*tPos.x            ,tUnitSize*tPos.y+tUnitSize));
            uvs.Add(new Vector2(tUnitSize*tPos.x+tUnitSize  ,tUnitSize*tPos.y+tUnitSize));
            uvs.Add(new Vector2(tUnitSize*tPos.x+tUnitSize  ,tUnitSize*tPos.y));
            uvs.Add(new Vector2(tUnitSize*tPos.x            ,tUnitSize*tPos.y));
            break;
        case Face.top:
            vertices.Add(center+new Vector3(-1, 1, 1)*size/2.00f); // 4
            vertices.Add(center+new Vector3( 1, 1, 1)*size/2.00f); // 5
            vertices.Add(center+new Vector3( 1, 1,-1)*size/2.00f); // 1
            vertices.Add(center+new Vector3(-1, 1,-1)*size/2.00f); // 0
            triangles.Add(startVert+0);
            triangles.Add(startVert+1);
            triangles.Add(startVert+3);
            triangles.Add(startVert+1);
            triangles.Add(startVert+2);
            triangles.Add(startVert+3); 
            uvs.Add(new Vector2(tUnitSize*tPos.x            ,tUnitSize*tPos.y+tUnitSize));
            uvs.Add(new Vector2(tUnitSize*tPos.x+tUnitSize  ,tUnitSize*tPos.y+tUnitSize));
            uvs.Add(new Vector2(tUnitSize*tPos.x+tUnitSize  ,tUnitSize*tPos.y));
            uvs.Add(new Vector2(tUnitSize*tPos.x            ,tUnitSize*tPos.y));
            break;
        case Face.bottom:
            vertices.Add(center+new Vector3( 1,-1, 1)*size/2.00f); // 6
            vertices.Add(center+new Vector3(-1,-1, 1)*size/2.00f); // 7
            vertices.Add(center+new Vector3(-1,-1,-1)*size/2.00f); // 3
            vertices.Add(center+new Vector3( 1,-1,-1)*size/2.00f); // 2
            triangles.Add(startVert+0);
            triangles.Add(startVert+1);
            triangles.Add(startVert+3);
            triangles.Add(startVert+1);
            triangles.Add(startVert+2);
            triangles.Add(startVert+3); 
            uvs.Add(new Vector2(tUnitSize*tPos.x            ,tUnitSize*tPos.y+tUnitSize));
            uvs.Add(new Vector2(tUnitSize*tPos.x+tUnitSize  ,tUnitSize*tPos.y+tUnitSize));
            uvs.Add(new Vector2(tUnitSize*tPos.x+tUnitSize  ,tUnitSize*tPos.y));
            uvs.Add(new Vector2(tUnitSize*tPos.x            ,tUnitSize*tPos.y));
            break;
        case Face.left:
            vertices.Add(center+new Vector3(-1, 1, 1)*size/2.00f); // 4
            vertices.Add(center+new Vector3(-1, 1,-1)*size/2.00f); // 0
            vertices.Add(center+new Vector3(-1,-1,-1)*size/2.00f); // 3
            vertices.Add(center+new Vector3(-1,-1, 1)*size/2.00f); // 7
            triangles.Add(startVert+0);
            triangles.Add(startVert+1);
            triangles.Add(startVert+3);
            triangles.Add(startVert+1);
            triangles.Add(startVert+2);
            triangles.Add(startVert+3); 
            uvs.Add(new Vector2(tUnitSize*tPos.x            ,tUnitSize*tPos.y+tUnitSize));
            uvs.Add(new Vector2(tUnitSize*tPos.x+tUnitSize  ,tUnitSize*tPos.y+tUnitSize));
            uvs.Add(new Vector2(tUnitSize*tPos.x+tUnitSize  ,tUnitSize*tPos.y));
            uvs.Add(new Vector2(tUnitSize*tPos.x            ,tUnitSize*tPos.y));
            break;
        case Face.right:
            vertices.Add(center+new Vector3( 1, 1,-1)*size/2.00f); // 1
            vertices.Add(center+new Vector3( 1, 1, 1)*size/2.00f); // 5
            vertices.Add(center+new Vector3( 1,-1, 1)*size/2.00f); // 6
            vertices.Add(center+new Vector3( 1,-1,-1)*size/2.00f); // 2
            triangles.Add(startVert+0);
            triangles.Add(startVert+1);
            triangles.Add(startVert+3);
            triangles.Add(startVert+1);
            triangles.Add(startVert+2);
            triangles.Add(startVert+3); 
            uvs.Add(new Vector2(tUnitSize*tPos.x            ,tUnitSize*tPos.y+tUnitSize));
            uvs.Add(new Vector2(tUnitSize*tPos.x+tUnitSize  ,tUnitSize*tPos.y+tUnitSize));
            uvs.Add(new Vector2(tUnitSize*tPos.x+tUnitSize  ,tUnitSize*tPos.y));
            uvs.Add(new Vector2(tUnitSize*tPos.x            ,tUnitSize*tPos.y));
            break;
        case Face.back:
            vertices.Add(center+new Vector3( 1, 1, 1)*size/2.00f); // 5
            vertices.Add(center+new Vector3(-1, 1, 1)*size/2.00f); // 4
            vertices.Add(center+new Vector3(-1,-1, 1)*size/2.00f); // 7
            vertices.Add(center+new Vector3( 1,-1, 1)*size/2.00f); // 6
            triangles.Add(startVert+0);
            triangles.Add(startVert+1);
            triangles.Add(startVert+3);
            triangles.Add(startVert+1);
            triangles.Add(startVert+2);
            triangles.Add(startVert+3); 
            uvs.Add(new Vector2(tUnitSize*tPos.x            ,tUnitSize*tPos.y+tUnitSize));
            uvs.Add(new Vector2(tUnitSize*tPos.x+tUnitSize  ,tUnitSize*tPos.y+tUnitSize));
            uvs.Add(new Vector2(tUnitSize*tPos.x+tUnitSize  ,tUnitSize*tPos.y));
            uvs.Add(new Vector2(tUnitSize*tPos.x            ,tUnitSize*tPos.y));
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
    public bool Contains(Vector3 point){
        Vector3 min = new Vector3(center.x-size/2.00f,center.y-size/2.00f,center.z-size/2.00f);
        Vector3 max = new Vector3(center.x+size/2.00f,center.y+size/2.00f,center.z+size/2.00f);

        if ( point.x >= min.x && point.x <= max.x && 
             point.y >= min.y && point.y <= max.y &&
             point.z >= min.z && point.z <= max.z ){

             return true;
        }

        return false;
    }
    public Face GetFaceFromPoint(Vector3 point){
        Vector3 min = new Vector3(center.x-size/2.00f,center.y-size/2.00f,center.z-size/2.00f);
        Vector3 max = new Vector3(center.x+size/2.00f,center.y+size/2.00f,center.z+size/2.00f);

        if ( point.z == min.z ){
            return Face.front;
        } else if ( point.z == max.z ){
            return Face.back;
        } else if ( point.y == max.y ){
            return Face.top;
        } else if ( point.y == min.y ){
            return Face.bottom;
        } else if ( point.x == min.x ){
            return Face.left;
        }

        return Face.right;
    }
}
