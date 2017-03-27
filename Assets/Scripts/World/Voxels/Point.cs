using UnityEngine;
using System.Collections;

[System.Serializable]
public struct Point {

    public int x, y, z;

    public Point(int x, int y, int z){
        this.x = x;
        this.y = y;
        this.z = z;
    }
    public Vector3 ToVector3(){
        return new Vector3(x,y,z);
    }

    public static Point Zero {
        get {
            return new Point(0,0,0);
        }
    }
    public static Point operator*(Point p1, Point p2){
        return new Point(p1.x*p2.x, p1.y*p2.y, p1.z*p2.z);
    }
    public static Point operator*(Point p, int val){
        return new Point(p.x*val, p.y*val, p.z*val);
    }
    public static Point operator*(int val, Point p){
        return new Point(p.x*val, p.y*val, p.z*val);
    }
    public static Point operator/(Point p, int val){
        return new Point(p.x*val, p.y*val, p.z*val);
    }
    public static Point operator+(Point p1, Point p2){
        return new Point(p1.x+p2.x,p1.y+p2.y,p1.z+p2.z);
    }
    public override string ToString(){
        return string.Format("({0},{1},{2})",x,y,z);
    }

}
