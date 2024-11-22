using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    private Transform target;
    public float speed = 10f;
    public int damage = 10;

    public void Seek(Transform target)
    {
        this.target = target;
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject); // If the target is lost, destroy the projectile
            return;
        }

        Vector2 direction = target.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = rotation;

        float distanceThisFrame = speed * Time.deltaTime;
        transform.Translate(direction.normalized * distanceThisFrame, Space.World);

        // Check if the projectile is close enough to the target
        if (direction.magnitude <= distanceThisFrame)
        {
            HitTarget();
        }
    }

    void HitTarget()
    {
        // Do damage to the target or whatever action you want when the projectile hits
        Destroy(gameObject);
        EnemyHealth enemyHealth = target.GetComponent<EnemyHealth>();
        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(damage);
        }
    }
}
