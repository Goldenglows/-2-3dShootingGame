using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class MyEnemyHealth : MonoBehaviour
{
    public AudioClip DeathClip;

    public int inithealth = 100;
    private int currentHealth;
    public bool IsDead = false;
    private bool IsSinking = false;

    public enum EnemyType { Pink, Blue, Yellow }
    public EnemyType enemyType;

    private AudioSource enemyAudio;
    private ParticleSystem enemyParticle;
    private Animator enemyDeathAnimator;
    private CapsuleCollider enemyCapsuleCollider;
    private Rigidbody myRigidbody;
    private NavMeshAgent navMeshAgent;
    private GameManager gameManager;


    private void Awake()
    {
        enemyAudio = GetComponent<AudioSource>();
        enemyParticle = GetComponentInChildren <ParticleSystem>();
        enemyDeathAnimator = GetComponent<Animator>();
        enemyCapsuleCollider = GetComponentInChildren <CapsuleCollider>();
        myRigidbody = GetComponent<Rigidbody>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        gameManager = FindObjectOfType<GameManager>();

        if (enemyAudio == null) Debug.LogError("AudioSource missing on " + gameObject.name);
        if (enemyParticle == null) Debug.LogError("ParticleSystem missing on " + gameObject.name);
        if (enemyDeathAnimator == null) Debug.LogError("Animator missing on " + gameObject.name);
        if (enemyCapsuleCollider == null) Debug.LogError("CapsuleCollider missing on " + gameObject.name);
        if (GetComponent<Rigidbody>() == null) Debug.LogError("Rigidbody missing on " + gameObject.name);
        if (navMeshAgent == null) Debug.LogError("NavMeshAgent missing on " + gameObject.name);

        if (gameManager != null)
        {
            int multiplier = gameManager.GetEnemyHealthMultiplier();
            currentHealth = inithealth * multiplier;
        }
        else
        {
            currentHealth = inithealth;
        }

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
        //如果已经死亡
        if (IsDead == true)
            return;

        //播放受伤声音
        enemyAudio.Play();

        //粒子效果
        enemyParticle.transform.position = hitPoint;
        enemyParticle.Play();

        currentHealth -= damage; // 使用当前血量
        //死亡
        if (currentHealth <= 0)
        {
            Death();
        }
    }

    private void Death()
    {
        IsDead = true;

        //动画触发死亡
        enemyDeathAnimator.SetTrigger("Death");
        enemyCapsuleCollider.isTrigger = true;

        //死亡音效
        enemyAudio.clip = DeathClip;
        enemyAudio.Play();

        //计数
        // 根据怪物类型更新计数
        switch (enemyType)
        {
            case EnemyType.Pink:
                KillCounter.KillScoresPink += 1;
                break;
            case EnemyType.Blue:
                KillCounter.KillScoresBlue += 1;
                break;
            case EnemyType.Yellow:
                KillCounter.KillScoresYellow += 1;
                break;
        }
        KillCounter.Instance.SaveKills(); //保存击杀数

        // 禁用自动寻路
        if (navMeshAgent != null)
            navMeshAgent.enabled = false;

        // 禁用物理模拟
        if (myRigidbody != null)
            myRigidbody.isKinematic = true;

        // 触发沉入（假设通过动画事件调用StartSinking，或直接调用）
        StartCoroutine(SinkAfterDelay(1f)); //延迟1秒沉入
    }

    //延迟沉入协程
    private IEnumerator SinkAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        StartSinking();
    }

    public void StartSinking()
    {
        IsSinking = true;

        //几秒后销毁
        Destroy(gameObject, 2f);

    }

}
