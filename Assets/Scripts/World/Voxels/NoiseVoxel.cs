using UnityEngine;
using System.Collections;

// Saves the index of the chunk and block with a bool indicating if its an empty block
public class NoiseVoxel {

    public Point chunkIndex;
    public Point blockIndex;
    public bool isEmpty;

    public NoiseVoxel(Point chunkIndex, Point blockIndex, bool isEmpty){
        this.chunkIndex = chunkIndex;
        this.blockIndex = blockIndex;
        this.isEmpty = isEmpty;
    }

}
