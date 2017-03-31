using UnityEngine;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using LibNoise.Unity;
using LibNoise.Unity.Generator;

public class World : MonoBehaviour {

    [Header("Performance Settings")]
    public float distanceFromCamera = 10;

    [Header("World Settings")]
    [Range(0.01f,1f)] public float scale = 0.5f;
    public Point chunkSize = new Point(1,1,1);
    public Point worldSize = new Point(1,1,1);

    public Chunk[,,] chunks { get; private set; }
    
    private Noise2D noise2d;
    //private Hashtable visibleChunks = new Hashtable();
    //private Dictionary<Vector3,Chunk> visibleChunks = new Dictionary<Vector3,Chunk>();
    private List<Chunk> visibleChunks = new List<Chunk>();

    void Awake(){
        chunks = new Chunk[worldSize.x,worldSize.y,worldSize.z];
    }
    IEnumerator Start(){
        yield return new WaitForSeconds(1f);
        CreateSpherical();
    }
    void OnDisable(){
        // Properly destroy gameObjects in scene so Unity doesn't crash when exiting game
        foreach (Chunk c in chunks){
            if ( c != null && c.gameObject != null ){
                Destroy(c.gameObject);
            }
        }
    }

    // Create the world by looping through chunks, X -> Z -> Y 
    private void CreatePlanar(){
        // Initialize noise
        noise2d = new Noise2D(worldSize.x*chunkSize.x,worldSize.z*chunkSize.z,new Perlin());
        noise2d.GeneratePlanar(-1, 1, -1, 1);

        // Initialize chunk positions
        InitializeChunks();

        // Generate initial chunks
        GenerateChunks();

        Debug.Log("World generated in " + Time.realtimeSinceStartup + " seconds.");
    }
    // Create a spherical world
    private void CreateSpherical(){
        noise2d = new Noise2D(worldSize.x*chunkSize.x,worldSize.z*chunkSize.z,new Perlin());
        noise2d.GenerateSpherical(-1,1,-1,1);

        InitializeChunks();

        foreach (Chunk c in chunks){
            if ( Vector3.Distance(c.scenePos,Camera.main.transform.position) < distanceFromCamera ){
                c.CreateBlocks(Vector3.zero,worldSize.y*chunkSize.y*0.5f);
                //c.ApplyNoise(noise2d, chunkSize.y*scale); 

                //visibleChunks.Add(c.scenePos,c);
                visibleChunks.Add(c) ;
            }
        }

        foreach (Chunk c in visibleChunks){
            c.SetupNeighbors();
        }
        foreach (Chunk c in visibleChunks){
            c.UpdateMesh();
        }
    }

    // Setup neighbors for chunks and blocks to make mesh creation/destruction easier
    private void SetupNeighbors(){
        // Loop through chunks in world
        foreach (Chunk c in chunks){
            if ( c != null ){
                SetupNeighbors(c);
            }
        }
    }
    // Setup neighbors for specified chunk
    private void SetupNeighbors(Chunk chunk){
        int x = chunk.worldPos.x;
        int y = chunk.worldPos.y;
        int z = chunk.worldPos.z;

        // Setup neighbors for each chunk
        for (int i = 0; i < chunk.neighbors.Length; i++){
            if ( chunk.neighbors[i] != null ) continue;
            switch (i){
                case (int)Face.front:
                if ( z-1 >= 0 && chunks[x,y,z-1] != null ){
                    chunk.neighbors[i] = chunks[x,y,z-1];
                }
                break;
                case (int)Face.back:
                if ( z+1 < worldSize.z && chunks[x,y,z+1] != null ){
                    chunk.neighbors[i] = chunks[x,y,z+1];
                }
                break;
                case (int)Face.left:
                if ( x-1 >= 0 && chunks[x-1,y,z] != null ){
                    chunk.neighbors[i] = chunks[x-1,y,z];
                }
                break;
                case (int)Face.right:
                if ( x+1 < worldSize.x && chunks[x+1,y,z] != null ){
                    chunk.neighbors[i] = chunks[x+1,y,z];
                }
                break;
                case (int)Face.top:
                if ( y+1 < worldSize.y && chunks[x,y+1,z] != null ){
                    chunk.neighbors[i] = chunks[x,y+1,z];
                }
                break;
                case (int)Face.bottom:
                if ( y-1 >= 0 && chunks[x,y-1,z] != null ){
                    chunk.neighbors[i] = chunks[x,y-1,z];
                }   
                break;
            }
        }
    }
    // Initialize chunks and setup neighbors
    private void InitializeChunks(){
        // Loop through the 3d array and fill it with chunks
        Vector3 startPos = Vector3.zero;
        startPos.x = -1*chunkSize.x*worldSize.x*0.5f + chunkSize.x*0.5f;
        startPos.y = -1*chunkSize.y*worldSize.y*0.5f + chunkSize.y*0.5f;
        startPos.z = -1*chunkSize.z*worldSize.z*0.5f + chunkSize.z*0.5f;

        // Initialize all chunks but do not set up blocks
        Vector3 pos = startPos;
        for (int y = 0; y < worldSize.y; y++){
            for (int z = 0; z < worldSize.z; z++){
                for (int x = 0; x < worldSize.x; x++){
                    // Check if chunk is around the player
                    chunks[x,y,z] = new Chunk(chunkSize.x,chunkSize.y,chunkSize.z);
                    chunks[x,y,z].worldPos = new Point(x,y,z);
                    chunks[x,y,z].scenePos = pos;

                    pos.x += chunkSize.x;
                }
                pos.x = startPos.x;
                pos.z += chunkSize.z;
            }
            pos.z = startPos.z;
            pos.y += chunkSize.y;
        }

        SetupNeighbors();
    }
    // Generate initial chunks
    private void GenerateChunks(){
        foreach (Chunk c in chunks){
            if ( Vector3.Distance(c.scenePos,Camera.main.transform.position) < distanceFromCamera ){
                c.CreateBlocks(); 
                c.ApplyNoise(noise2d, chunkSize.y*scale); 

                //visibleChunks.Add(c.scenePos,c);
                visibleChunks.Add(c);
            }
        }

        //foreach (DictionaryEntry entry in visibleChunks){
        //    ((Chunk)entry.Value).SetupNeighbors();
        //}
        //foreach (DictionaryEntry entry in visibleChunks){
        //    ((Chunk)entry.Value).UpdateMesh();
        //}

        //foreach (KeyValuePair<Vector3,Chunk> entry in visibleChunks){
        //    entry.Value.SetupNeighbors();
        //}
        //foreach (KeyValuePair<Vector3,Chunk> entry in visibleChunks){
        //    entry.Value.UpdateMesh();
        //}

        foreach (Chunk c in visibleChunks){
            c.SetupNeighbors();
        }
        foreach (Chunk c in visibleChunks){
            c.UpdateMesh();
        }
    }

    // Return chunk at scene position
    private Chunk GetChunkAt(Vector3 position){
        Point worldPos = new Point();

        worldPos.x = Mathf.FloorToInt( (position.x+chunkSize.x*worldSize.x*0.5f)/chunkSize.x );
        worldPos.y = Mathf.FloorToInt( (position.y+chunkSize.y*worldSize.y*0.5f)/chunkSize.y );
        worldPos.z = Mathf.FloorToInt( (position.z+chunkSize.z*worldSize.z*0.5f)/chunkSize.z );

        return chunks[worldPos.x,worldPos.y,worldPos.z];
    }
}
