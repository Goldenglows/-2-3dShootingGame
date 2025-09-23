using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MyPlayerHealth : MonoBehaviour
{
    //玩家血量
    public int playerStartHealth = 100;
    private int currentHealth;
    //玩家是否死亡
    public bool IsPlayerDead = false;
    private AudioSource playerAudio;
    private Animator playerAnim;
    public AudioClip PlayerDeathClip;
    //玩家ui
    public Text PlayerHealthUI;
    //玩家受伤
    public Image PlayerHurtUI;
    public Color FlashHurtColor = new Color(1f,0f,0f,1);
    private bool damaged = false;

    private PlayerMoving playerMovement;
    private MyPlayerShooting myPlayerShooting;
    public event System.Action OnPlayerDeath;



    private void Awake()
    {
        playerAudio = GetComponent<AudioSource>();
        playerAnim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMoving>();
        myPlayerShooting = GetComponentInChildren<MyPlayerShooting>();
        // 验证组件是否为空
        if (playerAudio == null) Debug.LogError("AudioSource missing on " + gameObject.name);
        if (playerAnim == null) Debug.LogError("Animator missing on " + gameObject.name);
        //if (playerMovement == null) Debug.LogError("PlayerMovement missing on " + gameObject.name);
        if (myPlayerShooting == null) Debug.LogError("MyPlayerShooting missing on " + gameObject.name);

        currentHealth = PlayerPrefs.GetInt("PlayerHealth", playerStartHealth);
        playerStartHealth = currentHealth;
        PlayerHealthUI.text = playerStartHealth.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        // 暂停时跳过
        if (Time.timeScale == 0f) return;
        if (damaged)
        {
            PlayerHurtUI.color = FlashHurtColor;
        }
        else
        {
            PlayerHurtUI.color = Color.Lerp(PlayerHurtUI.color, Color.clear, 2f*Time.deltaTime);
        }

        damaged = false;

    }

    public void TakeDamage(int damage)
    {
        if (IsPlayerDead)
        {
            Debug.Log("玩家已死亡，无法施加伤害");
            return;
        }
        //Debug.Log("玩家受到 " + damage + " 点伤害。当前血量：" + playerStartHealth);
        damaged = true;
        //播放受伤的声音
        playerAudio.Play();

        playerStartHealth -= damage;
        //更新玩家血量ui
        PlayerHealthUI.text = playerStartHealth.ToString();

        if (playerStartHealth <= 0)
            Death();


    }

    void Death()
    {
        IsPlayerDead = true;

        // 死亡音效
        if (playerAudio != null)
        {
            playerAudio.clip = PlayerDeathClip;
            playerAudio.Play();
        }

        // 死亡动效
        if (playerAnim != null)
            playerAnim.SetTrigger("Die");

        // 禁止移动 禁止射击
        if (playerMovement != null)
            playerMovement.enabled = false;
        if (myPlayerShooting != null)
            myPlayerShooting.enabled = false;

        OnPlayerDeath?.Invoke();
    }

    public void AddMaxHealth(int addAmount)
    {
        playerStartHealth += addAmount;
        PlayerHealthUI.text = playerStartHealth.ToString();
        // 保存继承血量
        PlayerPrefs.SetInt("PlayerHealth", playerStartHealth);
        PlayerPrefs.Save();
    }

    public void RestartLevel()
    {
        //SceneManager.LoadScene(0);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
