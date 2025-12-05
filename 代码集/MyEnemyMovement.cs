//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.AI;

//public class MyEnemyMovement : MonoBehaviour
//{
//    private GameObject player;
//    private NavMeshAgent nav;

//    private MyEnemyHealth myEnemyHealth;
//    private MyPlayerHealth myPlayerHealth;
//    //定义独立的基础速度
//    [SerializeField] private float pinkBaseSpeed = 6f;
//    [SerializeField] private float blueBaseSpeed = 3f;
//    [SerializeField] private float yellowBaseSpeed = 2f;

//    private void Awake()
//    {
//        player = GameObject.FindGameObjectWithTag("Player");
//        nav = GetComponent<NavMeshAgent>();
//        myEnemyHealth = GetComponent<MyEnemyHealth>();
//        myPlayerHealth = player.GetComponent<MyPlayerHealth>();
//    }

//    // Update is called once per frame
//    void Update()
//    {

//        if (!myEnemyHealth.IsDead && !myPlayerHealth.IsPlayerDead)
//        {
//            nav.SetDestination(player.transform.position);
//        }
//        else
//        {
//            nav.enabled = false;
//        }
//    }
//}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MyEnemyMovement : MonoBehaviour
{
    private GameObject player;
    private NavMeshAgent nav;
    private MyEnemyHealth myEnemyHealth;
    private MyPlayerHealth myPlayerHealth;

    // 为每种敌人类型定义独立的基础速度
    [SerializeField] private float pinkBaseSpeed = 6f;
    [SerializeField] private float blueBaseSpeed = 3f;
    [SerializeField] private float yellowBaseSpeed = 2f;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        nav = GetComponent<NavMeshAgent>();
        myEnemyHealth = GetComponent<MyEnemyHealth>();
        myPlayerHealth = player.GetComponent<MyPlayerHealth>();

        // 根据敌人类型设置基础速度
        float baseSpeed = GetBaseSpeedByType();
        // 从 GameManager 获取速度倍数并应用
        GameManager gameManager = FindObjectOfType<GameManager>();
        if (gameManager != null)
        {
            nav.speed = baseSpeed * gameManager.GetEnemySpeedMultiplier();
            Debug.Log($"{gameObject.name} speed set to {nav.speed} (Base: {baseSpeed}, Multiplier: {gameManager.GetEnemySpeedMultiplier()})");
        }
        else
        {
            nav.speed = baseSpeed;
            Debug.LogWarning("GameManager not found, using base speed: " + baseSpeed);
        }
    }

    private float GetBaseSpeedByType()
    {
        if (myEnemyHealth == null)
        {
            Debug.LogWarning("MyEnemyHealth not found, using default speed: " + pinkBaseSpeed);
            return pinkBaseSpeed; // 备用默认值
        }

        switch (myEnemyHealth.enemyType)
        {
            case MyEnemyHealth.EnemyType.Pink:
                return pinkBaseSpeed;
            case MyEnemyHealth.EnemyType.Blue:
                return blueBaseSpeed;
            case MyEnemyHealth.EnemyType.Yellow:
                return yellowBaseSpeed;
            default:
                Debug.LogWarning("Unknown enemy type, using default speed: " + pinkBaseSpeed);
                return pinkBaseSpeed;
        }
    }

    private void Update()
    {
        if (!myEnemyHealth.IsDead && !myPlayerHealth.IsPlayerDead)
        {
            nav.SetDestination(player.transform.position);
        }
        else
        {
            nav.enabled = false;
        }
    }
}