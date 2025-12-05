using CompleteProject;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyEnemyAttack : MonoBehaviour
{
    private GameObject player;
    private bool playerInRange = false;
    private float timer = 0;

    public int enemyAttack = 5;
    private GameManager gameManager;

    private MyPlayerHealth myPlayerHealth;
    private MyEnemyHealth myEnemyHealth;
    private Animator enemyAnim;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        myPlayerHealth = player.GetComponent<MyPlayerHealth>();
        enemyAnim = GetComponent<Animator>();
        gameManager = FindObjectOfType<GameManager>();
        myEnemyHealth = GetComponent<MyEnemyHealth>();

        // 验证组件
        if (player == null) Debug.LogError($"Player with tag 'Player' not found in scene on {gameObject.name}");
        if (myPlayerHealth == null) Debug.LogError($"MyPlayerHealth missing on {player?.name}");
        if (myEnemyHealth == null) Debug.LogError($"MyEnemyHealth missing on {gameObject.name}");
        if (enemyAnim == null) Debug.LogError($"Animator missing on {gameObject.name}");

        // 应用关卡攻击倍数
        if (gameManager != null)
        {
            int multiplier = gameManager.GetEnemyAttackMultiplier();
            enemyAttack *= multiplier;
            Debug.Log($"{gameObject.name} attack set to {enemyAttack} (multiplier: {multiplier})");
        }

    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (myEnemyHealth != null && !myEnemyHealth.IsDead &&
            myPlayerHealth != null && !myPlayerHealth.IsPlayerDead &&
            playerInRange && timer > 0.5f)
        {

            Attack();
        }
        else
        {
            Debug.Log($"{gameObject.name} cannot attack. Dead: {myEnemyHealth?.IsDead}, PlayerDead: {myPlayerHealth?.IsPlayerDead}, InRange: {playerInRange}, Timer: {timer}");
        }

        if (myPlayerHealth != null && myPlayerHealth.IsPlayerDead)
        {
            enemyAnim.SetTrigger("PlayerDead");
        }

    }

    private void Attack()
    {
        timer = 0;

        myPlayerHealth.TakeDamage(enemyAttack);
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            playerInRange = true;

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerInRange = false;

        }
    }


}
