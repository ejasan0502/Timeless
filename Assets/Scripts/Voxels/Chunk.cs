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
                    blocks[x,y,z] = new Block(new Vector3(X,Y,Z),size,new Vector3(x,y,z));
                }
            } 
        }
    }
}
