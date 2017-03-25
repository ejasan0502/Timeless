using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VoxelGenerator : MonoBehaviour {

    public bool debug = false;
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
    
    public enum Face {

        front = 0,
        top = 1,
        left = 2,
        right = 3,
        bottom = 4,
        back = 5

    }
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
    public class Chunk {

        public Vector3 center;
        public Vector3 posInWorld;
        public Block[,,] blocks;
        public Chunk[] neighbors;

        private VoxelGenerator vGen;

        public Chunk(){}
        public Chunk(Vector3 c, Vector3 p, int w, int h, int l, float blockSize){
            center = c;
            posInWorld = p;
            blocks = new Block[w,h,l];
            neighbors = new Chunk[6];
            vGen = GameObject.FindObjectOfType<VoxelGenerator>();

            CreateBlocks(w,h,l,blockSize);
        }

        private void CreateBlocks(int maxX, int maxY, int maxZ, float size){
            float startX = -maxX*size*0.5f + size/2.00f;
            float startY = -maxY*size*0.5f + size/2.00f;
            float startZ = -maxZ*size*0.5f + size/2.00f;

            float X = startX;
            float Y = startY;
            float Z = startZ;

            for (int x = 0; x < maxX; x++){
                X = startX + size*x;
                for (int y = 0; y < maxY; y++){
                    Y = startY + size*y;
                    for (int z = 0; z < maxZ; z++){
                        Z = startZ + size*z;
                        blocks[x,y,z] = new Block(center+new Vector3(X,Y,Z),size,new Vector3(x,y,z),new Vector2(0,0),0.25f);
                    }
                } 
            }
        }

        public void EmptyChunk(Vector3 center, Vector3 posInWorld, int w, int h, int l, float blockSize){
            this.center = center;
            this.posInWorld = posInWorld;
            blocks = new Block[w,h,l];
            neighbors = new Chunk[6];
            vGen = GameObject.FindObjectOfType<VoxelGenerator>();
        }
        public void DestroyBlockAt(Vector3 point){
            for (int x = 0; x < blocks.GetLength(0); x++){
                for (int y = 0; y < blocks.GetLength(1); y++){
                    for (int z = 0; z < blocks.GetLength(2); z++){
                        if ( blocks[x,y,z] == null ) continue;
                        if ( blocks[x,y,z].Contains(point) ){
                            for (int i = 0; i < blocks[x,y,z].neighbors.Length; i++){
                                if ( blocks[x,y,z].neighbors[i] == null ) continue;
                                switch(i){
                                case (int)Face.front:
                                blocks[x,y,z].neighbors[i].Create(Face.back);
                                break;
                                case (int)Face.top:
                                blocks[x,y,z].neighbors[i].Create(Face.bottom);
                                break;
                                case (int)Face.left:
                                blocks[x,y,z].neighbors[i].Create(Face.right);
                                break;
                                case (int)Face.right:
                                blocks[x,y,z].neighbors[i].Create(Face.left);
                                break;
                                case (int)Face.bottom:
                                blocks[x,y,z].neighbors[i].Create(Face.top);
                                break;
                                case (int)Face.back:
                                blocks[x,y,z].neighbors[i].Create(Face.front);
                                break;
                                }
                            }
                            Debug.Log(string.Format("Destroyed block ({0},{1},{2})",x,y,z));
                            blocks[x,y,z] = null;
                            return;
                        }
                    }
                }
            }
        }
        public void CreateBlockAt(Vector3 point){
            for (int x = 0; x < blocks.GetLength(0); x++){
                for (int y = 0; y < blocks.GetLength(1); y++){
                    for (int z = 0; z < blocks.GetLength(2); z++){
                        if ( blocks[x,y,z] == null ) continue;
                        if ( blocks[x,y,z].Contains(point) ){
                            Face face = blocks[x,y,z].GetFaceFromPoint(point);
                            switch(face){
                            #region Front
                            case Face.front:
                            if ( z-1 < 0 ){
                                if ( neighbors[(int)Face.front] != null ){
                                    if ( neighbors[(int)Face.front].blocks[x,y,blocks.GetLength(2)-1] == null ){
                                        // Create block in neighbor
                                        Block b = blocks[x,y,z];
                                        Vector3 c = b.center-new Vector3(0,0,vGen.blockSize);
                                        neighbors[(int)Face.front].blocks[x,y,blocks.GetLength(2)-1] = new Block(c, vGen.blockSize, new Vector3(x,y,blocks.GetLength(2)-1), b.tPos, b.tUnitSize);
                                        vGen.UpdateBlock(neighbors[(int)Face.front],neighbors[(int)Face.front].blocks[x,y,blocks.GetLength(2)-1]);
                                    } else {
                                        // That's weird
                                    }
                                } else {
                                    // Create chunk
                                    Chunk chunk = vGen.CreateChunk(this, Face.front);
                                    if ( chunk != null ){
                                        Debug.Log(string.Format("Created block ({0},{1},{2})",x,y,blocks.GetLength(2)-1));
                                        chunk.blocks[x,y,blocks.GetLength(2)-1] = new Block(blocks[x,y,z].center-new Vector3(0,0,vGen.blockSize),
                                                                                            vGen.blockSize,
                                                                                            new Vector3(x,y,blocks.GetLength(2)-1),
                                                                                            blocks[x,y,z].tPos,
                                                                                            blocks[x,y,z].tUnitSize);

                                        vGen.UpdateBlock(chunk,chunk.blocks[x,y,blocks.GetLength(2)-1]);
                                    } else {
                                        Debug.LogError(string.Format("Chunk ({0},{1},{2}) is null",x,y,blocks.GetLength(2)-1));
                                    }
                                }
                            } else {
                                if ( blocks[x,y,z-1] == null ){
                                    // Create block
                                    blocks[x,y,z-1] = new Block(blocks[x,y,z].center-new Vector3(0,0,vGen.blockSize),
                                                                vGen.blockSize,
                                                                new Vector3(x,y,z-1),
                                                                blocks[x,y,z].tPos,
                                                                blocks[x,y,z].tUnitSize);
                                    Debug.Log(string.Format("Created block ({0},{1},{2})",x,y,z-1));
                                    vGen.UpdateBlock(this,blocks[x,y,z-1]);
                                } else {
                                    // That's weird
                                }
                            }
                            break;
                            #endregion
                            #region Back
                            case Face.back:
                            if ( z+1 >= blocks.GetLength(2) ){
                                // Check for other chunk
                                if ( neighbors[(int)Face.back] != null ){
                                    if ( neighbors[(int)Face.back].blocks[x,y,0] == null ){
                                        // Create block in neighbor
                                        Block b = blocks[x,y,z];
                                        Vector3 c = b.center+new Vector3(0,0,vGen.blockSize);
                                        neighbors[(int)Face.back].blocks[x,y,0] = new Block(c, vGen.blockSize, new Vector3(x,y,0), b.tPos, b.tUnitSize);
                                        vGen.UpdateBlock(neighbors[(int)Face.back],neighbors[(int)Face.back].blocks[x,y,0]);
                                    } else {
                                        // That's weird
                                    }
                                } else {
                                    // Create chunk
                                    Chunk chunk = vGen.CreateChunk(this, Face.back);
                                    if ( chunk != null ){
                                        Debug.Log(string.Format("Created block ({0},{1},{2})",x,y,0));
                                        chunk.blocks[x,y,0] = new Block(blocks[x,y,z].center+new Vector3(0,0,vGen.blockSize),
                                                                        vGen.blockSize,
                                                                        new Vector3(x,y,0),
                                                                        blocks[x,y,z].tPos,
                                                                        blocks[x,y,z].tUnitSize);

                                        vGen.UpdateBlock(chunk,chunk.blocks[x,y,0]);
                                    }
                                }
                            } else {
                                // Check in current chunk
                                if ( blocks[x,y,z+1] == null ){
                                    // Create block
                                    blocks[x,y,z+1] = new Block(blocks[x,y,z].center+new Vector3(0,0,vGen.blockSize),
                                                                vGen.blockSize,
                                                                new Vector3(x,y,z+1),
                                                                blocks[x,y,z].tPos,
                                                                blocks[x,y,z].tUnitSize);
                                    Debug.Log(string.Format("Created block ({0},{1},{2})",x,y,z+1));
                                    vGen.UpdateBlock(this,blocks[x,y,z+1]);
                                } else {
                                    // That's weird
                                }
                            }
                            break;
                            #endregion
                            #region Top
                            case Face.top:
                            if ( y+1 >= blocks.GetLength(1) ){
                                // Check for other chunk
                                if ( neighbors[(int)Face.top] != null ){
                                    if ( neighbors[(int)Face.top].blocks[x,0,z] == null ){
                                        // Create block in neighbor
                                        Block b = blocks[x,y,z];
                                        Vector3 c = b.center+new Vector3(0,vGen.blockSize,0);
                                        neighbors[(int)Face.top].blocks[x,0,z] = new Block(c, vGen.blockSize, new Vector3(x,0,z), b.tPos, b.tUnitSize);
                                        vGen.UpdateBlock(neighbors[(int)Face.top],neighbors[(int)Face.top].blocks[x,0,z]);
                                    } else {
                                        // That's weird
                                    }
                                } else {
                                    // Create chunk
                                    Chunk chunk = vGen.CreateChunk(this, Face.top);
                                    if ( chunk != null ){
                                        Debug.Log(string.Format("Created block ({0},{1},{2})",x,0,z));
                                        chunk.blocks[x,0,z] = new Block(blocks[x,y,z].center+new Vector3(0,vGen.blockSize,0),
                                                                        vGen.blockSize,
                                                                        new Vector3(x,0,z),
                                                                        blocks[x,y,z].tPos,
                                                                        blocks[x,y,z].tUnitSize);

                                        vGen.UpdateBlock(chunk,chunk.blocks[x,0,z]);
                                    }
                                }
                            } else {
                                // Check in current chunk
                                if ( blocks[x,y+1,z] == null ){
                                    // Create block
                                    blocks[x,y+1,z] = new Block(blocks[x,y,z].center+new Vector3(0,vGen.blockSize,0),
                                                                vGen.blockSize,
                                                                new Vector3(x,y+1,z),
                                                                blocks[x,y,z].tPos,
                                                                blocks[x,y,z].tUnitSize);
                                    Debug.Log(string.Format("Created block ({0},{1},{2})",x,y+1,z));
                                    vGen.UpdateBlock(this,blocks[x,y+1,z]);
                                } else {
                                    // That's weird
                                }
                            }
                            break;
                            #endregion
                            #region Bottom
                            case Face.bottom:
                            if ( y-1 < 0 ){
                                if ( neighbors[(int)face] != null ){
                                    if ( neighbors[(int)face].blocks[x,blocks.GetLength(1)-1,z] == null ){
                                        // Create block in neighbor
                                        Block b = blocks[x,y,z];
                                        Vector3 c = b.center-new Vector3(0,vGen.blockSize,0);
                                        neighbors[(int)face].blocks[x,blocks.GetLength(1)-1,z] = new Block(c, vGen.blockSize, new Vector3(x,blocks.GetLength(1)-1,z), b.tPos, b.tUnitSize);
                                        vGen.UpdateBlock(neighbors[(int)face],neighbors[(int)face].blocks[x,blocks.GetLength(1)-1,z]);
                                    } else {
                                        // That's weird
                                    }
                                } else {
                                    // Create chunk
                                    Chunk chunk = vGen.CreateChunk(this, face);
                                    if ( chunk != null ){
                                        Debug.Log(string.Format("Created block ({0},{1},{2})",x,blocks.GetLength(1)-1,z));
                                        chunk.blocks[x,blocks.GetLength(1)-1,z] = new Block(blocks[x,y,z].center-new Vector3(0,vGen.blockSize,0),
                                                                                            vGen.blockSize,
                                                                                            new Vector3(x,blocks.GetLength(1)-1,z),
                                                                                            blocks[x,y,z].tPos,
                                                                                            blocks[x,y,z].tUnitSize);

                                        vGen.UpdateBlock(chunk,chunk.blocks[x,blocks.GetLength(1)-1,z]);
                                    } else {
                                        Debug.LogError(string.Format("Chunk ({0},{1},{2}) is null",x,blocks.GetLength(1)-1,z));
                                    }
                                }
                            } else {
                                if ( blocks[x,y-1,z] == null ){
                                    // Create block
                                    blocks[x,y-1,z] = new Block(blocks[x,y,z].center-new Vector3(0,vGen.blockSize,0),
                                                                vGen.blockSize,
                                                                new Vector3(x,y-1,z),
                                                                blocks[x,y,z].tPos,
                                                                blocks[x,y,z].tUnitSize);
                                    Debug.Log(string.Format("Created block ({0},{1},{2})",x,y-1,z));
                                    vGen.UpdateBlock(this,blocks[x,y-1,z]);
                                } else {
                                    // That's weird
                                }
                            }
                            break;
                            #endregion
                            #region Left
                            case Face.left:
                            if ( x-1 < 0 ){
                                if ( neighbors[(int)face] != null ){
                                    if ( neighbors[(int)face].blocks[blocks.GetLength(0)-1,y,z] == null ){
                                        // Create block in neighbor
                                        Block b = blocks[x,y,z];
                                        Vector3 c = b.center-new Vector3(vGen.blockSize,0,0);
                                        neighbors[(int)face].blocks[blocks.GetLength(0)-1,y,z] = new Block(c, vGen.blockSize, new Vector3(blocks.GetLength(0)-1,y,z), b.tPos, b.tUnitSize);
                                        vGen.UpdateBlock(neighbors[(int)face],neighbors[(int)face].blocks[blocks.GetLength(0)-1,y,z]);
                                    } else {
                                        // That's weird
                                    }
                                } else {
                                    // Create chunk
                                    Chunk chunk = vGen.CreateChunk(this, face);
                                    if ( chunk != null ){
                                        Debug.Log(string.Format("Created block ({0},{1},{2})",blocks.GetLength(0)-1,y,z));
                                        chunk.blocks[blocks.GetLength(0)-1,y,z] = new Block(blocks[x,y,z].center-new Vector3(vGen.blockSize,0,0),
                                                                                            vGen.blockSize,
                                                                                            new Vector3(blocks.GetLength(0)-1,y,z),
                                                                                            blocks[x,y,z].tPos,
                                                                                            blocks[x,y,z].tUnitSize);

                                        vGen.UpdateBlock(chunk,chunk.blocks[blocks.GetLength(0)-1,y,z]);
                                    } else {
                                        Debug.LogError(string.Format("Chunk ({0},{1},{2}) is null",blocks.GetLength(0)-1,y,z));
                                    }
                                }
                            } else {
                                if ( blocks[x-1,y,z] == null ){
                                    // Create block
                                    blocks[x-1,y,z] = new Block(blocks[x,y,z].center-new Vector3(vGen.blockSize,0,0),
                                                                vGen.blockSize,
                                                                new Vector3(x-1,y,z),
                                                                blocks[x,y,z].tPos,
                                                                blocks[x,y,z].tUnitSize);
                                    Debug.Log(string.Format("Created block ({0},{1},{2})",x-1,y,z));
                                    vGen.UpdateBlock(this,blocks[x-1,y,z]);
                                } else {
                                    // That's weird
                                }
                            }
                            break;
                            #endregion
                            #region Right
                            case Face.right:
                            if ( x+1 >= blocks.GetLength(0) ){
                                // Check for other chunk
                                if ( neighbors[(int)face] != null ){
                                    if ( neighbors[(int)face].blocks[0,y,z] == null ){
                                        // Create block in neighbor
                                        Block b = blocks[x,y,z];
                                        Vector3 c = b.center+new Vector3(vGen.blockSize,0,0);
                                        neighbors[(int)face].blocks[0,y,z] = new Block(c, vGen.blockSize, new Vector3(0,y,z), b.tPos, b.tUnitSize);
                                        vGen.UpdateBlock(neighbors[(int)face],neighbors[(int)face].blocks[0,y,z]);
                                    } else {
                                        // That's weird
                                    }
                                } else {
                                    // Create chunk
                                    Chunk chunk = vGen.CreateChunk(this, face);
                                    if ( chunk != null ){
                                        Debug.Log(string.Format("Created block ({0},{1},{2})",0,y,z));
                                        chunk.blocks[0,y,z] = new Block(blocks[x,y,z].center+new Vector3(vGen.blockSize,0,0),
                                                                        vGen.blockSize,
                                                                        new Vector3(0,y,z),
                                                                        blocks[x,y,z].tPos,
                                                                        blocks[x,y,z].tUnitSize);

                                        vGen.UpdateBlock(chunk,chunk.blocks[0,y,z]);
                                    }
                                }
                            } else {
                                // Check in current chunk
                                if ( blocks[x+1,y,z] == null ){
                                    // Create block
                                    blocks[x+1,y,z] = new Block(blocks[x,y,z].center+new Vector3(vGen.blockSize,0,0),
                                                                vGen.blockSize,
                                                                new Vector3(x+1,y,z),
                                                                blocks[x,y,z].tPos,
                                                                blocks[x,y,z].tUnitSize);
                                    Debug.Log(string.Format("Created block ({0},{1},{2})",x+1,y,z));
                                    vGen.UpdateBlock(this,blocks[x+1,y,z]);
                                } else {
                                    // That's weird
                                }
                            }
                            break;
                            #endregion
                            }

                            return;
                        }
                    }
                }
            }
        }

    }

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
        if ( Input.GetMouseButtonUp(0) ){
            RaycastHit hit;
            if ( Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit) ){
                string[] args = hit.collider.name.Split(',');
                Debug.Log(string.Format("Destroying block in chunk ({0},{1},{2})",args[0],args[1],args[2]));
                chunks[int.Parse(args[0]),int.Parse(args[1]),int.Parse(args[2])].DestroyBlockAt(hit.point);
                UpdateMesh();
            }
        }
        if ( Input.GetMouseButtonUp(1) ){
            RaycastHit hit;
            if ( Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit) ){
                string[] args = hit.collider.name.Split(',');
                Debug.Log(string.Format("Creating block in chunk ({0},{1},{2})",args[0],args[1],args[2]));
                chunks[int.Parse(args[0]),int.Parse(args[1]),int.Parse(args[2])].CreateBlockAt(hit.point);
                UpdateMesh();
            }
        }
    }
    void OnDrawGizmos(){
        if ( debug ){
            DrawChunkBorders();
        }
    }

    private void DrawChunkBorders(){
        if ( chunks == null ) return;
        foreach (Chunk c in chunks){
            if ( c != null ){
                Gizmos.color = Color.black;
                Gizmos.DrawWireCube(c.center, new Vector3(blockSize*chunkWidth,blockSize*chunkHeight,blockSize*chunkLength));
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
    private void SetupNeighbors(){
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
                                mr.material = voxelSheet;

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
        if ( chunks[cx,cy,cz].blocks[bx,by,bz] == null ){
            Debug.LogError(string.Format("Block ({0},{1},{2}) is null",bx,by,bz));
            return;
        }

        for (int i = 0; i < 6; i++){
            switch (i){  
            case (int)Face.front: 
                if ( bz-1 >= 0 ){
                    chunks[cx,cy,cz].blocks[bx,by,bz].neighbors[i] = chunks[cx,cy,cz].blocks[bx,by,bz-1];
                } else if ( cz-1 >= 0 && chunks[cx,cy,cz-1] != null ){
                    chunks[cx,cy,cz].blocks[bx,by,bz].neighbors[i] = chunks[cx,cy,cz-1].blocks[bx,by,bl-1];
                }
                break;
            case (int)Face.top: 
                if ( by+1 < bh ){
                    chunks[cx,cy,cz].blocks[bx,by,bz].neighbors[i] = chunks[cx,cy,cz].blocks[bx,by+1,bz];
                } else if ( cy+1 < cH && chunks[cx,cy+1,cz] != null ){
                    chunks[cx,cy,cz].blocks[bx,by,bz].neighbors[i] = chunks[cx,cy+1,cz].blocks[bx,0,bz];
                }
                break;
            case (int)Face.left: 
                if ( bx-1 >= 0 ){
                    chunks[cx,cy,cz].blocks[bx,by,bz].neighbors[i] = chunks[cx,cy,cz].blocks[bx-1,by,bz];
                } else if ( cx-1 >= 0 && chunks[cx-1,cy,cz] != null ){
                    chunks[cx,cy,cz].blocks[bx,by,bz].neighbors[i] = chunks[cx-1,cy,cz].blocks[bw-1,by,bz];
                }
                break;
            case (int)Face.right: 
                if ( bx+1 < bw ){
                    chunks[cx,cy,cz].blocks[bx,by,bz].neighbors[i] = chunks[cx,cy,cz].blocks[bx+1,by,bz];
                } else if ( cx+1 < cW && chunks[cx+1,cy,cz] != null ){
                    chunks[cx,cy,cz].blocks[bx,by,bz].neighbors[i] = chunks[cx+1,cy,cz].blocks[0,by,bz];
                }
                break;
            case (int)Face.bottom: 
                if ( by-1 >= 0 ){
                    chunks[cx,cy,cz].blocks[bx,by,bz].neighbors[i] = chunks[cx,cy,cz].blocks[bx,by-1,bz];
                } else if ( cy-1 >= 0 && chunks[cx,cy-1,cz] != null ){
                    chunks[cx,cy,cz].blocks[bx,by,bz].neighbors[i] = chunks[cx,cy-1,cz].blocks[bx,bh-1,bz];
                }
                break;
            case (int)Face.back: 
                if ( bz+1 < bl ){
                    chunks[cx,cy,cz].blocks[bx,by,bz].neighbors[i] = chunks[cx,cy,cz].blocks[bx,by,bz+1];
                } else if ( cz+1 < cL && chunks[cx,cy,cz+1] != null ){
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
                    if ( chunks[x,y,z] == null ) continue;
                    for (int i = 0; i < 6; i++){
                        switch (i){
                        case (int)Face.front:
                            if ( z-1 >= 0 ){
                                chunks[x,y,z].neighbors[i] = chunks[x,y,z-1];
                            } else {
                                chunks[x,y,z].neighbors[i] = null;
                            }
                            break;
                        case (int)Face.top: 
                            if ( y+1 < chunks.GetLength(1) ){
                                chunks[x,y,z].neighbors[i] = chunks[x,y+1,z];
                            } else {
                                chunks[x,y,z].neighbors[i] = null;
                            }
                            break;
                        case (int)Face.left: 
                            if ( x-1 >= 0 ){
                                chunks[x,y,z].neighbors[i] = chunks[x-1,y,z];
                            } else {
                                chunks[x,y,z].neighbors[i] = null;
                            }
                            break;
                        case (int)Face.right: 
                            if ( x+1 < chunks.GetLength(0) ){
                                chunks[x,y,z].neighbors[i] = chunks[x+1,y,z];
                            } else {
                                chunks[x,y,z].neighbors[i] = null;
                            }
                            break;
                        case (int)Face.bottom: 
                            if ( y-1 >= 0 ){
                                chunks[x,y,z].neighbors[i] = chunks[x,y-1,z];
                            } else {
                                chunks[x,y,z].neighbors[i] = null;
                            }
                            break;
                        case (int)Face.back: 
                            if ( z+1 < chunks.GetLength(2) ){
                                chunks[x,y,z].neighbors[i] = chunks[x,y,z+1];
                            } else {
                                chunks[x,y,z].neighbors[i] = null;
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
        #region Front/Back
        if ( face == Face.front || face == Face.back ){
            // Expand chunks
            Chunk[,,] newChunks = new Chunk[chunks.GetLength(0),chunks.GetLength(1),chunks.GetLength(2)+1];

            if ( face == Face.front && chunk.posInWorld.z-1 < 0 ){
                // Shift all chunks backward
                for (int x = 0; x < chunks.GetLength(0); x++){
                    for (int y = 0; y < chunks.GetLength(1); y++){
                        for (int z = 0; z < chunks.GetLength(2); z++){
                            if ( chunks[x,y,z] == null ) continue;
                            chunks[x,y,z].posInWorld = new Vector3(x,y,z+1);
                            newChunks[x,y,z+1] = chunks[x,y,z];
                        }
                    }
                }
            } else {
                // Keep chunks in same place
                for (int x = 0; x < chunks.GetLength(0); x++){
                    for (int y = 0; y < chunks.GetLength(1); y++){
                        for (int z = 0; z < chunks.GetLength(2); z++){
                            newChunks[x,y,z] = chunks[x,y,z];
                        }
                    }
                }
            }

            int amt = face == Face.front ? -1 : 1;
            Vector3 posInWorld = new Vector3(chunk.posInWorld.x,chunk.posInWorld.y,chunk.posInWorld.z+amt);
            newChunks[(int)posInWorld.x,(int)posInWorld.y,(int)posInWorld.z] = new Chunk();
            newChunks[(int)posInWorld.x,(int)posInWorld.y,(int)posInWorld.z].EmptyChunk( new Vector3(chunk.center.x,chunk.center.y,chunk.center.z+amt*blockSize*chunkLength),
                                                                                         posInWorld,
                                                                                         chunkWidth,
                                                                                         chunkHeight,
                                                                                         chunkLength,
                                                                                         blockSize);

            chunks = newChunks;
            UpdateChunk((int)posInWorld.x,(int)posInWorld.y,(int)posInWorld.z);

            return newChunks[(int)posInWorld.x,(int)posInWorld.y,(int)posInWorld.z];
        } else 
        #endregion
        #region Top/Bottom
        if ( face == Face.top || face == Face.bottom ){
            // Expand chunks
            Chunk[,,] newChunks = new Chunk[chunks.GetLength(0),chunks.GetLength(1)+1,chunks.GetLength(2)];

            if ( face == Face.bottom && chunk.posInWorld.y-1 < 0 ){
                // Shift all chunks upward
                for (int x = 0; x < chunks.GetLength(0); x++){
                    for (int y = 0; y < chunks.GetLength(1); y++){
                        for (int z = 0; z < chunks.GetLength(2); z++){
                            if ( chunks[x,y,z] == null ) continue;
                            chunks[x,y,z].posInWorld = new Vector3(x,y+1,z);
                            newChunks[x,y+1,z] = chunks[x,y,z];
                        }
                    }
                }
            } else {
                // Keep chunks in same place
                for (int x = 0; x < chunks.GetLength(0); x++){
                    for (int y = 0; y < chunks.GetLength(1); y++){
                        for (int z = 0; z < chunks.GetLength(2); z++){
                            newChunks[x,y,z] = chunks[x,y,z];
                        }
                    }
                }
            }

            int amt = face == Face.bottom ? -1 : 1;
            Vector3 posInWorld = new Vector3(chunk.posInWorld.x,chunk.posInWorld.y+amt,chunk.posInWorld.z);
            newChunks[(int)posInWorld.x,(int)posInWorld.y,(int)posInWorld.z] = new Chunk();
            newChunks[(int)posInWorld.x,(int)posInWorld.y,(int)posInWorld.z].EmptyChunk( new Vector3(chunk.center.x,chunk.center.y+amt*blockSize*chunkLength,chunk.center.z),
                                                                                         posInWorld,
                                                                                         chunkWidth,
                                                                                         chunkHeight,
                                                                                         chunkLength,
                                                                                         blockSize);

            chunks = newChunks;
            UpdateChunk((int)posInWorld.x,(int)posInWorld.y,(int)posInWorld.z);

            return newChunks[(int)posInWorld.x,(int)posInWorld.y,(int)posInWorld.z];
        } else
        #endregion
        #region Left/Right
        if ( face == Face.left || face == Face.right ){
            // Expand chunks
            Chunk[,,] newChunks = new Chunk[chunks.GetLength(0)+1,chunks.GetLength(1),chunks.GetLength(2)];

            if ( face == Face.left && chunk.posInWorld.x-1 < 0 ){
                // Shift all chunks upward
                for (int x = 0; x < chunks.GetLength(0); x++){
                    for (int y = 0; y < chunks.GetLength(1); y++){
                        for (int z = 0; z < chunks.GetLength(2); z++){
                            if ( chunks[x,y,z] == null ) continue;
                            chunks[x,y,z].posInWorld = new Vector3(x+1,y,z);
                            newChunks[x+1,y,z] = chunks[x,y,z];
                        }
                    }
                }
            } else {
                // Keep chunks in same place
                for (int x = 0; x < chunks.GetLength(0); x++){
                    for (int y = 0; y < chunks.GetLength(1); y++){
                        for (int z = 0; z < chunks.GetLength(2); z++){
                            newChunks[x,y,z] = chunks[x,y,z];
                        }
                    }
                }
            }

            int amt = face == Face.left ? -1 : 1;
            Vector3 posInWorld = new Vector3(chunk.posInWorld.x+amt,chunk.posInWorld.y,chunk.posInWorld.z);
            newChunks[(int)posInWorld.x,(int)posInWorld.y,(int)posInWorld.z] = new Chunk();
            newChunks[(int)posInWorld.x,(int)posInWorld.y,(int)posInWorld.z].EmptyChunk( new Vector3(chunk.center.x+amt*blockSize*chunkLength,chunk.center.y,chunk.center.z),
                                                                                         posInWorld,
                                                                                         chunkWidth,
                                                                                         chunkHeight,
                                                                                         chunkLength,
                                                                                         blockSize);

            chunks = newChunks;
            UpdateChunk((int)posInWorld.x,(int)posInWorld.y,(int)posInWorld.z);

            return newChunks[(int)posInWorld.x,(int)posInWorld.y,(int)posInWorld.z];
        }
        #endregion

        Debug.LogError("Unable to create chunk.");
        return null;
    }
}