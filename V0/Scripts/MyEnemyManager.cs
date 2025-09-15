using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyEnemyManager : MonoBehaviour
{
    public GameObject Enemy;
    public GameObject CreateEnemyPoint;

    public float FirstCreateEnemyTime = 0f;
    public float CreateEnemyTime = 3f;

   // private float creatEnemyTime = 0;

    private void Start()
    {
        //Spawn();
        InvokeRepeating("Spawn", FirstCreateEnemyTime, CreateEnemyTime);

    }

    // Update is called once per frame
    //void Update()
    //{
    //    creatEnemyTime+=Time.deltaTime;

    //    if(creatEnemyTime > 3f)
    //        Spawn();
    //}

    private void Spawn()
    {
        //creatEnemyTime = 0;
        Instantiate(Enemy, CreateEnemyPoint.transform.position,CreateEnemyPoint.transform.rotation) ;
    }

}
