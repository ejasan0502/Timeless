using UnityEngine;
using System.Collections;

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
    public void  CreateBlockAt(Vector3 point){
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
