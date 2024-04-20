using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class EnemyChasePlayer : MonoBehaviour
{
    //Movement
    private NavMeshAgent agent;
    private Transform player; 
    private float stoppingDistance = 10.0f;
    public List<Transform> waypoints = new List<Transform>(); 
    public List<Transform> visitedWaypoints = new List<Transform>();
    public int maxVisitedWaypoints = 5;
    private bool isMoving;
    public float rotationSpeed = 999999.1f; // Adjust as needed


    //Chasing
    public float chaseDistance = 30.0f;   
    public float fieldOfViewAngle = 90.0f; 
    public bool chasingPlayer = false;
    public Vector3 lastKnownPlayerPosition; 
    public float timeSinceLastSight;
    public float waitDuration = 3.0f; 

    //Death
    public GameObject ammoPickup;
    public float maxHealth = 3f;
    public float currentHealth = 3f;

    //Attack
    private bool canAttack = true;
    private bool isCooldownActive = false;
    public float attackCooldown = 1f;
    public int hitPlayerCount = 0;
    public GameObject gunshotLight;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>(); // Get the NavMeshAgent component attached to this GameObject
        player = GameObject.FindGameObjectWithTag("Player").transform; // Find and store the player's Transform
        FindWaypoints(); // Populate the list of available waypoints in the scene
        MoveToRandomWaypoint(); // Start by moving to a random waypoint
        currentHealth = maxHealth;

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
                                
                // Calculate the direction to the player
                Vector3 targetDirection = (player.position - transform.position).normalized;

                // Calculate the rotation needed to look at the player
                Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

                // Smoothly rotate towards the player
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    

                if (distanceToPlayer <= stoppingDistance)
                {
                    agent.isStopped = true; // Stops the enemy from moving
                    if(canAttack && !isCooldownActive)
                    {
    
                        AttackPlayer();
                        isCooldownActive = true;
                        StartCoroutine(AttackCooldown());
                        Debug.Log("Attacking");
                    }
                }
                else
                {
                    hitPlayerCount = 0;
                    agent.isStopped = false; // Allows the enemy to move again
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
                    chaseDistance = 30.0f; // Reset chase distance
                    fieldOfViewAngle = 90.0f; // Reset field of view angle
                    isMoving = true;
                    FindWaypoints();
                    MoveToRandomWaypoint();
                }
            }
        }

        else if (!chasingPlayer && !isMoving && !agent.pathPending && agent.remainingDistance < 1.0f)
        {
            isMoving = true;
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
        waypoints.Clear();
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
            return;
        }

        int randomIndex = Random.Range(0, waypoints.Count);
        Transform randomWaypoint = waypoints[randomIndex];


        agent.SetDestination(randomWaypoint.position); // Move to the random waypoint

        // Filters Waypoints
        visitedWaypoints.Add(randomWaypoint);
        waypoints.Remove(randomWaypoint);

        int i = 0;
        while (visitedWaypoints.Count > 5 && i < visitedWaypoints.Count)
        {
            waypoints.Add(visitedWaypoints[i]); // Add it back to the waypoints list
            visitedWaypoints.RemoveAt(i);
            i = -1; // Start over from the beginning of the list
            i++;
        }
        isMoving = false;
    }

    void ReturnToLastKnownPlayerPosition()
    {
        agent.SetDestination(lastKnownPlayerPosition); // Return to the last known player position
    }
    void AttackPlayer()
{
     // Start shooting coroutine
    StartCoroutine(FireShots());
    
    // // Calculate the direction to the player
    // Vector3 targetDirection = (player.position - transform.position).normalized;

    // // Calculate the rotation needed to look at the player
    // Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

    // // Smoothly rotate towards the player
    // transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    
}

    IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
        isCooldownActive = false;
    }

    IEnumerator FireShots()
    {

        for (int i = 0; i < 3; i++)
        {
            gunshotLight.SetActive(true);
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity))
            {
                // Check if the ray hits the player
                if (hit.transform.CompareTag("Player"))
                {
                    hitPlayerCount++;
                    if (hitPlayerCount == 3)
                    {
                        hit.transform.GetComponent<PlayerHealth>().TakeDamage();
                        hitPlayerCount = 0;
                    }
                    
                }
            }
            yield return new WaitForSeconds(attackCooldown);
            gunshotLight.SetActive(false);
        }

    }

    public void TakeDamage()
    {
        if (currentHealth <= 0)
        {
            Instantiate(ammoPickup, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        currentHealth -= 1f;
    }

}
