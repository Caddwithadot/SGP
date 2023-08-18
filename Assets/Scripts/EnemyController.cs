using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public float speed = 5f; // Speed of the enemy
    public float radius = 10f; // Radius within which the enemy can detect players
    public float avoidanceRadius = 2f; // Radius for collision avoidance
    public float avoidanceTime = 1f; // Duration of evasive action after collision
    private GameObject target; // The player to follow
    private NavMeshAgent agent; // Reference to the NavMeshAgent component
    private float originalSpeed; // Original speed of the enemy
    private float avoidanceTimer; // Timer for evasive action

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        originalSpeed = agent.speed;
        avoidanceTimer = 0f;
    }

    private void Update()
    {
        FindNearestPlayer();

        if (target != null)
        {
            if (avoidanceTimer <= 0f)
            {
                agent.isStopped = false;
                agent.speed = originalSpeed;
                agent.SetDestination(target.transform.position);
            }
            else
            {
                agent.isStopped = true;
                avoidanceTimer -= Time.deltaTime;
            }
        }
    }

    private void FindNearestPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        float closestDistance = Mathf.Infinity;
        target = null;

        foreach (GameObject player in players)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            if (distance < closestDistance && distance <= radius && CanSeePlayer(player))
            {
                closestDistance = distance;
                target = player;
            }
        }
    }

    private bool CanSeePlayer(GameObject player)
    {
        RaycastHit hit;
        Vector3 direction = player.transform.position - transform.position;

        if (Physics.Raycast(transform.position, direction, out hit))
        {
            if (hit.collider.gameObject == player)
            {
                return true; // The enemy has line of sight to the player
            }
        }

        return false; // The enemy cannot see the player
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Vector3 avoidanceDirection = transform.position - collision.transform.position;
            avoidanceDirection.y = 0f;
            avoidanceDirection.Normalize();

            Vector3 targetPosition = transform.position + avoidanceDirection * avoidanceRadius;
            agent.SetDestination(targetPosition);

            avoidanceTimer = avoidanceTime;
            agent.speed = originalSpeed / 2f;
        }
    }
}