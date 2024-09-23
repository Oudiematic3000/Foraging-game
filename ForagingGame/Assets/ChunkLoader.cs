using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkLoader : MonoBehaviour
{
    private Terrain[] chunks;
    public float loadDistance;
    public int chunkSize;
    public float checkRate;
    void Start()
    {
        chunks = FindObjectsOfType<Terrain>();
        // check the chunks every 'checkRate' seconds
        InvokeRepeating("CheckChunks", 0.0f, checkRate);
    }

    void CheckChunks()
    {
        Vector3 playerPos = transform.position;
        playerPos.y = 0;
        foreach (Terrain chunk in chunks)
        {
           
            Vector3 chunkCenterPos = chunk.transform.position + new Vector3(chunkSize / 2, 0, chunkSize / 2);

            if (!IsTerrainInView(Camera.main, chunk))
                chunk.gameObject.SetActive(false);
            else
                chunk.gameObject.SetActive(true);
        }
    }

     bool IsTerrainInView(Camera cam, Terrain terrain)
    {
        // Get the frustum planes of the camera
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(cam);

        // Get the terrain's bounding box
        Bounds terrainBounds = terrain.terrainData.bounds;
        terrainBounds.center = terrain.GetPosition() + terrainBounds.center; // Adjust bounds to world position

        // Check if the terrain's bounds intersect with the frustum planes
        return GeometryUtility.TestPlanesAABB(planes, terrainBounds);
    }
}
