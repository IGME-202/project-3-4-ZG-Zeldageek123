using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGen : MonoBehaviour
{
    /// <summary>
    /// Author: Cathryn Szulczewski
    /// Purpose: Create a heightmap using perlin noise for random terrain
    /// Errors: None known
    /// </summary>
    /// 

    private TerrainData myTerrainData;
    public Vector3 worldSize;
    public int resolution = 129;            // number of vertices along X and Z axes. 129 x 129

    // Start is called before the first frame update
    void Start()
    {
        myTerrainData = gameObject.GetComponent<TerrainCollider>().terrainData;
        worldSize = new Vector3(100, 50, 100);
        myTerrainData.size = worldSize;
        myTerrainData.heightmapResolution = resolution;
    }

        // Update is called once per frame
        void Update()
    {
        
    }
}
