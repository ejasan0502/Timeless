using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VoxelGenerator : MonoBehaviour {

    public Material voxelSheet;
    public float blockSize = 1f;
    public int chunkWidth = 10;
    public int chunkHeight = 10;
    public int chunkLength = 10;
    public int worldWidth = 1;
    public int worldHeight = 1;
    public int worldLength = 1;

    public Chunk[,,] chunks;
    private List<GameObject> voxels;

    void Awake(){
        chunks = new Chunk[worldWidth,worldHeight,worldLength];
        voxels = new List<GameObject>();
    }
    void Start(){
        CreateWorld();
        SetupNeighbors();
        //DebugNeighbors();
        CreateSingleMesh();
        //CreateMesh();
    }
    void Update(){
        if ( Input.GetMouseButtonDown(0) ){
            RaycastHit hit;
            if ( Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit) ){
                string[] args = hit.collider.name.Split(',');
                Debug.Log(string.Format("Destroying block in chunk ({0},{1},{2})",args[0],args[1],args[2]));
                chunks[int.Parse(args[0]),int.Parse(args[1]),int.Parse(args[2])].DestroyBlockAt(hit.point);
                UpdateMesh();
            }
        }
        if ( Input.GetMouseButtonDown(1) ){
            RaycastHit hit;
            if ( Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit) ){
                string[] args = hit.collider.name.Split(',');
                Debug.Log(string.Format("Creating block in chunk ({0},{1},{2})",args[0],args[1],args[2]));
                chunks[int.Parse(args[0]),int.Parse(args[1]),int.Parse(args[2])].CreateBlockAt(hit.point);
                UpdateMesh();
            }
        }
    }

    private void CreateWorld(){
        float startX = -worldWidth*chunkWidth*blockSize*0.5f + chunkWidth*blockSize*0.5f;
        float startY = -worldHeight*chunkHeight*blockSize*0.5f + chunkHeight*blockSize*0.5f;
        float startZ = -worldLength*chunkLength*blockSize*0.5f + chunkLength*blockSize*0.5f;

        float X = startX;
        float Y = startY;
        float Z = startZ;

        for (int x = 0; x < worldWidth; x++){
            X = startX + chunkWidth*blockSize*x;
            for (int y = 0; y < worldHeight; y++){
                Y = startY + chunkHeight*blockSize*y;
                for (int z = 0; z < worldLength; z++){
                    Z = startZ + chunkLength*blockSize*z;
                    chunks[x,y,z] = new Chunk(new Vector3(X,Y,Z),new Vector3(x,y,z),chunkWidth,chunkHeight,chunkLength,blockSize);
                }
            }
        }
    }
    public void SetupNeighbors(){
        int cW = chunks.GetLength(0);
        int cH = chunks.GetLength(1);
        int cL = chunks.GetLength(2);

        // Setup chunk neighbors
        for (int x = 0; x < cW; x++){
            for (int y = 0; y < cH; y++){
                for (int z = 0; z < cL; z++){
                    for (int i = 0; i < 6; i++){
                        switch (i){
                        case (int)Face.front:
                            if ( z-1 >= 0 ){
                                chunks[x,y,z].neighbors[i] = chunks[x,y,z-1];
                            }
                            break;
                        case (int)Face.top: 
                            if ( y+1 < cH ){
                                chunks[x,y,z].neighbors[i] = chunks[x,y+1,z];
                            }
                            break;
                        case (int)Face.left: 
                            if ( x-1 >= 0 ){
                                chunks[x,y,z].neighbors[i] = chunks[x-1,y,z];
                            }
                            break;
                        case (int)Face.right: 
                            if ( x+1 < cW ){
                                chunks[x,y,z].neighbors[i] = chunks[x+1,y,z];
                            }
                            break;
                        case (int)Face.bottom: 
                            if ( y-1 >= 0 ){
                                chunks[x,y,z].neighbors[i] = chunks[x,y-1,z];
                            }
                            break;
                        case (int)Face.back: 
                            if ( z+1 < cL ){
                                chunks[x,y,z].neighbors[i] = chunks[x,y,z+1];
                            }
                            break;
                        }
                    }
                }
            }
        }

        // Setup block neighbors
        for (int cx = 0; cx < cW; cx++){
            for (int cy = 0; cy < cH; cy++){
                for (int cz = 0; cz < cL; cz++){
                    int bw = chunks[cx,cy,cz].blocks.GetLength(0);
                    int bh = chunks[cx,cy,cz].blocks.GetLength(1);
                    int bl = chunks[cx,cy,cz].blocks.GetLength(2);

                    for (int bx = 0; bx < bw; bx++){
                        for (int by = 0; by < bh; by++){
                            for (int bz = 0; bz < bl; bz++){
                                for (int i = 0; i < 6; i++){
                                    switch (i){
                                    case (int)Face.front: 
                                        if ( bz-1 >= 0 ){
                                            chunks[cx,cy,cz].blocks[bx,by,bz].neighbors[i] = chunks[cx,cy,cz].blocks[bx,by,bz-1];
                                        } else if ( cz-1 >= 0 ){
                                            chunks[cx,cy,cz].blocks[bx,by,bz].neighbors[i] = chunks[cx,cy,cz-1].blocks[bx,by,bl-1];
                                        } else {
                                            chunks[cx,cy,cz].blocks[bx,by,bz].Create(Face.front);
                                        }
                                        break;
                                    case (int)Face.top: 
                                        if ( by+1 < bh ){
                                            chunks[cx,cy,cz].blocks[bx,by,bz].neighbors[i] = chunks[cx,cy,cz].blocks[bx,by+1,bz];
                                        } else if ( cy+1 < cH ){
                                            chunks[cx,cy,cz].blocks[bx,by,bz].neighbors[i] = chunks[cx,cy+1,cz].blocks[bx,0,bz];
                                        } else {
                                            chunks[cx,cy,cz].blocks[bx,by,bz].Create(Face.top);
                                        }
                                        break;
                                    case (int)Face.left: 
                                        if ( bx-1 >= 0 ){
                                            chunks[cx,cy,cz].blocks[bx,by,bz].neighbors[i] = chunks[cx,cy,cz].blocks[bx-1,by,bz];
                                        } else if ( cx-1 >= 0 ){
                                            chunks[cx,cy,cz].blocks[bx,by,bz].neighbors[i] = chunks[cx-1,cy,cz].blocks[bw-1,by,bz];
                                        } else {
                                            chunks[cx,cy,cz].blocks[bx,by,bz].Create(Face.left);
                                        }
                                        break;
                                    case (int)Face.right: 
                                        if ( bx+1 < bw ){
                                            chunks[cx,cy,cz].blocks[bx,by,bz].neighbors[i] = chunks[cx,cy,cz].blocks[bx+1,by,bz];
                                        } else if ( cx+1 < cW ){
                                            chunks[cx,cy,cz].blocks[bx,by,bz].neighbors[i] = chunks[cx+1,cy,cz].blocks[0,by,bz];
                                        } else {
                                            chunks[cx,cy,cz].blocks[bx,by,bz].Create(Face.right);
                                        }
                                        break;
                                    case (int)Face.bottom: 
                                        if ( by-1 >= 0 ){
                                            chunks[cx,cy,cz].blocks[bx,by,bz].neighbors[i] = chunks[cx,cy,cz].blocks[bx,by-1,bz];
                                        } else if ( cy-1 >= 0 ){
                                            chunks[cx,cy,cz].blocks[bx,by,bz].neighbors[i] = chunks[cx,cy-1,cz].blocks[bx,bh-1,bz];
                                        } else {
                                            chunks[cx,cy,cz].blocks[bx,by,bz].Create(Face.bottom);
                                        }
                                        break;
                                    case (int)Face.back: 
                                        if ( bz+1 < bl ){
                                            chunks[cx,cy,cz].blocks[bx,by,bz].neighbors[i] = chunks[cx,cy,cz].blocks[bx,by,bz+1];
                                        } else if ( cz+1 < cL ){
                                            chunks[cx,cy,cz].blocks[bx,by,bz].neighbors[i] = chunks[cx,cy,cz+1].blocks[bx,by,0];
                                        } else {
                                            chunks[cx,cy,cz].blocks[bx,by,bz].Create(Face.back);
                                        }
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }   
    private void DebugNeighbors(){
        int cW = chunks.GetLength(0);
        int cH = chunks.GetLength(1);
        int cL = chunks.GetLength(2);

        for (int i = 0; i < 10; i++){
            int cx, cy, cz;
            cx = Random.Range(0,cW);
            cy = Random.Range(0,cH);
            cz = Random.Range(0,cL);

            int bx, by, bz;
            bx = Random.Range(0,chunks[cx,cy,cz].blocks.GetLength(0));
            by = Random.Range(0,chunks[cx,cy,cz].blocks.GetLength(1));
            bz = Random.Range(0,chunks[cx,cy,cz].blocks.GetLength(2));

            string s = string.Format("C: {0},{1},{2} B: {3},{4},{5}", cx, cy, cz, bx, by, bz);
            s += "\n Neighbors:";
            foreach (Block b in chunks[cx,cy,cz].blocks[bx,by,bz].neighbors){
                if ( b != null ){
                    s += "\n" + string.Format("{0},{1},{2}", b.posInChunk.x, b.posInChunk.y, b.posInChunk.z);
                }
            }

            Debug.Log(s);
        }
    }
    private void CreateMesh(){
        int cW = chunks.GetLength(0);
        int cH = chunks.GetLength(1);
        int cL = chunks.GetLength(2);

        GameObject root = new GameObject("Voxels");
        root.AddComponent<MeshFilter>();
        root.AddComponent<MeshRenderer>();
        for (int cx = 0; cx < cW; cx++){
            for (int cy = 0; cy < cH; cy++){
                for (int cz = 0; cz < cL; cz++){
                    int bw = chunks[cx,cy,cz].blocks.GetLength(0);
                    int bh = chunks[cx,cy,cz].blocks.GetLength(1);
                    int bl = chunks[cx,cy,cz].blocks.GetLength(2);

                    for (int bx = 0; bx < bw; bx++){
                        for (int by = 0; by < bh; by++){
                            for (int bz = 0; bz < bl; bz++){
                                GameObject o = new GameObject();
                                o.transform.position = chunks[cx,cy,cz].center + chunks[cx,cy,cz].blocks[bx,by,bz].center;

                                MeshFilter mf = o.AddComponent<MeshFilter>();
                                MeshRenderer mr = o.AddComponent<MeshRenderer>();

                                mf.sharedMesh = chunks[cx,cy,cz].blocks[bx,by,bz].Mesh;

                                o.transform.SetParent(root.transform);
                            }
                        }
                    }
                }
            }
        }

        root.AddComponent<CombineMeshes>();
    }
    private void CreateSingleMesh(){
        int cW = chunks.GetLength(0);
        int cH = chunks.GetLength(1);
        int cL = chunks.GetLength(2);

        for (int cx = 0; cx < cW; cx++){
            for (int cy = 0; cy < cH; cy++){
                for (int cz = 0; cz < cL; cz++){
                    if ( chunks[cx,cy,cz] == null ) continue;
                    List<Vector3> vertices = new List<Vector3>();
                    List<Vector2> uvs = new List<Vector2>();
                    List<int> triangles = new List<int>();

                    int bw = chunks[cx,cy,cz].blocks.GetLength(0);
                    int bh = chunks[cx,cy,cz].blocks.GetLength(1);
                    int bl = chunks[cx,cy,cz].blocks.GetLength(2);

                    for (int bx = 0; bx < bw; bx++){
                        for (int by = 0; by < bh; by++){
                            for (int bz = 0; bz < bl; bz++){
                                if ( chunks[cx,cy,cz].blocks[bx,by,bz] == null ) continue;

                                foreach (int i in chunks[cx,cy,cz].blocks[bx,by,bz].triangles){
                                    triangles.Add(i+vertices.Count);
                                }
                                foreach (Vector3 v in chunks[cx,cy,cz].blocks[bx,by,bz].vertices){
                                    vertices.Add(v);
                                }

                                uvs.AddRange(chunks[cx,cy,cz].blocks[bx,by,bz].uvs);
                            }
                        }
                    }
                    
                    Mesh m = new Mesh();
                    m.vertices = vertices.ToArray();
                    m.triangles = triangles.ToArray();
                    m.uv = uvs.ToArray();
        
                    m.Optimize();
                    m.RecalculateNormals();

                    GameObject root = new GameObject("Voxels");
                    root.name = string.Format("{0},{1},{2}",cx,cy,cz);
                    //root.transform.position = chunks[cx,cy,cz].center;

                    MeshFilter mf = root.AddComponent<MeshFilter>();
                    MeshRenderer mr = root.AddComponent<MeshRenderer>();
                    MeshCollider mc = root.AddComponent<MeshCollider>();

                    mf.sharedMesh = m;
                    mr.material = voxelSheet;
                    mc.sharedMesh = m;

                    voxels.Add(root);
                }
            }
        }
    }
    private void UpdateMesh(){
        if ( voxels.Count > 0 ){
            for (int i = voxels.Count-1; i >= 0; i--){
                Destroy(voxels[i]);
            }
        }
        voxels = new List<GameObject>();
        CreateSingleMesh();
    } 

    public void UpdateBlock(Chunk chunk, Block block){
        int cx = (int)chunk.posInWorld.x;
        int cy = (int)chunk.posInWorld.y;
        int cz = (int)chunk.posInWorld.z;
        int cW = (int)chunks.GetLength(0);
        int cH = (int)chunks.GetLength(1);
        int cL = (int)chunks.GetLength(2);

        int bx = (int)block.posInChunk.x;
        int by = (int)block.posInChunk.y;
        int bz = (int)block.posInChunk.z;
        int bw = (int)chunk.blocks.GetLength(0);
        int bh = (int)chunk.blocks.GetLength(1);
        int bl = (int)chunk.blocks.GetLength(2);

        if ( chunks[cx,cy,cz] == null ){
            Debug.LogError(string.Format("Chunk ({0},{1},{2}) is null",cx,cy,cz));
            return;
        }

        for (int i = 0; i < 6; i++){
            switch (i){
            case (int)Face.front: 
                if ( bz-1 >= 0 ){
                    chunks[cx,cy,cz].blocks[bx,by,bz].neighbors[i] = chunks[cx,cy,cz].blocks[bx,by,bz-1];
                } else if ( cz-1 >= 0 ){
                    chunks[cx,cy,cz].blocks[bx,by,bz].neighbors[i] = chunks[cx,cy,cz-1].blocks[bx,by,bl-1];
                }
                break;
            case (int)Face.top: 
                if ( by+1 < bh ){
                    chunks[cx,cy,cz].blocks[bx,by,bz].neighbors[i] = chunks[cx,cy,cz].blocks[bx,by+1,bz];
                } else if ( cy+1 < cH ){
                    chunks[cx,cy,cz].blocks[bx,by,bz].neighbors[i] = chunks[cx,cy+1,cz].blocks[bx,0,bz];
                }
                break;
            case (int)Face.left: 
                if ( bx-1 >= 0 ){
                    chunks[cx,cy,cz].blocks[bx,by,bz].neighbors[i] = chunks[cx,cy,cz].blocks[bx-1,by,bz];
                } else if ( cx-1 >= 0 ){
                    chunks[cx,cy,cz].blocks[bx,by,bz].neighbors[i] = chunks[cx-1,cy,cz].blocks[bw-1,by,bz];
                }
                break;
            case (int)Face.right: 
                if ( bx+1 < bw ){
                    chunks[cx,cy,cz].blocks[bx,by,bz].neighbors[i] = chunks[cx,cy,cz].blocks[bx+1,by,bz];
                } else if ( cx+1 < cW ){
                    chunks[cx,cy,cz].blocks[bx,by,bz].neighbors[i] = chunks[cx+1,cy,cz].blocks[0,by,bz];
                }
                break;
            case (int)Face.bottom: 
                if ( by-1 >= 0 ){
                    chunks[cx,cy,cz].blocks[bx,by,bz].neighbors[i] = chunks[cx,cy,cz].blocks[bx,by-1,bz];
                } else if ( cy-1 >= 0 ){
                    chunks[cx,cy,cz].blocks[bx,by,bz].neighbors[i] = chunks[cx,cy-1,cz].blocks[bx,bh-1,bz];
                }
                break;
            case (int)Face.back: 
                if ( bz+1 < bl ){
                    chunks[cx,cy,cz].blocks[bx,by,bz].neighbors[i] = chunks[cx,cy,cz].blocks[bx,by,bz+1];
                } else if ( cz+1 < cL ){
                    chunks[cx,cy,cz].blocks[bx,by,bz].neighbors[i] = chunks[cx,cy,cz+1].blocks[bx,by,0];
                }
                break;
            }

            if ( chunks[cx,cy,cz].blocks[bx,by,bz].neighbors[i] == null ){
                chunks[cx,cy,cz].blocks[bx,by,bz].Create((Face)System.Enum.GetValues(typeof(Face)).GetValue(i));
            }
        }
    }
    public void UpdateChunk(int cx, int cy, int cz){
        for (int x = 0; x < chunks.GetLength(0); x++){
            for (int y = 0; y < chunks.GetLength(1); y++){
                for (int z = 0; z < chunks.GetLength(2); z++){
                    for (int i = 0; i < 6; i++){
                        switch (i){
                        case (int)Face.front:
                            if ( z-1 >= 0 ){
                                chunks[x,y,z].neighbors[i] = chunks[x,y,z-1];
                            }
                            break;
                        case (int)Face.top: 
                            if ( y+1 < chunks.GetLength(1) ){
                                chunks[x,y,z].neighbors[i] = chunks[x,y+1,z];
                            }
                            break;
                        case (int)Face.left: 
                            if ( x-1 >= 0 ){
                                chunks[x,y,z].neighbors[i] = chunks[x-1,y,z];
                            }
                            break;
                        case (int)Face.right: 
                            if ( x+1 < chunks.GetLength(0) ){
                                chunks[x,y,z].neighbors[i] = chunks[x+1,y,z];
                            }
                            break;
                        case (int)Face.bottom: 
                            if ( y-1 >= 0 ){
                                chunks[x,y,z].neighbors[i] = chunks[x,y-1,z];
                            }
                            break;
                        case (int)Face.back: 
                            if ( z+1 < chunks.GetLength(2) ){
                                chunks[x,y,z].neighbors[i] = chunks[x,y,z+1];
                            }
                            break;
                        }
                    }
                }
            }
        }
    }
    public Chunk CreateChunk(Chunk chunk, Face face){
        Debug.Log(string.Format("Creating chunk ({0},{1},{2}) in {3}",chunk.posInWorld.x,chunk.posInWorld.y,chunk.posInWorld.z,face.ToString()));
        if ( face == Face.front || face == Face.back ){
            // Expand chunks
            Chunk[,,] newChunks = new Chunk[chunks.GetLength(0),chunks.GetLength(1),chunks.GetLength(2)+1];

            if ( chunk.posInWorld.z-1 < 0 ){
                // Shift all chunks backward
                for (int x = 0; x < chunks.GetLength(0); x++){
                    for (int y = 0; y < chunks.GetLength(1); y++){
                        for (int z = 0; z < chunks.GetLength(2); z++){
                            chunks[x,y,z].posInWorld = new Vector3(x,y,z+1);
                            newChunks[x,y,z+1] = chunks[x,y,z];
                        }
                    }
                }
            }

            Vector3 posInWorld = new Vector3(chunk.posInWorld.x,chunk.posInWorld.y,chunk.posInWorld.z+(face == Face.front ? -1 : 1));
            newChunks[(int)posInWorld.x,(int)posInWorld.y,(int)posInWorld.z] = new Chunk();
            newChunks[(int)posInWorld.x,(int)posInWorld.y,(int)posInWorld.z].EmptyChunk( new Vector3(chunk.center.x,chunk.center.y,chunk.center.z-blockSize*chunkLength),
                                                                                         posInWorld,
                                                                                         chunkWidth,
                                                                                         chunkHeight,
                                                                                         chunkLength,
                                                                                         blockSize);

            chunks = newChunks;
            UpdateChunk((int)posInWorld.x,(int)posInWorld.y,(int)posInWorld.z);

            return newChunks[(int)posInWorld.x,(int)posInWorld.y,(int)posInWorld.z];
        }

        Debug.Log("Unable to create chunk.");
        return null;
    }
}
