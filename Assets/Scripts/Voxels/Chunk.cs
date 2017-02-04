using UnityEngine;
using System.Collections;

public class Chunk {

    public Vector3 center;
    public Vector3 posInWorld;
    public Block[,,] blocks;
    public Chunk[] neighbors;

    private VoxelGenerator vGen;

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
                    CreateBlock(center+new Vector3(X,Y,Z),size,new Vector3(x,y,z),new Vector2(0,0),0.25f);
                }
            } 
        }
    }

    public void CreateBlock(Vector3 center, float size, Vector3 posInChunk, Vector2 tPos, float tUnit){
        blocks[(int)posInChunk.x,(int)posInChunk.y,(int)posInChunk.z] = new Block(center,size,posInChunk,tPos,tUnit);
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
                        switch(blocks[x,y,z].GetFaceFromPoint(point)){
                        case Face.front:
                        if ( z-1 < 0 ){
                            if ( neighbors[(int)Face.front] != null ){
                                if ( neighbors[(int)Face.front].blocks[x,y,blocks.GetLength(2)-1] == null ){
                                    // Create block in neighbor
                                } else {
                                    // That's weird
                                }
                            } else {
                                // Create chunk
                            }
                        } else {
                            if ( blocks[x,y,z-1] == null ){
                                // Create block
                                CreateBlock(blocks[x,y,z].center-new Vector3(0,0,vGen.blockSize),
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
                        case Face.back:
                        break;
                        case Face.top:
                        break;
                        case Face.bottom:
                        break;
                        case Face.left:
                        break;
                        case Face.right:
                        break;
                        }

                        return;
                    }
                }
            }
        }
    }

}
