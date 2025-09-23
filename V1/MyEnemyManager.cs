using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyEnemyManager : MonoBehaviour
{
    public GameObject Enemy;
    public GameObject CreateEnemyPoint;

    public float FirstCreateEnemyTime = 0f;
    private float currentCreateEnemyTime = 3f; // ��ǰ���ɼ��

    private void Start()
    {
        InvokeRepeating("Spawn", FirstCreateEnemyTime, currentCreateEnemyTime);
    }

    //���������ٶ�
    public void SetSpawnRate(float newRate)
    {
        currentCreateEnemyTime = newRate;
        CancelInvoke("Spawn"); // ֹͣ��ǰ
        InvokeRepeating("Spawn", 0f, currentCreateEnemyTime); // ���¿�ʼ
    }

    //�������ɣ�����ʱ���ã�
    public void ResetSpawn()
    {
        CancelInvoke("Spawn");
        //�������е�ǰ����
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
