using UnityEngine;
using System.Collections;

public class Block {

    public Vector3 scenePos;
    public Point chunkPos;
    public bool isEmpty;
    public Block[] neighbors;

    public Vector3[] vertices;  // 8 vertices that composes the block

    public Block(){
        isEmpty = false;
        neighbors = new Block[6];
    }

}
