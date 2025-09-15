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

    private MyPlayerHealth myPlayerHealth;
    private Animator enemyAnim;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        myPlayerHealth = player.GetComponent<MyPlayerHealth>();
        enemyAnim = GetComponent<Animator>();

        // 验证组件是否为空
        if (myPlayerHealth == null) Debug.LogError("MyPlayerHealth missing on " + player.name);
        if (enemyAnim == null) Debug.LogError("Animator missing on " + gameObject.name);
        if (player == null)
        {
            Debug.LogError("Player with tag 'Player' not found in scene.");
            return;
        }

    }

    private void Update()
    {
        timer += Time.deltaTime;

        if(myPlayerHealth != null && !myPlayerHealth.IsPlayerDead && playerInRange && timer > 0.5f )
        {
            Attack();
        }

        if (myPlayerHealth != null && myPlayerHealth.IsPlayerDead) {
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
        if(other.gameObject.tag == "Player")
        {
            playerInRange = true;

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerInRange = false;

        }
    }


}
