using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    private Mesh mesh;
    public Vector3[] vertices;
    public int[] lines;

    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        mesh.name = "MyMesh";
        Vector3 p0 = new Vector3(-1f, -1f, -1f);
        Vector3 p1 = new Vector3(1f, -1f, -1f);
        Vector3 p2 = new Vector3(1f, -1f, -3f);
        Vector3 p3 = new Vector3(-1f, -1f, -3f);
        Vector3 p4 = new Vector3(-1f, 1f, -1f);
        Vector3 p5 = new Vector3(1f, 1f, -1f);
        Vector3 p6 = new Vector3(1f, 1f, -3f);
        Vector3 p7 = new Vector3(-1f, 1f, -3f);

        vertices = new Vector3[]
        {
	        // Bottom
	        p0, p1, p2, p3,
 	        // Left
	        p7, p4, p0, p3,
 	        // Front
	        p4, p5, p1, p0,
 	        // Back
	        p6, p7, p3, p2,
 	        // Right
	        p5, p6, p2, p1,
 	        // Top
	        p7, p6, p5, p4
        };

        int[] triangles = new int[]
        {
            3, 1, 0, // Bottom
	        3, 2, 1,
            7, 5, 4, // Left
	        7, 6, 5,
            11, 9, 8, // Front
	        11, 10, 9,
            15, 13, 12, // Back
	        15, 14, 13,
            19, 17, 16, // Right
	        19, 18, 17,
            23, 21, 20, // Top
	        23, 22, 21,
        };

        lines = new int[]
        {
            0, 1,
            0, 3,
            0, 5,
            1, 2,
            1, 9,
            2, 3,
            2, 12,
            3, 4,
            5, 9,
            5, 4,
            9, 12,
            12, 4
        };

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        
        Material material = new Material(Shader.Find("Hidden/Internal-Colored"));
        material.SetInteger("_Wireframe", 1);

        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.material = material;
        
    }
}
