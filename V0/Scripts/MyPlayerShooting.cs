using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPlayerShooting : MonoBehaviour
{

    public float timeBetweenBullets = 0.15f;

    private float time = 0f;
    private float effectDisplayTime = 0.2f;
    private AudioSource gunAudio;
    private Light gunLight;
    private LineRenderer gunLine;
    private ParticleSystem gunParticle;

    //开枪相关变量
    private Ray shootRay;
    private RaycastHit shootHit;
    private int shootMask;
    public int shootDamage = 50;


    private void Awake()
    {
        gunAudio = GetComponent<AudioSource>();
        gunLight = GetComponent<Light>();
        gunParticle = GetComponent<ParticleSystem>();
        gunLine = GetComponent<LineRenderer>();

        // 验证组件是否为空
        if (gunAudio == null) Debug.LogError("AudioSource missing on " + gameObject.name);
        if (gunLight == null) Debug.LogError("Light missing on " + gameObject.name);
        if (gunParticle == null) Debug.LogError("ParticleSystem missing on " + gameObject.name);
        if (gunLine == null) Debug.LogError("LineRenderer missing on " + gameObject.name);

        // 验证 Enemy 层是否存在
        shootMask = LayerMask.GetMask("Enemy");
        if (shootMask == 0) Debug.LogError("Layer 'Enemy' not found. Please ensure the layer exists.");

    }

    void Update()
    {
        time = time + Time.deltaTime;
        //获取用户开火键,按下fire1就返回true
        if (Input.GetButton("Fire1")&& time>= timeBetweenBullets)
        {
            //射击
            Shoot();
        }

        //让特效超过时间失效
        if(time >= timeBetweenBullets * effectDisplayTime)
        {
            gunLight.enabled = false;
            gunLine.enabled = false;
        }

        //发射射线，检测有没有命中


    }

    void Shoot()
    {
        time = 0f;

        gunLight.enabled = true;

        gunLine.SetPosition(0,transform.position);
        //gunLine.SetPosition(1,transform.position+transform.forward * 100);
        gunLine.enabled = true;

        gunParticle.Play();

        //Debug. Log(DateTime.Now.ToString("HH:mm:ss:fff"));
        gunAudio.Play();

        //发射射线，检测是否命中

        shootRay.origin = transform.position;
        shootRay.direction = transform.forward;

        if(Physics.Raycast(shootRay,out shootHit, 100, shootMask))
        {
            gunLine.SetPosition(1, shootHit.point);

            MyEnemyHealth enemyHealth = shootHit.collider.GetComponent<MyEnemyHealth>();
            //enemyHealth.inithealth = enemyHealth.inithealth - damage;
            enemyHealth.TakeDamage(shootDamage, shootHit.point);
        }
        else
        {
            gunLine.SetPosition(1, transform.position + transform.forward * 100);
        }


    }
        

}
