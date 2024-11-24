using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Transform track; 
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
            
            Destroy(gameObject);
        }
       
    }

    void MoveTowardsTarget()
    {
        Vector3 targetPosition = waypoints[currentWaypointIndex].position;
        Vector3 direction = (targetPosition - transform.position).normalized;
        transform.Translate(direction * moveSpeed * Time.deltaTime);

        
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            
            currentWaypointIndex++;
        }
    }
   
}
