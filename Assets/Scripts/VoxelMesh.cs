using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class VoxelPlaneGenerator : MonoBehaviour
{
    public int width = 10;  // Number of cubes in X
    public int depth = 10;  // Number of cubes in Z
    public float voxelSize = 1f; // Size of each cube

    void Start()
    {
        GenerateVoxelPlane();
    }

    void GenerateVoxelPlane()
    {
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();

        // Loop through grid positions
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < depth; z++)
            {
                Vector3 position = new Vector3(x * voxelSize, 0, z * voxelSize);
                AddCube(position, vertices, triangles);
            }
        }

        // Create the new voxel mesh
        Mesh voxelMesh = new Mesh();
        voxelMesh.vertices = vertices.ToArray();
        voxelMesh.triangles = triangles.ToArray();
        voxelMesh.RecalculateNormals();

        // Assign mesh to object
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        if (meshFilter != null)
        {
            meshFilter.mesh = voxelMesh;
        }
    }

    void AddCube(Vector3 pos, List<Vector3> verts, List<int> tris)
    {
        int startIndex = verts.Count;

        // Define 8 vertices for a cube
        Vector3[] cubeVertices = new Vector3[]
        {
            pos + new Vector3(0, 0, 0), // 0 Bottom-Left-Front
            pos + new Vector3(voxelSize, 0, 0), // 1 Bottom-Right-Front
            pos + new Vector3(voxelSize, 0, voxelSize), // 2 Bottom-Right-Back
            pos + new Vector3(0, 0, voxelSize), // 3 Bottom-Left-Back
            pos + new Vector3(0, voxelSize, 0), // 4 Top-Left-Front
            pos + new Vector3(voxelSize, voxelSize, 0), // 5 Top-Right-Front
            pos + new Vector3(voxelSize, voxelSize, voxelSize), // 6 Top-Right-Back
            pos + new Vector3(0, voxelSize, voxelSize) // 7 Top-Left-Back
        };

        verts.AddRange(cubeVertices);

        // Define 6 cube faces (2 triangles per face)
        int[] cubeTriangles = new int[]
        {
            startIndex, startIndex + 1, startIndex + 5, startIndex, startIndex + 5, startIndex + 4, // Front
            startIndex + 1, startIndex + 2, startIndex + 6, startIndex + 1, startIndex + 6, startIndex + 5, // Right
            startIndex + 2, startIndex + 3, startIndex + 7, startIndex + 2, startIndex + 7, startIndex + 6, // Back
            startIndex + 3, startIndex, startIndex + 4, startIndex + 3, startIndex + 4, startIndex + 7, // Left
            startIndex + 4, startIndex + 5, startIndex + 6, startIndex + 4, startIndex + 6, startIndex + 7, // Top
            startIndex + 3, startIndex + 2, startIndex + 1, startIndex + 3, startIndex + 1, startIndex // Bottom
        };

        tris.AddRange(cubeTriangles);
    }
}
