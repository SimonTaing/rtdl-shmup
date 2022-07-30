using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviourTest : MonoBehaviour
{
    [SerializeField] float AttackCooldown;
    [SerializeField] float AttackCooldownTimer;
    [SerializeField] GameObject AttackObject;

    // Start is called before the first frame update
    void Start()
    {
        AttackCooldownTimer = AttackCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        AttackCooldownTimer -= Time.deltaTime;
        if(AttackCooldownTimer <= 0)
        {
            GameObject projectile;
            projectile = Instantiate(AttackObject, transform.position, transform.rotation);
            projectile.GetComponent<ProjectileBehaviour>().HSpeed = -0.5f;
            projectile.GetComponent<ProjectileBehaviour>().VSpeed = 0f;
            AttackCooldownTimer = AttackCooldown;
        }
    }
}
