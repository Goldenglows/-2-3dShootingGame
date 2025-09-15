using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class MyEnemyHealth : MonoBehaviour
{
    public AudioClip DeathClip;

    public int inithealth = 100;
    public bool IsDead = false;
    private bool IsSinking = false;

    private AudioSource enemyAudio;
    private ParticleSystem enemyParticle;
    private Animator enemyDeathAnimator;
    private CapsuleCollider enemyCapsuleCollider;
    private Rigidbody myRigidbody;
    private NavMeshAgent navMeshAgent;


    private void Awake()
    {
        enemyAudio = GetComponent<AudioSource>();
        enemyParticle = GetComponentInChildren <ParticleSystem>();
        enemyDeathAnimator = GetComponent<Animator>();
        enemyCapsuleCollider = GetComponentInChildren <CapsuleCollider>();
        myRigidbody = GetComponent<Rigidbody>();
        navMeshAgent = GetComponent<NavMeshAgent>();

        if (enemyAudio == null) Debug.LogError("AudioSource missing on " + gameObject.name);
        if (enemyParticle == null) Debug.LogError("ParticleSystem missing on " + gameObject.name);
        if (enemyDeathAnimator == null) Debug.LogError("Animator missing on " + gameObject.name);
        if (enemyCapsuleCollider == null) Debug.LogError("CapsuleCollider missing on " + gameObject.name);
        if (GetComponent<Rigidbody>() == null) Debug.LogError("Rigidbody missing on " + gameObject.name);
        if (navMeshAgent == null) Debug.LogError("NavMeshAgent missing on " + gameObject.name);

    }

    private void Update()
    {
        if(IsSinking == true)
        {
            //transform.Translate(-transform.up * 2.5f * Time.deltaTime);
            transform.Translate(Vector3.down * 2.5f * Time.deltaTime);
        }
    }

    public void TakeDamage(int damage,Vector3 hitPoint)
    {
        //��������ʲô������
        if (IsDead == true)
            return;

        //�����ܻ�����
        enemyAudio.Play();

        //������Ч
        enemyParticle.transform.position = hitPoint;
        enemyParticle.Play();

        inithealth -= damage;
        //����
        if (inithealth <= 0)
        {
            Death();
        }

    }

    private void Death()
    {

        IsDead = true;

        //������������
        enemyDeathAnimator.SetTrigger("Death");
        enemyCapsuleCollider.isTrigger = true;

        //����������Ч
        enemyAudio.clip = DeathClip;
        enemyAudio.Play();

        //计数
        MyPlayerScores.Scores += 1;

        //�����Զ�Ѱ·
        //GetComponent<NavMeshAgent>().enabled = false;
        //��С����
        //GetComponent<Rigidbody>().isKinematic = true;
        // 禁用自动寻路
        if (navMeshAgent != null)
            navMeshAgent.enabled = false;

        // 禁用物理模拟
        if (myRigidbody != null)
            myRigidbody.isKinematic = true;

    }

    public void StartSinking()
    {
        IsSinking = true;

        //���ٵ���
        Destroy(gameObject,2f);

    }

}
