using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// https://www.youtube.com/watch?v=3MoHJtBnn2U
public class WaterNoiseGenerator : MonoBehaviour
{
    public float power = 3;
    public float scale = 1;
    public float timeScale = 1;
    public float planeThreshhold = 0.1f;

    private float xOffset;
    private float yOffset;
    private MeshFilter mf;

    void Start()
    {
        mf = GetComponent<MeshFilter>();
        MakeNoise();
    }

    // Update is called once per frame
    void Update()
    {
        MakeNoise();
        xOffset += Time.deltaTime * timeScale;
        if(yOffset <= planeThreshhold) {
            yOffset += Time.deltaTime * timeScale;
        }
        if(yOffset >= power) {
            yOffset -= Time.deltaTime * timeScale;
        }
    }

    private void MakeNoise() {
        Vector3[] vertices = mf.mesh.vertices;

        for(int i = 0; i < vertices.Length; i++) {
            Vector3 vertex = vertices[i];
            vertices[i].y = CalculateHeight(vertex.x, vertex.z) * power;
        }
        mf.mesh.vertices = vertices;
    }

    private float CalculateHeight(float x, float y) {
        float xCoord = x * scale + xOffset;
        float yCoord = y * scale + yOffset;
        return Mathf.PerlinNoise(xCoord, yCoord);
    }
}
