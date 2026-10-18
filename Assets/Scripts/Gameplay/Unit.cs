using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour
{
    public string unitId;
    public int health = 100;
    public float speed = 5f;
    public float attackRange = 8f;
    public int damage = 10;

    private NavMeshAgent agent;
    private GameObject currentTarget;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent)
        {
            agent.speed = speed;
        }
    }

    void Update()
    {
        if (currentTarget)
        {
            float dist = Vector3.Distance(transform.position, currentTarget.transform.position);
            if (dist <= attackRange)
            {
                // attack logic (placeholder)
                // In a production game you'd handle attack rate, animations, and RPCs for network
            }
        }
    }

    public void MoveTo(Vector3 destination)
    {
        if (agent) agent.SetDestination(destination);
        else transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);
    }

    public void Attack(GameObject target)
    {
        currentTarget = target;
    }

    public void TakeDamage(int d)
    {
        health -= d;
        if (health <= 0) Die();
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
