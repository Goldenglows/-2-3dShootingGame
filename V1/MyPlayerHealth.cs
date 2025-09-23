using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MyPlayerHealth : MonoBehaviour
{
    //���Ѫ��
    public int playerStartHealth = 100;
    private int currentHealth;
    //����Ƿ�����
    public bool IsPlayerDead = false;
    private AudioSource playerAudio;
    private Animator playerAnim;
    public AudioClip PlayerDeathClip;
    //���ui
    public Text PlayerHealthUI;
    //�������
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
        // ��֤����Ƿ�Ϊ��
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
        // ��ͣʱ����
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
            Debug.Log("������������޷�ʩ���˺�");
            return;
        }
        //Debug.Log("����ܵ� " + damage + " ���˺�����ǰѪ����" + playerStartHealth);
        damaged = true;
        //�������˵�����
        playerAudio.Play();

        playerStartHealth -= damage;
        //�������Ѫ��ui
        PlayerHealthUI.text = playerStartHealth.ToString();

        if (playerStartHealth <= 0)
            Death();


    }

    void Death()
    {
        IsPlayerDead = true;

        // ������Ч
        if (playerAudio != null)
        {
            playerAudio.clip = PlayerDeathClip;
            playerAudio.Play();
        }

        // ������Ч
        if (playerAnim != null)
            playerAnim.SetTrigger("Die");

        // ��ֹ�ƶ� ��ֹ���
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
        // ����̳�Ѫ��
        PlayerPrefs.SetInt("PlayerHealth", playerStartHealth);
        PlayerPrefs.Save();
    }

    public void RestartLevel()
    {
        //SceneManager.LoadScene(0);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
