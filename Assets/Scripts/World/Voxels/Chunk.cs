using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
    public Block[,,] blocks { get; private set; }
    public Chunk[] neighbors;
    public MeshData meshData;
    public GameObject gameObject;

    public Chunk(int width, int height, int length){
        blocks = new Block[width,height,length];
        neighbors = new Chunk[6];
        meshData = new MeshData();
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
            
        }
    }

}
