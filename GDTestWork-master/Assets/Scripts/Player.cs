using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    static public Player instance;

    [Header("Player Settings")]
    public float Hp;
    public float Damage;
    public float SuperDamage;
    public float AtackSpeed;
    public float AttackRange = 2;
    public float superAttackReloadTime;

    [Space]

    [Header("Links")]
    [SerializeField] private Slider heathSlider;
    [SerializeField] private GameObject superDamageBlocker;

    //Links
    [SerializeField] private Button attackBtn;
    [SerializeField] private Button superAttackBtn;

    public Animator AnimatorController;

    //Checks
    [HideInInspector] public bool blockScript = false;
    [HideInInspector] public bool timerEnd = true;
    [HideInInspector] public bool startAttack = false;

    private bool canDamdge = false;
    private bool canSuperDamdge;
    private bool inRange = false;

    private void Awake()
    {
        instance = this;

        Timer.instance.maTime = superAttackReloadTime;
    }

    private void Update()
    {
        heathSlider.value = Hp;

        if (blockScript)
        {
            return;
        }

        if (Hp <= 0)
        {
            Die();
            return;
        }

        if (SceneManager.Instance.winGame)
        {
            Win();
            return;
        }

        AttackLogic();
    }

    //The "AttackStart" method is called when the player presses the button. In the string variable you need to specify the name of the attack animation that will occur
    public void AttackStart(string animationName)
    {
        if (!startAttack && !blockScript)
        {
            startAttack = true;
            AnimatorController.SetTrigger(animationName);

            attackBtn.interactable = false;
        }
    }

    //The “AttackLogic” method stores the main logic based on the methods, position and actions made by the player
    private void AttackLogic()
    {
        var enemies = SceneManager.Instance.Enemies;
        Enemie closestEnemie = null;

        for (int i = 0; i < enemies.Count; i++)
        {
            var enemie = enemies[i];
            if (enemie == null)
            {
                continue;
            }

            if (closestEnemie == null)
            {
                closestEnemie = enemie;
                continue;
            }

            var distance = Vector3.Distance(transform.position, enemie.transform.position);
            var closestDistance = Vector3.Distance(transform.position, closestEnemie.transform.position);

            if (distance < closestDistance)
            {
                closestEnemie = enemie;
            }

        }

        if (closestEnemie != null)
        {
            var distance = Vector3.Distance(transform.position, closestEnemie.transform.position);
            if (distance <= AttackRange)
            {
                inRange = true;
                
                if (startAttack)
                    transform.transform.rotation = Quaternion.LookRotation(closestEnemie.transform.position - transform.position);

                if (canDamdge)
                {
                    closestEnemie.Hp -= Damage;
                    canDamdge = false;
                }
                else if (canSuperDamdge)
                {
                    closestEnemie.Hp -= SuperDamage;
                    canSuperDamdge = false;
                }

            }else {
                inRange = false;
            }
        }


        if (!startAttack && timerEnd && inRange)
        {
            superAttackBtn.interactable = true;
        }else{
            superAttackBtn.interactable = false;
        }
    }

    //The "CanDamage" and "CanSuperDamage()" methods are called in the animation allowing damage to be dealt
    private void CanDamage()
    {
        if (inRange)
        {
            canDamdge = true;
        }
    }

    private void CanSuperDamage()
    {
        if (inRange)
        {
            canSuperDamdge = true;
        }
    }

    //The "AttackEnd" method is called in the animation ending the attack logic
    private void AttackEnd()
    {
        startAttack = false;

        attackBtn.interactable = true;
    }

    //The "SuperBlocker" method is called in the animation, starting to block a super attack for a while
    private void SuperBlocker()
    {
        //Activation of an object that contains the logic of blocking a super attack
        superDamageBlocker.SetActive(true);
        //Activation of an timer
        timerEnd = false;
    }

    private void Die()
    {
        blockScript = true;
        AnimatorController.SetTrigger("Die");

        SceneManager.Instance.GameOver();
    }

    private void Win()
    {
        blockScript = true;
        AnimatorController.SetTrigger("Win");
    }


}
