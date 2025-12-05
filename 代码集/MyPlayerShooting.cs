using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPlayerShooting : MonoBehaviour
{

    public float timeBetweenBullets = 0.15f;

    private float lastShotTime = 0f;
    private float effectTimer = 0f;

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
    private int shootableMask;
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
    void DisableEffects()
    {
        gunLight.enabled = false;
        gunLine.enabled = false;
    }

    void Update()
    {
        // 暂停时跳过
        if (Time.timeScale == 0f) return;

        time = time + Time.deltaTime;
        //获取用户开火键,按下fire1就返回true
        //if (Input.GetButton("Fire1")&& (Time.time - lastShotTime) >= timeBetweenBullets)
        //{
        //    //射击
        //    Shoot();
        //    lastShotTime = Time.time;
        //    effectTimer = 0f;

        //}

        //让特效超过时间失效
        //if(time >= timeBetweenBullets * effectDisplayTime)
        //{
        //    gunLight.enabled = false;
        //    gunLine.enabled = false;
        //}

        //发射射线，检测有没有命中
        //effectTimer += Time.deltaTime;
        //if (effectTimer >= effectDisplayTime)
        //{
        //    DisableEffects();
        //}
        // 获取用户开火键，按下 Fire1 就返回 true
        if (Input.GetButton("Fire1") && time >= timeBetweenBullets)
        {
            // 射击
            Shoot();
            time = 0f;
        }

        // 让特效超过时间失效
        if (time >= effectDisplayTime)
        {
            gunLight.enabled = false;
            gunLine.enabled = false;
        }
    }

    //void Shoot()
    //{
    //    //time = 0f;

    //    //gunLight.enabled = true;

    //    //gunLine.SetPosition(0,transform.position);
    //    ////gunLine.SetPosition(1,transform.position+transform.forward * 100);
    //    //gunLine.enabled = true;

    //    //gunParticle.Play();

    //    ////Debug. Log(DateTime.Now.ToString("HH:mm:ss:fff"));
    //    //gunAudio.Play();

    //    ////发射射线，检测是否命中

    //    //shootRay.origin = transform.position;
    //    //shootRay.direction = transform.forward;

    //    //if(Physics.Raycast(shootRay,out shootHit, 100, shootMask))
    //    //{
    //    //    gunLine.SetPosition(1, shootHit.point);

    //    //    MyEnemyHealth enemyHealth = shootHit.collider.GetComponent<MyEnemyHealth>();
    //    //    //enemyHealth.inithealth = enemyHealth.inithealth - damage;
    //    //    enemyHealth.TakeDamage(shootDamage, shootHit.point);
    //    //}
    //    //else
    //    //{
    //    //    gunLine.SetPosition(1, transform.position + transform.forward * 100);
    //    //}

    //    EnableEffects();
    //    shootRay.origin = transform.position;
    //    shootRay.direction = transform.forward;

    //    if (Physics.Raycast(shootRay, out shootHit, 100f, shootableMask))
    //    {
    //        gunLine.SetPosition(1, shootHit.point);
    //        MyEnemyHealth enemyHealth = shootHit.collider.GetComponent<MyEnemyHealth>();
    //        enemyHealth?.TakeDamage(shootDamage, shootHit.point);
    //    }
    //    else
    //    {
    //        gunLine.SetPosition(1, transform.position + transform.forward * 100);
    //    }
    //    void EnableEffects()
    //    {
    //        gunLight.enabled = true;
    //        gunLine.enabled = true;
    //        gunLine.SetPosition(0, transform.position);
    //        gunParticle.Play();
    //        gunAudio.pitch = UnityEngine.Random.Range(0.9f, 1.1f); // 随机音调
    //        gunAudio.Play();
    //    }



    //}
    void Shoot()
    {
        gunLight.enabled = true;
        gunLine.enabled = true;
        gunLine.SetPosition(0, transform.position);

        gunParticle.Play();
        gunAudio.Play();

        // 发射射线，检测是否命中
        shootRay.origin = transform.position;
        shootRay.direction = transform.forward;

        if (Physics.Raycast(shootRay, out shootHit, 100f, shootMask))
        {
            gunLine.SetPosition(1, shootHit.point);
            MyEnemyHealth enemyHealth = shootHit.collider.GetComponent<MyEnemyHealth>();
            if (enemyHealth != null)
            {
                // 新增：应用关卡倍数（从GameManager获取）
                int multiplier = FindObjectOfType<GameManager>().GetEnemyHealthMultiplier();
                enemyHealth.TakeDamage(shootDamage * multiplier, shootHit.point); // 错误：这里应该是玩家伤害，不乘倍数。修正为直接伤害
                enemyHealth.TakeDamage(shootDamage, shootHit.point);
            }
            else
            {
                Debug.LogWarning("射中对象没有 MyEnemyHealth 组件: " + shootHit.collider.name);
            }
        }
        else
        {
            gunLine.SetPosition(1, transform.position + transform.forward * 100);
        }
    }
    // 升级加攻击力
    public void AddDamage(int addAmount)
    {
        shootDamage += addAmount;
        Debug.Log($"攻击力升级为: {shootDamage}");
    }

}
