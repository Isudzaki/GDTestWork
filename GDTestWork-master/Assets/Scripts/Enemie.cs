using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemie : MonoBehaviour
{
    [Header("Enemie Settings")]
    public float Hp;
    public float Damage;
    public float AtackSpeed;
    public float AttackRange = 2;
    public float AgentNormalSpeed = 3.5f;

    public int hpForKill;

    //Components
    private Animator animatorController;
    private NavMeshAgent agent;
    private CapsuleCollider coliider;

    //System
    private float lastAttackTime = 0;
    private bool isDead = false;
    private bool enemyCanDamage = false;
    private bool inRange = false;

    private void Awake()
    {

        coliider = GetComponent<CapsuleCollider>();
        animatorController = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }
    private void Start()
    {
        SceneManager.Instance.AddEnemie(this);
        agent.SetDestination(SceneManager.Instance.Player.transform.position);
    }

    private void Update()
    {
        EnemyAILogic();
    }

    private void EnemyAILogic()
    {
        if (isDead || Player.instance.blockScript)
        {
            agent.isStopped = true;
            agent.speed = 0;
            return;
        }

        if (Hp <= 0)
        {
            Die();
            agent.isStopped = true;
            return;
        }

        var distance = Vector3.Distance(transform.position, SceneManager.Instance.Player.transform.position);

        if (distance <= AttackRange)
        {
            agent.isStopped = true;
            agent.speed = 0;
            inRange = true;

            if (Time.time - lastAttackTime > AtackSpeed)
            {
                transform.transform.rotation = Quaternion.LookRotation(Player.instance.transform.position - transform.position);

                animatorController.SetTrigger("Attack");

                lastAttackTime = Time.time;
            }
            if (enemyCanDamage)
            {
                Player.instance.Hp -= Damage;
                enemyCanDamage = false;
            }
        }
        else
        {
            agent.speed = AgentNormalSpeed;
            inRange = false;
            agent.isStopped = false;
            agent.SetDestination(SceneManager.Instance.Player.transform.position);
        }

        animatorController.SetFloat("Speed", agent.speed);
    }

    //The "EnemyCanDamage" method is called in the animation allowing damage to be dealt
    private void EnemyCanDamage()
    {
        if (inRange)
        {
            enemyCanDamage = true;
        }
    }

    public virtual void Die()
    {
        isDead = true;
        coliider.enabled = false;

        Player.instance.Hp += hpForKill;
        SceneManager.Instance.RemoveEnemie(this);

        animatorController.SetTrigger("Die");

        Debug.Log("Enem");
    }

}
