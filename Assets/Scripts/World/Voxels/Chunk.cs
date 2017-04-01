using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LibNoise.Unity;
using LibNoise.Unity.Generator;

public class Chunk {

    public static Vector3[] BlockVertexTable = new Vector3[8]{
        new Vector3(-1,-1,-1),
        new Vector3( 1,-1,-1),
        new Vector3(-1,-1, 1),
        new Vector3( 1,-1, 1),

        new Vector3(-1, 1,-1),
        new Vector3( 1, 1,-1),
        new Vector3(-1, 1, 1),
        new Vector3( 1, 1, 1),
    };
    public static List<int[]> BlockFaceVertTable = new List<int[]>(){
        new int[4]{ 0, 1, 4, 5 }, // Front
        new int[4]{ 6, 7, 2, 3 }, // Back
        new int[4]{ 2, 0, 6, 4 }, // Left
        new int[4]{ 1, 3, 5, 7 }, // Right
        new int[4]{ 4, 5, 6, 7 }, // Top
        new int[4]{ 2, 3, 0, 1 }, // Bottom
    };
    public static int[] BlockTriTable = new int[6]{
        0, 2, 1,
        1, 2, 3
    };

    public Point worldPos;
    public Vector3 scenePos;
    public Block[,,] blocks { get; private set; }
    public Chunk[] neighbors;
    public MeshData meshData;
    public GameObject gameObject;
    public bool blocksInitialized;

    public Chunk(int width, int height, int length){
        blocks = new Block[width,height,length];
        neighbors = new Chunk[6];
        meshData = new MeshData();

        blocksInitialized = false;
    }

    // Create all blocks into a plane
    public void CreateBlocks(){
        MarchingCubes marchingCubes = new MarchingCubes();
        Point chunkSize = new Point(blocks.GetLength(0), blocks.GetLength(1), blocks.GetLength(2));

        Vector3 startPos = Vector3.zero;
        startPos.x = scenePos.x - chunkSize.x*0.5f + 0.5f;
        startPos.y = scenePos.y - chunkSize.y*0.5f + 0.5f;
        startPos.z = scenePos.z - chunkSize.z*0.5f + 0.5f;

        Vector3 pos = startPos;
        for (int y = 0; y < chunkSize.y; y++){
            for (int z = 0; z < chunkSize.z; z++){
                for (int x = 0; x < chunkSize.x; x++){
                    marchingCubes.MarchCube(pos.x,pos.y,pos.z,1f);

                    pos.x += 1f;
                }
                pos.x = startPos.x;
                pos.z += 1f;
            }
            pos.z = startPos.z;
            pos.y += 1f;
        }

        Mesh m = new Mesh();
        m.name = "Chunk " + worldPos;
        m.vertices = marchingCubes.GetVertices();
        m.triangles = marchingCubes.GetIndices();
        m.RecalculateBounds();
        m.RecalculateNormals();

        GameObject o = new GameObject("Chunk " + worldPos);
        o.AddComponent<MeshFilter>().mesh = m;
        o.AddComponent<MeshRenderer>().material = new Material(Shader.Find("Standard"));
        gameObject = o;

        blocksInitialized = true;
    }
    // Create block with center and position in chunk
    public void CreateBlock(Vector3 scenePos, int x, int y, int z){
        blocks[x,y,z] = new Block();
        blocks[x,y,z].scenePos = scenePos;
        blocks[x,y,z].chunkPos = new Point(x,y,z);
        
        List<Vector3> vertices = new List<Vector3>();
        for (int i = 0; i < BlockVertexTable.Length; i++){
            vertices.Add(scenePos + BlockVertexTable[i]*0.5f);
        }
        blocks[x,y,z].vertices = vertices.ToArray();
    }
    // Create blocks within a radius and a center point
    public void CreateBlocks(Vector3 center, float radius){
        CreateBlocks();

        foreach (Block b in blocks){
            if ( Vector3.Distance(b.scenePos,center) > radius ){
                b.isEmpty = true;
            }
        }
    }
    // Setup block neighbors
    public void SetupNeighbors(){
        // Loop through all blocks
        int width = blocks.GetLength(0);
        int height = blocks.GetLength(1);
        int length = blocks.GetLength(2);

        for (int y = 0; y < height; y++){
            for (int z = 0; z < length; z++){
                for (int x = 0; x < width; x++){
                    for (int i = 0; i < blocks[x,y,z].neighbors.Length; i++){
                        if ( blocks[x,y,z].neighbors[i] != null ) continue;

                        // Make sure to check inside the chunk and neighboring chunks for neighboring blocks
                        switch (i){
                            case (int)Face.front:
                            if ( z-1 >= 0 ){
                                blocks[x,y,z].neighbors[i] = blocks[x,y,z-1];
                            } else if ( neighbors[i] != null ){
                                blocks[x,y,z].neighbors[i] = neighbors[i].blocks[x,y,length-1];
                            }
                            break;
                            case (int)Face.back:
                            if ( z+1 < length ){
                                blocks[x,y,z].neighbors[i] = blocks[x,y,z+1];
                            } else if ( neighbors[i] != null ){
                                blocks[x,y,z].neighbors[i] = neighbors[i].blocks[x,y,0];
                            }
                            break;
                            case (int)Face.left:
                            if ( x-1 >= 0 ){
                                blocks[x,y,z].neighbors[i] = blocks[x-1,y,z];
                            } else if ( neighbors[i] != null ){
                                blocks[x,y,z].neighbors[i] = neighbors[i].blocks[width-1,y,z];
                            }
                            break;
                            case (int)Face.right:
                            if ( x+1 < width ){
                                blocks[x,y,z].neighbors[i] = blocks[x+1,y,z];
                            } else if ( neighbors[i] != null ){
                                blocks[x,y,z].neighbors[i] = neighbors[i].blocks[0,y,z];
                            }
                            break;
                            case (int)Face.top:
                            if ( y+1 < height ){
                                blocks[x,y,z].neighbors[i] = blocks[x,y+1,z];
                            } else if ( neighbors[i] != null ){
                                blocks[x,y,z].neighbors[i] = neighbors[i].blocks[x,0,z];
                            }
                            break;
                            case (int)Face.bottom:
                            if ( y-1 >= 0 ){
                                blocks[x,y,z].neighbors[i] = blocks[x,y-1,z];
                            } else if ( neighbors[i] != null ){
                                blocks[x,y,z].neighbors[i] = neighbors[i].blocks[x,height-1,z];
                            }
                            break;
                        }
                    }
                }
            }
        }
    }
    // Apply noise to chunk's blocks
    public void ApplyNoise(Noise2D noise2d, float height){
        Point chunkSize = new Point(blocks.GetLength(0), blocks.GetLength(1), blocks.GetLength(2));

        foreach (Block b in blocks){
            int x = chunkSize.x*worldPos.x + b.chunkPos.x;
            int z = chunkSize.z*worldPos.z + b.chunkPos.z;

            float noise = noise2d.m_data[x,z]*height;
            if ( b.scenePos.y > noise ){
                b.isEmpty = true;
            }
        }
    }
    // Apply spherical noise to chunk
    public void ApplyNoise(Perlin perlin, float scale, Vector3 center, float radius){
        foreach (Block b in blocks){
            Vector3 pos = Vector3.zero;
            pos.x = center.x + b.scenePos.x/radius;
            pos.y = center.y + b.scenePos.y/radius;
            pos.z = center.z + b.scenePos.z/radius;

            float noise = (float)perlin.GetValue(pos) * radius * scale;
            if ( Vector3.Distance(b.scenePos,center) > noise+radius ){
                b.isEmpty = true;
            }
        }
    }
    // Update meshData
    public void UpdateMesh(){
        // Make sure to clear previous mesh data
        meshData = new MeshData();

        // Loop through all blocks and setup each face based on neighbors
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        foreach (Block b in blocks){
            // SKip this block if its empty
            if ( b.isEmpty ) continue;

            for (int i = 0; i < b.neighbors.Length; i++){
                int v = vertices.Count;
                if ( b.neighbors[i] == null || b.neighbors[i].isEmpty ){
                    // Grab vertices from BlockFaceVertTable by looping through the list of int provided
                    for (int j = 0; j < BlockFaceVertTable[i].Length; j++){
                        int index = BlockFaceVertTable[i][j];
                        vertices.Add( b.vertices[index] );
                    }

                    // Create triangles of given vertices
                    for (int k = 0; k < BlockTriTable.Length; k++){
                        triangles.Add(v + BlockTriTable[k]);
                    }
                }
            }
        }

        // Set mesh data
        meshData.vertices = vertices;
        meshData.triangles = triangles;

        if ( meshData.vertices.Count > Settings.instance.maximum_vertices ){
            Debug.LogError("Mesh contains too many vertices!");
            return;
        }

        // Check if we should create a new gameObject or update the existing one
        if ( gameObject == null ){
            // Create Mesh
            Mesh m = meshData.CreateMesh("Chunk " + worldPos);

            // Create chunk object
            GameObject o = new GameObject("Chunk " + worldPos);
            o.AddComponent<MeshFilter>().mesh = m;
            o.AddComponent<MeshRenderer>().material = new Material(Shader.Find("Standard"));
            o.AddComponent<MeshCollider>().sharedMesh = m;

            // Make sure to save gameObject to chunk
            gameObject = o;
        } else {
            Mesh m = meshData.CreateMesh("Chunk " + worldPos);

            gameObject.GetComponent<MeshFilter>().mesh = m;
            gameObject.GetComponent<MeshCollider>().sharedMesh = m;
        }
    }

}
