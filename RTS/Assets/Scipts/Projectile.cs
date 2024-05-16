using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 20f;
    public float damage = 10f;
    public float lifetime = 5f;

    private Transform target;

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    private void Start()
    {
        // Уничтожить пулю через некоторое время, если она не достигла цели
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        if (target == null) return;
        
        var direction = (target.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;
        
        if (Vector3.Distance(transform.position, target.position) < 0.5f)
        {
            HitTarget();
        }
    }

    private void HitTarget()
    {
        var targetHealth = target.GetComponent<Health>();
        
        if (targetHealth != null)
        {
            targetHealth.TakeDamage(damage);
        }
        
        Destroy(gameObject);
    }
}
