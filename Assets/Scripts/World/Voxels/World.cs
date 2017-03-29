using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LibNoise.Unity;
using LibNoise.Unity.Generator;

public class World : MonoBehaviour {

    [Header("Performance Settings")]
    public float distanceFromCamera = 10;
    public float updateChunksThreshold = 5;
    public float updateChunksFrequency = 10;

    [Header("World Settings")]
    [Range(0.01f,1f)] public float scale = 0.5f;
    public Point chunkSize = new Point(1,1,1);
    public Point worldSize = new Point(1,1,1);

    public Chunk[,,] chunks { get; private set; }
    
    private const int MaximumVertices = 65000;
    private Noise2D noise2d;
    private List<Chunk> visibleChunks = new List<Chunk>();
    private List<Chunk> chunksToRemove = new List<Chunk>();
    private Vector3 prevCamPos;
    private float updateChunkStartTime = 0f;

    void Awake(){
        prevCamPos = Camera.main.transform.position;
        chunks = new Chunk[worldSize.x,worldSize.y,worldSize.z];
    }
    void Start(){
        Create();
    }
    void Update(){
        //if ( Time.time - updateChunkStartTime > updateChunksFrequency ){
        //    if ( Vector3.Distance(Camera.main.transform.position,prevCamPos) > updateChunksThreshold ){
        //        GenerateChunks();
        //        prevCamPos = Camera.main.transform.position;
        //        updateChunkStartTime = Time.time;
        //    }
        //}
    }

    // Create the world by looping through chunks, X -> Z -> Y
    private void Create(){
        // Initialize noise
        noise2d = new Noise2D(worldSize.x*chunkSize.x,worldSize.z*chunkSize.z,new Perlin());
        noise2d.GeneratePlanar(-1, 1, -1, 1);

        // Check if vertices for each chunk is above mesh capacity
        int totalVerts = chunkSize.x+1 * chunkSize.y+1 * chunkSize.z+1;
        if ( totalVerts >= MaximumVertices ){
            Debug.LogError("A chunk cannot have more than " + MaximumVertices + " vertices. Change chunkSize!");
            return;
        }

        // Initialize chunk positions
        InitializeChunks();

        // Create chunks based on camera distance
        GenerateChunks();

        Debug.Log("World generated in " + Time.realtimeSinceStartup + " seconds.");
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
    // Apply noise to terrain
    private void ApplyNoise(){
        foreach (Chunk c in visibleChunks){
            if ( c.noiseInitialized ) continue;
            foreach (Block b in c.blocks){
                int x = chunkSize.x*c.worldPos.x + b.chunkPos.x;
                int z = chunkSize.z*c.worldPos.z + b.chunkPos.z;

                float noise = noise2d.m_data[x,z]*(worldSize.y*chunkSize.y);
                if ( b.scenePos.y > noise*scale ){
                    b.isEmpty = true;
                }
            }
            c.noiseInitialized = true;
        }

        //int width = worldSize.x*chunkSize.x;
        //int height = worldSize.y*chunkSize.y;
        //int length = worldSize.z*chunkSize.z;

        //int xMulti = 0, yMulti = 0, zMulti = 0;
        //int X = 0, Y = 0, Z = 0;
        //for (int y = 0; y < height; y++){
        //    zMulti = 0;
        //    if ( y != 0 && y%chunkSize.y == 0 ){
        //        yMulti++;
        //    }
        //    Y = y - chunkSize.y*yMulti;

        //    for (int z = 0; z < length; z++){
        //        xMulti = 0;
        //        if ( z != 0 && z%chunkSize.z == 0 ){
        //            zMulti++;
        //        }
        //        Z = z - chunkSize.z*zMulti;

        //        for (int x = 0; x < width; x++){
        //            if ( x != 0 && x%chunkSize.x == 0 ){
        //                xMulti++;
        //            }
        //            X = x - chunkSize.x*xMulti;

        //            float noise = noise2d.m_data[x,z]*height;

        //            if ( chunks[xMulti,yMulti,zMulti] != null &&
        //                 chunks[xMulti,yMulti,zMulti].blocks[X,Y,Z].scenePos.y > noise*scale ){
        //                chunks[xMulti,yMulti,zMulti].blocks[X,Y,Z].isEmpty = true;
        //            }
        //        }
        //    }
        //}
    }
    // Initialize chunks
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
    // Generate chunks based on distance from camera
    private void GenerateChunks(){
        // Reset visibleChunks
        if ( visibleChunks.Count > 0 ){
            chunksToRemove = new List<Chunk>(visibleChunks);
            visibleChunks.Clear();
        }

        //foreach (Chunk c in chunks){
        //    if ( Vector3.Distance(Camera.main.transform.position, c.scenePos) < distanceFromCamera ){
        //        if ( !c.blocksInitialized ) c.CreateBlocks();
        //        visibleChunks.Add( c );
        //    }
        //}

        // Make sure first chunk is initialized
        Chunk firstChunk = GetChunkAt(Camera.main.transform.position);
        firstChunk.CreateBlocks();
        SetupNeighbors(firstChunk);
        visibleChunks.Add(firstChunk);

        // Continiously loop through the chunk neighbors until distance requirement is met
        int count = 0;
        List<Chunk> chunksToAdd = new List<Chunk>(firstChunk.neighbors);
        List<Chunk> nextChunksToAdd = new List<Chunk>();
        while ( count < distanceFromCamera ){
            foreach (Chunk c in chunksToAdd){
                if ( c == null || visibleChunks.Contains(c) ) continue;

                if ( !c.blocksInitialized ){ 
                    c.CreateBlocks();
                }
                visibleChunks.Add(c);

                nextChunksToAdd.AddRange( c.neighbors );
            }

            chunksToAdd = new List<Chunk>(nextChunksToAdd);
            nextChunksToAdd.Clear();
            count++;
        }

        // Setup block neighbors
        foreach (Chunk c in visibleChunks){
            c.SetupNeighbors();
        }

        // Apply noise
        ApplyNoise();

        // Compare visibleChunks and chunksToRemove, 
        // Remove any chunks from chunksToRemove that are in visibleChunks
        if ( chunksToRemove.Count > 0 ){
            for (int i = chunksToRemove.Count-1; i >= 0; i--){
                if ( visibleChunks.Contains(chunksToRemove[i]) ){
                    chunksToRemove.RemoveAt(i);
                }
            }

            // Loop through chunksToRemove and disable gameObjects
            foreach (Chunk c in chunksToRemove){
                if ( c != null && c.gameObject != null ){
                    c.gameObject.SetActive(false);
                }
            }
        }

        // Loop through visibleChunks and create meshes
        foreach (Chunk c in visibleChunks){
            if ( c != null ){
                if ( c.gameObject == null )
                    c.UpdateMesh();
                else
                    c.gameObject.SetActive(true);
            }
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
