using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    public int width = 256;
    public int height = 256;
    public float scale = 20f;
    public Mesh terrainMesh;
    public Material terrainMaterial;

    void Start()
    {
        GenerateTerrain();
    }

    void GenerateTerrain()
    {
        Vector3[] vertices = new Vector3[width * height];
        int[] triangles = new int[(width - 1) * (height - 1) * 6];
        Vector2[] uv = new Vector2[width * height];

        int vertexIndex = 0;
        int triangleIndex = 0;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float xCoord = (float)x / width * scale;
                float yCoord = (float)y / height * scale;
                float noise = Mathf.PerlinNoise(xCoord, yCoord);

                vertices[vertexIndex] = new Vector3(x, noise * 10f, y);
                uv[vertexIndex] = new Vector2((float)x / width, (float)y / height);

                if (x != width - 1 && y != height - 1)
                {
                    triangles[triangleIndex + 0] = vertexIndex;
                    triangles[triangleIndex + 1] = vertexIndex + width + 1;
                    triangles[triangleIndex + 2] = vertexIndex + width;
                    triangles[triangleIndex + 3] = vertexIndex;
                    triangles[triangleIndex + 4] = vertexIndex + 1;
                    triangles[triangleIndex + 5] = vertexIndex + width + 1;

                    triangleIndex += 6;
                }

                vertexIndex++;
            }
        }

        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uv;
        mesh.RecalculateNormals();

        terrainMesh = mesh;

        GetComponent<MeshFilter>().mesh = mesh;
        GetComponent<MeshRenderer>().material = terrainMaterial;
    }
}
