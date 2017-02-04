using UnityEngine;
using System.Collections;

public class Chunk {

    public Vector3 center;
    public Block[,,] blocks;
    public Chunk[] neighbors;

    public Chunk(Vector3 c, int w, int h, int l, float blockSize){
        center = c;
        blocks = new Block[w,h,l];
        neighbors = new Chunk[6];

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
                    blocks[x,y,z] = new Block(new Vector3(X,Y,Z),size,new Vector3(x,y,z),new Vector2(0,0),0.25f);
                }
            } 
        }
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
                        blocks[x,y,z] = null;
                    }
                }
            }
        }
    }
}
