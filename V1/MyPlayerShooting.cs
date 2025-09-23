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

    //��ǹ��ر���
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

        // ��֤����Ƿ�Ϊ��
        if (gunAudio == null) Debug.LogError("AudioSource missing on " + gameObject.name);
        if (gunLight == null) Debug.LogError("Light missing on " + gameObject.name);
        if (gunParticle == null) Debug.LogError("ParticleSystem missing on " + gameObject.name);
        if (gunLine == null) Debug.LogError("LineRenderer missing on " + gameObject.name);

        // ��֤ Enemy ���Ƿ����
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
        // ��ͣʱ����
        if (Time.timeScale == 0f) return;

        time = time + Time.deltaTime;
        //��ȡ�û������,����fire1�ͷ���true
        //if (Input.GetButton("Fire1")&& (Time.time - lastShotTime) >= timeBetweenBullets)
        //{
        //    //���
        //    Shoot();
        //    lastShotTime = Time.time;
        //    effectTimer = 0f;

        //}

        //����Ч����ʱ��ʧЧ
        //if(time >= timeBetweenBullets * effectDisplayTime)
        //{
        //    gunLight.enabled = false;
        //    gunLine.enabled = false;
        //}

        //�������ߣ������û������
        //effectTimer += Time.deltaTime;
        //if (effectTimer >= effectDisplayTime)
        //{
        //    DisableEffects();
        //}
        // ��ȡ�û������������ Fire1 �ͷ��� true
        if (Input.GetButton("Fire1") && time >= timeBetweenBullets)
        {
            // ���
            Shoot();
            time = 0f;
        }

        // ����Ч����ʱ��ʧЧ
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

    //    ////�������ߣ�����Ƿ�����

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
    //        gunAudio.pitch = UnityEngine.Random.Range(0.9f, 1.1f); // �������
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

        // �������ߣ�����Ƿ�����
        shootRay.origin = transform.position;
        shootRay.direction = transform.forward;

        if (Physics.Raycast(shootRay, out shootHit, 100f, shootMask))
        {
            gunLine.SetPosition(1, shootHit.point);
            MyEnemyHealth enemyHealth = shootHit.collider.GetComponent<MyEnemyHealth>();
            if (enemyHealth != null)
            {
                // ������Ӧ�ùؿ���������GameManager��ȡ��
                int multiplier = FindObjectOfType<GameManager>().GetEnemyHealthMultiplier();
                enemyHealth.TakeDamage(shootDamage * multiplier, shootHit.point); // ��������Ӧ��������˺������˱���������Ϊֱ���˺�
                enemyHealth.TakeDamage(shootDamage, shootHit.point);
            }
            else
            {
                Debug.LogWarning("���ж���û�� MyEnemyHealth ���: " + shootHit.collider.name);
            }
        }
        else
        {
            gunLine.SetPosition(1, transform.position + transform.forward * 100);
        }
    }
    // �����ӹ�����
    public void AddDamage(int addAmount)
    {
        shootDamage += addAmount;
        Debug.Log($"����������Ϊ: {shootDamage}");
    }

}
