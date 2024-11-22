using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Transform track; // Parent object containing child waypoints
    public float moveSpeed = 5f;
    public bool isSlowed;
   
    private Transform[] waypoints;
    private int currentWaypointIndex = 0;

    void Start()
    {
        track = GameObject.FindWithTag("Track").transform;
        waypoints = new Transform[track.childCount];
        for (int i = 0; i < track.childCount; i++)
        {
            waypoints[i] = track.GetChild(i);
        }
    }

    void Update()
    {
        if (currentWaypointIndex < waypoints.Length)
        {
            MoveTowardsTarget();
        }
        else
        {
            // Enemy reached the end of the track
            // You can handle this as needed, like destroying the enemy
            Destroy(gameObject);
        }
       
    }

    void MoveTowardsTarget()
    {
        Vector3 targetPosition = waypoints[currentWaypointIndex].position;
        Vector3 direction = (targetPosition - transform.position).normalized;
        transform.Translate(direction * moveSpeed * Time.deltaTime);

        // Check if the enemy is close enough to the current waypoint
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            // Move to the next waypoint
            currentWaypointIndex++;
        }
    }
    // You can implement other methods like HitTarget and Die as needed
}