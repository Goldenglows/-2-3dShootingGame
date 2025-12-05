using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyEnemyManager : MonoBehaviour
{
    public GameObject Enemy;
    public GameObject CreateEnemyPoint;

    public float FirstCreateEnemyTime = 0f;
    private float currentCreateEnemyTime = 3f; // 当前生成间隔

    private void Start()
    {
        InvokeRepeating("Spawn", FirstCreateEnemyTime, currentCreateEnemyTime);
    }

    //设置生成速度
    public void SetSpawnRate(float newRate)
    {
        currentCreateEnemyTime = newRate;
        CancelInvoke("Spawn"); // 停止当前
        InvokeRepeating("Spawn", 0f, currentCreateEnemyTime); // 重新开始
    }

    //重置生成（进关时调用）
    public void ResetSpawn()
    {
        CancelInvoke("Spawn");
        //销毁所有当前敌人
        MyEnemyHealth[] enemies = FindObjectsOfType<MyEnemyHealth>();
        foreach (var enemy in enemies)
        {
            if (!enemy.IsDead) Destroy(enemy.gameObject);
        }
    }

    private void Spawn()
    {
        //creatEnemyTime = 0;
        Instantiate(Enemy, CreateEnemyPoint.transform.position, CreateEnemyPoint.transform.rotation);
    }
}
