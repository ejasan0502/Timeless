using UnityEngine;
using System.Collections;

public class World : MonoBehaviour {

    public bool debug = false;
    public Point chunkSize = new Point(1,1,1);
    public Point worldSize = new Point(1,1,1);

    public Chunk[,,] chunks { get; private set; }

    void Awake(){
        chunks = new Chunk[worldSize.x,worldSize.y,worldSize.z];
    }
    void Start(){
        Create();
    }
    void OnDrawGizmos(){
        if ( debug ){
            Gizmos.color = Color.black;
            if ( chunks != null && chunks.Length > 0 ){
                foreach (Chunk c in chunks){
                    if ( c != null ){
                        if ( c.blocks != null ){
                            foreach (Block b in c.blocks){
                                if ( b.vertices != null && b.vertices.Length > 0 ){
                                    foreach (Vector3 v in b.vertices){
                                        Gizmos.DrawSphere(v, 0.1f);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    // Create the world by looping through chunks, X -> Z -> Y
    private void Create(){
        // Loop through the 3d array and fill it with chunks
        Vector3 startPos = Vector3.zero;
        startPos.x = -1*chunkSize.x*worldSize.x*0.5f + chunkSize.x*0.5f;
        startPos.y = -1*chunkSize.y*worldSize.y*0.5f + chunkSize.y*0.5f;
        startPos.z = -1*chunkSize.z*worldSize.z*0.5f + chunkSize.z*0.5f;

        Vector3 pos = startPos;
        for (int y = 0; y < worldSize.y; y++){
            for (int z = 0; z < worldSize.z; z++){
                for (int x = 0; x < worldSize.x; x++){
                    CreateChunk(pos,x,y,z);
                    pos.x += chunkSize.x;
                }
                pos.x = startPos.x;
                pos.z += chunkSize.z;
            }
            pos.z = startPos.z;
            pos.y += chunkSize.y;
        }

        // Prep chunks and blocks for mesh creation
        SetupNeighbors();

        // Loop through all chunks and create/update meshes
        foreach (Chunk c in chunks){
            c.UpdateMesh();
        }
    }
    // Create a chunk at position in world
    private void CreateChunk(Vector3 scenePos, int X, int Y, int Z){
        // Initialize chunk
        chunks[X,Y,Z] = new Chunk(chunkSize.x,chunkSize.y,chunkSize.z);
        chunks[X,Y,Z].worldPos = new Point(X,Y,Z);

        // Create all blocks in chunk
        Vector3 startPos = Vector3.zero;
        startPos.x = scenePos.x - chunkSize.x*0.5f + 0.5f;
        startPos.y = scenePos.y - chunkSize.y*0.5f + 0.5f;
        startPos.z = scenePos.z - chunkSize.z*0.5f + 0.5f;

        Vector3 pos = startPos;
        for (int y = 0; y < chunkSize.y; y++){
            for (int z = 0; z < chunkSize.z; z++){
                for (int x = 0; x < chunkSize.x; x++){
                    chunks[X,Y,Z].CreateBlock(pos,x,y,z);
                    pos.x += 1f;
                }
                pos.x = startPos.x;
                pos.z += 1f;
            }
            pos.z = startPos.z;
            pos.y += 1f;
        }
    }
    // Setup neighbors for chunks and blocks to make mesh creation/destruction easier
    private void SetupNeighbors(){
        // Loop through chunks in world
        for (int y = 0; y < worldSize.y; y++){
            for (int z = 0; z < worldSize.z; z++){
                for (int x = 0; x < worldSize.x; x++){
                    // Setup neighbors for each chunk
                    for (int i = 0; i < chunks[x,y,z].neighbors.Length; i++){
                        switch (i){
                            case (int)Face.front:
                            if ( z-1 >= 0 ){
                                chunks[x,y,z].neighbors[i] = chunks[x,y,z-1];
                            }
                            break;
                            case (int)Face.back:
                            if ( z+1 < worldSize.z ){
                                chunks[x,y,z].neighbors[i] = chunks[x,y,z+1];
                            }
                            break;
                            case (int)Face.left:
                            if ( x-1 >= 0 ){
                                chunks[x,y,z].neighbors[i] = chunks[x-1,y,z];
                            }
                            break;
                            case (int)Face.right:
                            if ( x+1 < worldSize.x ){
                                chunks[x,y,z].neighbors[i] = chunks[x+1,y,z];
                            }
                            break;
                            case (int)Face.top:
                            if ( y+1 < worldSize.y ){
                                chunks[x,y,z].neighbors[i] = chunks[x,y+1,z];
                            }
                            break;
                            case (int)Face.bottom:
                            if ( y-1 >= 0 ){
                                chunks[x,y,z].neighbors[i] = chunks[x,y-1,z];
                            }   
                            break;
                        }
                    }

                    // Setup neighbors for each block in chunk
                    chunks[x,y,z].SetupNeighbors();
                }
            }
        }
    }
}
