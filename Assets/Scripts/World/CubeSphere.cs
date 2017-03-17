using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LibNoise.Unity;
using LibNoise.Unity.Generator;
using LibNoise.Unity.Operator;

// Generate a sphere out of a cube
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class CubeSphere : MonoBehaviour {

	public int gridSize;
	public float radius = 1f;
    public float minRadius;
    public float delay = 0.1f;
    public float frequency = 1f;

	//private Mesh mesh;
	private Vector3[] vertices;
	private Vector3[] normals;
    private List<int> triangles;

    private class SphereLayer {
        public Vector3[] vertices;
        public Vector3[] normals;
        public List<int> triangles;

        public SphereLayer(){}
    }
    private List<SphereLayer> sphereLayers = new List<SphereLayer>();

    private class Block {
        public List<Vector3> vertices;
        public List<int> triangles;

        public Block(){
            vertices = new List<Vector3>();
            triangles = new List<int>();
        }
        public Block(params Vector3[] vertices){
            this.vertices = new List<Vector3>();
            this.vertices.AddRange(vertices);

            CreateTriangles();
        }

        private void CreateTriangles(){
            triangles = new List<int>();
            
		    // Front
            triangles.Add(0);
            triangles.Add(4);
            triangles.Add(1);
            triangles.Add(1);
            triangles.Add(4);
            triangles.Add(5);

            // Back
            triangles.Add(3);
            triangles.Add(7);
            triangles.Add(2);
            triangles.Add(2);
            triangles.Add(7);
            triangles.Add(6);

            // Left
            triangles.Add(2);
            triangles.Add(6);
            triangles.Add(0);
            triangles.Add(0);
            triangles.Add(6);
            triangles.Add(4);

            // Right
            triangles.Add(1);
            triangles.Add(5);
            triangles.Add(3);
            triangles.Add(3);
            triangles.Add(5);
            triangles.Add(7);

            // Top
            triangles.Add(5);
            triangles.Add(4);
            triangles.Add(7);
            triangles.Add(7);
            triangles.Add(4);
            triangles.Add(6);

            // Bottom
            triangles.Add(0);
            triangles.Add(1);
            triangles.Add(2);
            triangles.Add(2);
            triangles.Add(1);
            triangles.Add(3);
        }
    }
    private List<Block> blocks = new List<Block>();

    private class Chunk {
        public List<Block> blocks;

        public Chunk(){
            blocks = new List<Block>();
        }
    }
    private List<Chunk> chunks = new List<Chunk>();

    private Color[] colors = new Color[6]{
        Color.red,
        Color.blue,
        Color.green,
        Color.yellow,
        Color.cyan,
        Color.magenta
    };

	void Awake () {
        for (int i = 0; i < 6; i++)
            chunks.Add(new Chunk());

		StartCoroutine(Generate());
	}
    void OnDrawGizmos(){
        for (int i = 0; i < chunks.Count; i++){
            if ( chunks[i].blocks.Count > 0 ){
                Gizmos.color = colors[i];
                foreach (Block b in chunks[i].blocks){
                    for (int j = 2; j < b.triangles.Count; j+=3){
                        Gizmos.DrawLine(b.vertices[b.triangles[j]], b.vertices[b.triangles[j-1]]);
                        Gizmos.DrawLine(b.vertices[b.triangles[j-1]], b.vertices[b.triangles[j-2]]);
                        Gizmos.DrawLine(b.vertices[b.triangles[j-2]], b.vertices[b.triangles[j]]);
                    }
                }
            }
        }
    }

    // Generate the mesh
	private IEnumerator Generate () {
        #region Create Layers
        while (radius > minRadius){
            CreateVertices();
            CreateTriangles();

            SphereLayer sphereLayer = new SphereLayer();
            sphereLayer.vertices = vertices;
            sphereLayer.normals = normals;
            sphereLayer.triangles = triangles;
            sphereLayers.Add(sphereLayer);
            yield return new WaitForSeconds(delay);

            radius--;
        }
        #endregion
        #region Create Blocks and Chunks
        int ring = (gridSize + gridSize) * 2;

        for (int i = 1; i < sphereLayers.Count; i++){
            SphereLayer sl1 = sphereLayers[i];
            SphereLayer sl2 = sphereLayers[i-1];

		    int v = 0;

            #region Sides
		    for (int y = 0; y < gridSize; y++, v++) {
			    for (int q = 0; q < gridSize; q++, v++) {
				    // v, v + 1, v + ring, v + ring + 1);
                    blocks.Add(new Block(sl1.vertices[v], sl1.vertices[v+1], sl1.vertices[v+ring], sl1.vertices[v+ring+1],
                                         sl2.vertices[v], sl2.vertices[v+1], sl2.vertices[v+ring], sl2.vertices[v+ring+1]));
                                     
                    chunks[0].blocks.Add(blocks[blocks.Count-1]);
                    yield return new WaitForSeconds(delay);
			    }
			    for (int q = 0; q < gridSize; q++, v++) {
				    // v, v + 1, v + ring, v + ring + 1);
                    blocks.Add(new Block(sl1.vertices[v], sl1.vertices[v+1], sl1.vertices[v+ring], sl1.vertices[v+ring+1],
                                         sl2.vertices[v], sl2.vertices[v+1], sl2.vertices[v+ring], sl2.vertices[v+ring+1]));
                               
                    chunks[1].blocks.Add(blocks[blocks.Count-1]);
                    yield return new WaitForSeconds(delay);
			    }
			    for (int q = 0; q < gridSize; q++, v++) {
				    // v, v + 1, v + ring, v + ring + 1);
                    blocks.Add(new Block(sl1.vertices[v], sl1.vertices[v+1], sl1.vertices[v+ring], sl1.vertices[v+ring+1],
                                         sl2.vertices[v], sl2.vertices[v+1], sl2.vertices[v+ring], sl2.vertices[v+ring+1]));
                                     
                    chunks[2].blocks.Add(blocks[blocks.Count-1]);
                    yield return new WaitForSeconds(delay);
			    }
			    for (int q = 0; q < gridSize - 1; q++, v++) {
				    // v, v + 1, v + ring, v + ring + 1);
                    blocks.Add(new Block(sl1.vertices[v], sl1.vertices[v+1], sl1.vertices[v+ring], sl1.vertices[v+ring+1],
                                         sl2.vertices[v], sl2.vertices[v+1], sl2.vertices[v+ring], sl2.vertices[v+ring+1]));
                                     
                    chunks[3].blocks.Add(blocks[blocks.Count-1]);
                    yield return new WaitForSeconds(delay);
			    }
			    // v, v-ring+1, v+ring, v+1);
                blocks.Add(new Block(sl1.vertices[v], sl1.vertices[v-ring+1], sl1.vertices[v+ring], sl1.vertices[v+1],
                                        sl2.vertices[v], sl2.vertices[v-ring+1], sl2.vertices[v+ring], sl2.vertices[v+1]));
                
                chunks[3].blocks.Add(blocks[blocks.Count-1]);         
                yield return new WaitForSeconds(delay);
		    }
            #endregion
            #region Top
		    v = ring * gridSize;
		    for (int x = 0; x < gridSize - 1; x++, v++) {
			    // v, v + 1, v + ring - 1, v + ring);
                blocks.Add(new Block(sl1.vertices[v], sl1.vertices[v+1], sl1.vertices[v+ring-1], sl1.vertices[v+ring],
                                     sl2.vertices[v], sl2.vertices[v+1], sl2.vertices[v+ring-1], sl2.vertices[v+ring]));
                  
                chunks[5].blocks.Add(blocks[blocks.Count-1]);
                yield return new WaitForSeconds(delay);
            }
		    // v, v + 1, v + ring - 1, v + 2);
            blocks.Add(new Block(sl1.vertices[v], sl1.vertices[v+1], sl1.vertices[v+ring-1], sl1.vertices[v+2],
                                    sl2.vertices[v], sl2.vertices[v+1], sl2.vertices[v+ring-1], sl2.vertices[v+2]));
                                     
            chunks[5].blocks.Add(blocks[blocks.Count-1]);
            yield return new WaitForSeconds(delay);

		    int vMin = ring * (gridSize + 1) - 1;
		    int vMid = vMin + 1;
		    int vMax = v + 2;

		    for (int z = 1; z < gridSize - 1; z++, vMin--, vMid++, vMax++) {
			    // vMin, vMid, vMin - 1, vMid + gridSize - 1);
                blocks.Add(new Block(sl1.vertices[vMin], sl1.vertices[vMid], sl1.vertices[vMin-1], sl1.vertices[vMid+gridSize-1],
                                        sl2.vertices[vMin], sl2.vertices[vMid], sl2.vertices[vMin-1], sl2.vertices[vMid+gridSize-1]));
                                     
                chunks[5].blocks.Add(blocks[blocks.Count-1]);
                yield return new WaitForSeconds(delay);
			    for (int x = 1; x < gridSize - 1; x++, vMid++) {
				    // vMid, vMid + 1, vMid+gridSize-1, vMid+gridSize);
                    blocks.Add(new Block(sl1.vertices[vMid], sl1.vertices[vMid+1], sl1.vertices[vMid+gridSize-1], sl1.vertices[vMid+gridSize],
                                            sl2.vertices[vMid], sl2.vertices[vMid+1], sl2.vertices[vMid+gridSize-1], sl2.vertices[vMid+gridSize]));
                                     
                    chunks[5].blocks.Add(blocks[blocks.Count-1]);
                    yield return new WaitForSeconds(delay);
			    }
			    // vMid, vMax, vMid+gridSize-1, vMax+1);
                blocks.Add(new Block(sl1.vertices[vMid], sl1.vertices[vMax], sl1.vertices[vMid+gridSize-1], sl1.vertices[vMax+1],
                                        sl2.vertices[vMid], sl2.vertices[vMax], sl2.vertices[vMid+gridSize-1], sl2.vertices[vMax+1]));
                                     
                chunks[5].blocks.Add(blocks[blocks.Count-1]);
                yield return new WaitForSeconds(delay);
		    }

		    int vTop = vMin - 2;
		    // vMin, vMid, vTop+1, vTop);
            blocks.Add(new Block(sl1.vertices[vMin], sl1.vertices[vMid], sl1.vertices[vTop+1], sl1.vertices[vTop],
                                    sl2.vertices[vMin], sl2.vertices[vMid], sl2.vertices[vTop+1], sl2.vertices[vTop]));
                                     
            chunks[5].blocks.Add(blocks[blocks.Count-1]);
            yield return new WaitForSeconds(delay);
		    for (int x = 1; x < gridSize - 1; x++, vTop--, vMid++) {
			    // vMid, vMid + 1, vTop, vTop-1);
                blocks.Add(new Block(sl1.vertices[vMid], sl1.vertices[vMid+1], sl1.vertices[vTop], sl1.vertices[vTop-1],
                                        sl2.vertices[vMid], sl2.vertices[vMid+1], sl2.vertices[vTop], sl2.vertices[vTop-1]));
                                     
                chunks[5].blocks.Add(blocks[blocks.Count-1]);
                yield return new WaitForSeconds(delay);
		    }
		    // vMid, vTop-2, vTop, vTop-1);
            blocks.Add(new Block(sl1.vertices[vMid], sl1.vertices[vTop-2], sl1.vertices[vTop], sl1.vertices[vTop-1],
                                    sl2.vertices[vMid], sl2.vertices[vTop-2], sl2.vertices[vTop], sl2.vertices[vTop-1]));
                                     
            chunks[5].blocks.Add(blocks[blocks.Count-1]);
            yield return new WaitForSeconds(delay);
            #endregion
            #region Bottom
            v = 1;
		    vMid = vertices.Length - (gridSize - 1) * (gridSize - 1);
		    // ring-1, vMid, 0, 1);
            blocks.Add(new Block(sl1.vertices[ring-1], sl1.vertices[vMid], sl1.vertices[0], sl1.vertices[1],
                                    sl2.vertices[ring-1], sl2.vertices[vMid], sl2.vertices[0], sl2.vertices[1]));
                                     
            chunks[4].blocks.Add(blocks[blocks.Count-1]);
            yield return new WaitForSeconds(delay);
		    for (int x = 1; x < gridSize - 1; x++, v++, vMid++) {
			    // vMid, vMid+1, v, v+1);
                blocks.Add(new Block(sl1.vertices[vMid], sl1.vertices[vMid+1], sl1.vertices[v], sl1.vertices[v+1],
                                        sl2.vertices[vMid], sl2.vertices[vMid+1], sl2.vertices[v], sl2.vertices[v+1]));
                                     
                chunks[4].blocks.Add(blocks[blocks.Count-1]);
                yield return new WaitForSeconds(delay);
		    }
		    // vMid, v+2, v, v+1);
            blocks.Add(new Block(sl1.vertices[vMid], sl1.vertices[v+2], sl1.vertices[v], sl1.vertices[v+1],
                                    sl2.vertices[vMid], sl2.vertices[v+2], sl2.vertices[v], sl2.vertices[v+1]));
                                     
            chunks[4].blocks.Add(blocks[blocks.Count-1]);
            yield return new WaitForSeconds(delay);

		    vMin = ring - 2;
		    vMid -= gridSize - 2;
		    vMax = v + 2;

		    for (int z = 1; z < gridSize - 1; z++, vMin--, vMid++, vMax++) {
			    // vMin, vMid+gridSize-1, vMin+1, vMid);
                blocks.Add(new Block(sl1.vertices[vMin], sl1.vertices[vMid+gridSize-1], sl1.vertices[vMin+1], sl1.vertices[vMid],
                                        sl2.vertices[vMin], sl2.vertices[vMid+gridSize-1], sl2.vertices[vMin+1], sl2.vertices[vMid]));
                                     
                chunks[4].blocks.Add(blocks[blocks.Count-1]);
                yield return new WaitForSeconds(delay);
			    for (int x = 1; x < gridSize - 1; x++, vMid++) {
					// vMid+gridSize-1, vMid+gridSize, vMid, vMid+1);
                    blocks.Add(new Block(sl1.vertices[vMid+gridSize-1], sl1.vertices[vMid+gridSize], sl1.vertices[vMid], sl1.vertices[vMid+1],
                                            sl2.vertices[vMid+gridSize-1], sl2.vertices[vMid+gridSize], sl2.vertices[vMid], sl2.vertices[vMid+1]));
                                     
                    chunks[4].blocks.Add(blocks[blocks.Count-1]);
                    yield return new WaitForSeconds(delay);
			    }
			    // vMid+gridSize-1, vMax+1, vMid, vMax);
                blocks.Add(new Block(sl1.vertices[vMid+gridSize-1], sl1.vertices[vMax+1], sl1.vertices[vMid], sl1.vertices[vMax],
                                        sl2.vertices[vMid+gridSize-1], sl2.vertices[vMax+1], sl2.vertices[vMid], sl2.vertices[vMax]));
                                     
                chunks[4].blocks.Add(blocks[blocks.Count-1]);
                yield return new WaitForSeconds(delay);
		    }

		    vTop = vMin - 1;
		    // vTop+1, vTop, vTop+2, vMid);
            blocks.Add(new Block(sl1.vertices[vTop+1], sl1.vertices[vTop], sl1.vertices[vTop+2], sl1.vertices[vMid],
                                    sl2.vertices[vTop+1], sl2.vertices[vTop], sl2.vertices[vTop+2], sl2.vertices[vMid]));
                                     
            chunks[4].blocks.Add(blocks[blocks.Count-1]);
            yield return new WaitForSeconds(delay);
		    for (int x = 1; x < gridSize - 1; x++, vTop--, vMid++) {
			    // vTop, vTop-1, vMid, vMid+1);
                blocks.Add(new Block(sl1.vertices[vTop], sl1.vertices[vTop-1], sl1.vertices[vMid], sl1.vertices[vMid+1],
                                        sl2.vertices[vTop], sl2.vertices[vTop-1], sl2.vertices[vMid], sl2.vertices[vMid+1]));
                                     
                chunks[4].blocks.Add(blocks[blocks.Count-1]);
                yield return new WaitForSeconds(delay);
		    }
		    // vTop, vTop-1, vMid, vTop-2);
            blocks.Add(new Block(sl1.vertices[vTop], sl1.vertices[vTop-1], sl1.vertices[vMid], sl1.vertices[vTop-2],
                                    sl2.vertices[vTop], sl2.vertices[vTop-1], sl2.vertices[vMid], sl2.vertices[vTop-2]));
                                     
            chunks[4].blocks.Add(blocks[blocks.Count-1]);
            yield return new WaitForSeconds(delay);
            #endregion
        }

        #endregion 
        yield break;
	}

    // Create the vertices of the sphere
	private void CreateVertices () {
		int cornerVertices = 8;
		int edgeVertices = (gridSize + gridSize + gridSize - 3) * 4;
		int faceVertices = (
			(gridSize - 1) * (gridSize - 1) +
			(gridSize - 1) * (gridSize - 1) +
			(gridSize - 1) * (gridSize - 1)) * 2;
		vertices = new Vector3[cornerVertices + edgeVertices + faceVertices];
		normals = new Vector3[vertices.Length];

		int v = 0;
		for (int y = 0; y <= gridSize; y++) {
			for (int x = 0; x <= gridSize; x++) {
				SetVertex(v++, x, y, 0);
			}
			for (int z = 1; z <= gridSize; z++) {
				SetVertex(v++, gridSize, y, z);
			}
			for (int x = gridSize - 1; x >= 0; x--) {
				SetVertex(v++, x, y, gridSize);
			}
			for (int z = gridSize - 1; z > 0; z--) {
				SetVertex(v++, 0, y, z);
			}
		}
		for (int z = 1; z < gridSize; z++) {
			for (int x = 1; x < gridSize; x++) {
				SetVertex(v++, x, gridSize, z);
			}
		}
		for (int z = 1; z < gridSize; z++) {
			for (int x = 1; x < gridSize; x++) {
				SetVertex(v++, x, 0, z);
			}
		}
	}
    // Create the vertex based on vertices count and a given vertex position from a cube
	private void SetVertex (int i, int x, int y, int z) {
		Vector3 v = new Vector3(x, y, z) * 2f / gridSize - Vector3.one;
		float x2 = v.x * v.x;
		float y2 = v.y * v.y;
		float z2 = v.z * v.z;
		Vector3 s;
		s.x = v.x * Mathf.Sqrt(1f - y2 / 2f - z2 / 2f + y2 * z2 / 3f);
		s.y = v.y * Mathf.Sqrt(1f - x2 / 2f - z2 / 2f + x2 * z2 / 3f);
		s.z = v.z * Mathf.Sqrt(1f - x2 / 2f - y2 / 2f + x2 * y2 / 3f);
		normals[i] = s;
		vertices[i] = normals[i] * radius;
	}
    // Create the triangles connecting each vertex to form the mesh
	private void CreateTriangles () {
		int[] trianglesZ = new int[(gridSize * gridSize) * 12];
		int[] trianglesX = new int[(gridSize * gridSize) * 12];
		int[] trianglesY = new int[(gridSize * gridSize) * 12];
		int ring = (gridSize + gridSize) * 2;
		int tZ = 0, tX = 0, tY = 0, v = 0;

		for (int y = 0; y < gridSize; y++, v++) {
			for (int q = 0; q < gridSize; q++, v++) {
				tZ = SetQuad(trianglesZ, tZ, v, v + 1, v + ring, v + ring + 1);
			}
			for (int q = 0; q < gridSize; q++, v++) {
				tX = SetQuad(trianglesX, tX, v, v + 1, v + ring, v + ring + 1);
			}
			for (int q = 0; q < gridSize; q++, v++) {
				tZ = SetQuad(trianglesZ, tZ, v, v + 1, v + ring, v + ring + 1);
			}
			for (int q = 0; q < gridSize - 1; q++, v++) {
				tX = SetQuad(trianglesX, tX, v, v + 1, v + ring, v + ring + 1);
			}
			tX = SetQuad(trianglesX, tX, v, v - ring + 1, v + ring, v + 1);
		}

		tY = CreateTopFace(trianglesY, tY, ring);
		tY = CreateBottomFace(trianglesY, tY, ring);
        
        triangles = new List<int>();
        triangles.AddRange(trianglesZ);
        triangles.AddRange(trianglesY);
        triangles.AddRange(trianglesX);
	}
    // Create the top face of the spherified cube
	private int CreateTopFace (int[] triangles, int t, int ring) {
		int v = ring * gridSize;
		for (int x = 0; x < gridSize - 1; x++, v++) {
			t = SetQuad(triangles, t, v, v + 1, v + ring - 1, v + ring);
		}
		t = SetQuad(triangles, t, v, v + 1, v + ring - 1, v + 2);

		int vMin = ring * (gridSize + 1) - 1;
		int vMid = vMin + 1;
		int vMax = v + 2;

		for (int z = 1; z < gridSize - 1; z++, vMin--, vMid++, vMax++) {
			t = SetQuad(triangles, t, vMin, vMid, vMin - 1, vMid + gridSize - 1);
			for (int x = 1; x < gridSize - 1; x++, vMid++) {
				t = SetQuad(
					triangles, t,
					vMid, vMid + 1, vMid + gridSize - 1, vMid + gridSize);
			}
			t = SetQuad(triangles, t, vMid, vMax, vMid + gridSize - 1, vMax + 1);
		}

		int vTop = vMin - 2;
		t = SetQuad(triangles, t, vMin, vMid, vTop + 1, vTop);
		for (int x = 1; x < gridSize - 1; x++, vTop--, vMid++) {
			t = SetQuad(triangles, t, vMid, vMid + 1, vTop, vTop - 1);
		}
		t = SetQuad(triangles, t, vMid, vTop - 2, vTop, vTop - 1);

		return t;
	}
    // Create the bottom face of the spherified cube
	private int CreateBottomFace (int[] triangles, int t, int ring) {
		int v = 1;
		int vMid = vertices.Length - (gridSize - 1) * (gridSize - 1);
		t = SetQuad(triangles, t, ring - 1, vMid, 0, 1);
		for (int x = 1; x < gridSize - 1; x++, v++, vMid++) {
			t = SetQuad(triangles, t, vMid, vMid + 1, v, v + 1);
		}
		t = SetQuad(triangles, t, vMid, v + 2, v, v + 1);

		int vMin = ring - 2;
		vMid -= gridSize - 2;
		int vMax = v + 2;

		for (int z = 1; z < gridSize - 1; z++, vMin--, vMid++, vMax++) {
			t = SetQuad(triangles, t, vMin, vMid + gridSize - 1, vMin + 1, vMid);
			for (int x = 1; x < gridSize - 1; x++, vMid++) {
				t = SetQuad(
					triangles, t,
					vMid + gridSize - 1, vMid + gridSize, vMid, vMid + 1);
			}
			t = SetQuad(triangles, t, vMid + gridSize - 1, vMax + 1, vMid, vMax);
		}

		int vTop = vMin - 1;
		t = SetQuad(triangles, t, vTop + 1, vTop, vTop + 2, vMid);
		for (int x = 1; x < gridSize - 1; x++, vTop--, vMid++) {
			t = SetQuad(triangles, t, vTop, vTop - 1, vMid, vMid + 1);
		}
		t = SetQuad(triangles, t, vTop, vTop - 1, vMid, vTop - 2);

		return t;
	}
    // Create the face using the triangles and four vertices
	private static int
	SetQuad (int[] triangles, int i, int v00, int v10, int v01, int v11) {
		triangles[i] = v00;
		triangles[i + 1] = triangles[i + 4] = v01;
		triangles[i + 2] = triangles[i + 3] = v10;
		triangles[i + 5] = v11;
		return i + 6;
	}

}