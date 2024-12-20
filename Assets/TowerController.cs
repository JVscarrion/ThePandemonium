using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TowerController : MonoBehaviour
{
    public enum TowerType { Archer, Stone, Fire, Ice }

    public TowerType towerType;
    public GameObject projectilePrefab;
    public Transform firePoint;
    public GameObject fireSfx;
    public float attackRange = 5f;
    public float attackRate = 2f; 
    public int damage;
    public float circleColour;

    private float nextAttackTime = 0f;
    private GameObject target;
    private GameObject rangeCircle;
    private GameObject targetEnemy;

   
    [System.Serializable]
    public struct FireVariables
    {
        public int fireDamage;
        public float damageOverTimeInterval;
        public bool isDamgeStarted;
    }
    public FireVariables fireVariables;

   
    [System.Serializable]
    public struct IceVariables
    {
        public float iceSlowRate;
        public bool isSlowed;
    }
    public IceVariables iceVariables;

    [System.Serializable]
    public struct StoneVariables
    {
        public int stoneDamage;
        public float damageOverTimeInterval;
        public bool isDamgeStarted;
    }
    public StoneVariables stoneVariables;

    public void Start()
    {
        rangeCircle = transform.GetChild(3).gameObject;
        RangeCircle();
    }
    void Update()
    {
        if (Time.time >= nextAttackTime)
        {
            if (targetEnemy == null || !IsEnemyWithinRange(targetEnemy)) 
            {
                targetEnemy = GetNearestEnemy();
            }
            if (targetEnemy != null)
            {
                target = targetEnemy;
                Shoot(targetEnemy);
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
        Ability();
    }

    void Shoot(GameObject target)
    {
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        GameObject sfxSpawned = Instantiate(fireSfx, firePoint.position, Quaternion.identity);
        Destroy(sfxSpawned, 5f);
        ProjectileController projectileController = projectile.GetComponent<ProjectileController>();
        if (projectileController != null)
        {
            projectileController.Seek(target.transform);
            projectileController.damage = damage;
        }
    }
    bool IsEnemyWithinRange(GameObject enemy)
    {
        if (enemy == null)
        {
            return false;
        }

        float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
        return distanceToEnemy <= attackRange;
    }
    GameObject GetNearestEnemy()
    {
        GameObject nearestEnemy = null;
        float shortestDistance = Mathf.Infinity;
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector2.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance && distanceToEnemy <= attackRange)
            {
                nearestEnemy = enemy;
                shortestDistance = distanceToEnemy;
            }
        }
        return nearestEnemy;
    }

    public void Ability()
    {
        if (towerType == TowerType.Ice && target != null)
        {
            Ice();
        }
        else if (towerType == TowerType.Fire && target != null)
        {
            Fire();
        }
        else if (towerType == TowerType.Stone && target != null)
        {
            Stone();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    public void Ice()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, attackRange);
        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject.CompareTag("Enemy"))
            {
                EnemyController enemyController = collider.GetComponent<EnemyController>();
                if (enemyController != null)
                {
                    bool wasInsideRange = enemyController.isSlowed; // Check if the enemy was previously inside the range
                    bool isInsideRange = Vector2.Distance(transform.position, collider.transform.position) <= attackRange;

                    if (isInsideRange)
                    {
                        if (!enemyController.isSlowed)
                        {
                            
                            enemyController.moveSpeed -= iceVariables.iceSlowRate;
                            enemyController.isSlowed = true;
                            enemyController.gameObject.transform.GetChild(0).gameObject.GetComponent<Animator>().Play("FRun");
                            Debug.Log("Slowed: " + enemyController.moveSpeed);
                        }
                    }
                    else if (wasInsideRange)
                    {
                        
                        enemyController.moveSpeed += iceVariables.iceSlowRate;
                        enemyController.isSlowed = false; 
                        enemyController.gameObject.transform.GetChild(0).gameObject.GetComponent<Animator>().Play("Run");
                        Debug.Log("Speed increased: " + enemyController.moveSpeed);
                    }
                }
            }
        }
    }

    public void Fire()
    {
        if (!fireVariables.isDamgeStarted)
        {
            StartCoroutine(ApplyFireDamageOverTime());
            fireVariables.isDamgeStarted = true;
        }
    }
    public void Stone()
    {
        if (!stoneVariables.isDamgeStarted)
        {
            StartCoroutine(ApplyStoneDamageOverTime());
            stoneVariables.isDamgeStarted = true;
        }
    }
    IEnumerator ApplyFireDamageOverTime()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, attackRange);
        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject.CompareTag("Enemy"))
            {
                EnemyHealth enemyHealth = collider.GetComponent<EnemyHealth>();
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(fireVariables.fireDamage);
                    enemyHealth.gameObject.transform.GetChild(0).gameObject.GetComponent<Animator>().Play("BRun");
                    Debug.Log("Damaged: " + enemyHealth.currentHealth + "Fire Tower");
                }
            }
        }

        yield return new WaitForSeconds(fireVariables.damageOverTimeInterval);

        if (towerType == TowerType.Fire)
        {
            StartCoroutine(ApplyFireDamageOverTime());
        }
    }
    IEnumerator ApplyStoneDamageOverTime()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, attackRange);
        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject.CompareTag("Enemy"))
            {
                EnemyHealth enemyHealth = collider.GetComponent<EnemyHealth>();
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(stoneVariables.stoneDamage);
                    enemyHealth.gameObject.transform.GetChild(0).gameObject.GetComponent<Animator>().Play("SRun");
                    Debug.Log("Damaged: " + enemyHealth.currentHealth);
                }
            }
        }

        yield return new WaitForSeconds(stoneVariables.damageOverTimeInterval);

        if (towerType == TowerType.Stone)
        {
            StartCoroutine(ApplyStoneDamageOverTime());
        }
    }
    public void RangeCircle()
    {
        if(rangeCircle != null)
        {
            rangeCircle.transform.localScale = new Vector2(attackRange + attackRange, attackRange + attackRange);
            SpriteRenderer spriteRenderer = rangeCircle.GetComponent<SpriteRenderer>();
            Color spriteColor = spriteRenderer.color;

            spriteColor.a = circleColour;

            spriteRenderer.color = spriteColor;
        }
        else
        {
            rangeCircle = transform.GetChild(3).gameObject;
            return;
        }
    }
}
