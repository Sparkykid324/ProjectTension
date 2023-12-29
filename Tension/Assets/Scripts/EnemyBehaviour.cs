using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class EnemyChasePlayer : MonoBehaviour
{
    public int maxVisitedWaypoints = 5; // Maximum number of waypoints the enemy can visit
    private NavMeshAgent agent; // Reference to the NavMeshAgent component
    private Transform player; // Reference to the player's Transform
    public float chaseDistance = 10.0f; // Distance at which the enemy starts chasing the player
    private float stoppingDistance = 1.0f; // Distance at which the enemy stops when it reaches the player
    public float fieldOfViewAngle = 90.0f; // Field of view angle within which the enemy can detect the player
    private List<Transform> waypoints = new List<Transform>(); // List to store available waypoints
    private List<Transform> visitedWaypoints = new List<Transform>(); // List to track visited waypoints
    public bool chasingPlayer = false; // Flag to indicate whether the enemy is currently chasing the player
    public Vector3 lastKnownPlayerPosition; // The last known position of the player
    public float timeSinceLastSight; // Time elapsed since the enemy last saw the player
    public float waitDuration = 3.0f; // Duration for which the enemy waits after losing sight of the player

    void Start()
    {
        agent = GetComponent<NavMeshAgent>(); // Get the NavMeshAgent component attached to this GameObject
        player = GameObject.FindGameObjectWithTag("Player").transform; // Find and store the player's Transform
        FindWaypoints(); // Populate the list of available waypoints in the scene
        MoveToRandomWaypoint(); // Start by moving to a random waypoint
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= chaseDistance)
        {
            if (CanSeePlayer())
            {
                agent.SetDestination(player.position); // Start chasing the player
                chasingPlayer = true;
                chaseDistance = 100.0f; // Increase chase distance (e.g., for longer visibility)
                fieldOfViewAngle = 300.0f; // Increase field of view angle (e.g., for wider visibility)

                if (distanceToPlayer <= stoppingDistance)
                {
                    // You can add code here to attack the player or perform other actions
                }
            }
            else if (chasingPlayer)
            {
                timeSinceLastSight += Time.deltaTime;
                if (timeSinceLastSight >= waitDuration)
                {
                    ReturnToLastKnownPlayerPosition();
                    timeSinceLastSight = 0;
                }

                // if enemy is close to last known player position,stop chasing player
                if (Vector3.Distance(transform.position, lastKnownPlayerPosition) < stoppingDistance)
                {
                    chasingPlayer = false;
                    chaseDistance = 10.0f; // Reset chase distance
                    fieldOfViewAngle = 90.0f; // Reset field of view angle
                    MoveToRandomWaypoint();
                }
            }
        }
        else if (!chasingPlayer && agent.remainingDistance < 0.1f)
        {
            MoveToRandomWaypoint();
        }
    }

    bool CanSeePlayer()
    {
        Vector3 directionToPlayer = player.position - transform.position;
        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

        if (angleToPlayer < fieldOfViewAngle * 0.5f)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, directionToPlayer, out hit, chaseDistance))
            {
                if (hit.transform == player)
                {
                    lastKnownPlayerPosition = player.position; // Mark the player's position
                    Debug.DrawRay(transform.position, directionToPlayer, Color.green); // Draw a green ray to indicate visibility
                    return true;
                }
            }
        }
        Debug.DrawRay(transform.position, directionToPlayer, Color.red); // Draw a red ray to indicate no visibility
        return false;
    }

    void FindWaypoints()
    {
        GameObject[] waypointObjects = GameObject.FindGameObjectsWithTag("Waypoint");
        foreach (GameObject waypointObject in waypointObjects)
        {
            waypoints.Add(waypointObject.transform);
        }
    }

    void MoveToRandomWaypoint()
    {
        if (waypoints.Count == 0)
        {
            Debug.LogWarning("No waypoints are found.");
            return;
        }

        List<Transform> availableWaypoints = new List<Transform>(waypoints);

        int count = Mathf.Min(maxVisitedWaypoints, visitedWaypoints.Count);
        for (int i = 0; i < count; i++)
        {
            availableWaypoints.Remove(visitedWaypoints[visitedWaypoints.Count - 1]);
            visitedWaypoints.RemoveAt(visitedWaypoints.Count - 1);
        }

        if (availableWaypoints.Count > 0)
        {
            int randomIndex = Random.Range(0, availableWaypoints.Count);
            Transform targetWaypoint = availableWaypoints[randomIndex];

            agent.SetDestination(targetWaypoint.position); // Move to the selected waypoint
            visitedWaypoints.Add(targetWaypoint); // Track that the waypoint has been visited
        }
    }

    void ReturnToLastKnownPlayerPosition()
    {
        agent.SetDestination(lastKnownPlayerPosition); // Return to the last known player position
    }
}
