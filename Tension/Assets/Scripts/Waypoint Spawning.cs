using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WaypointSpawner : MonoBehaviour
{
    private List<Vector3> waypoints = new List<Vector3>();
    private List<GameObject> debugCubes = new List<GameObject>();
    private bool areWaypointsActive = true;

    void Start()
    {
        SpawnWaypoints();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            areWaypointsActive = !areWaypointsActive;
            ToggleWaypointVisibility(areWaypointsActive);
        }
    }

    void SpawnWaypoints()
    {
        NavMeshTriangulation triangulation = NavMesh.CalculateTriangulation();

        for (int i = 0; i < triangulation.indices.Length; i += 3)
        {
            int index1 = triangulation.indices[i];
            int index2 = triangulation.indices[i + 1];
            int index3 = triangulation.indices[i + 2];

            Vector3 vertex1 = triangulation.vertices[index1];
            Vector3 vertex2 = triangulation.vertices[index2];
            Vector3 vertex3 = triangulation.vertices[index3];

            // Calculate the center of the triangle
            Vector3 center = (vertex1 + vertex2 + vertex3) / 3;

            // Adjust the y-coordinate to be 1 unit above the NavMesh
            center.y += 1f;

            waypoints.Add(center);

            // Create a debug cube at the waypoint position
            GameObject debugCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            debugCube.transform.position = center;
            debugCube.SetActive(areWaypointsActive);
            debugCubes.Add(debugCube);
            debugCube.tag = "Waypoint";
        }
    }

    void ToggleWaypointVisibility(bool enable)
    {
        foreach (GameObject debugCube in debugCubes)
        {
            debugCube.SetActive(enable);
        }
    }
}