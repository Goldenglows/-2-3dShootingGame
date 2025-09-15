using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MyPlayerHealth : MonoBehaviour
{
    //���Ѫ��
    public int playerStartHealth = 100;
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

    private PlayerMovement playerMovement;
    private MyPlayerShooting myPlayerShooting;
    



    private void Awake()
    {
        playerAudio = GetComponent<AudioSource>();
        playerAnim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        myPlayerShooting = GetComponentInChildren<MyPlayerShooting>();
        // ��֤����Ƿ�Ϊ��
        if (playerAudio == null) Debug.LogError("AudioSource missing on " + gameObject.name);
        if (playerAnim == null) Debug.LogError("Animator missing on " + gameObject.name);
        //if (playerMovement == null) Debug.LogError("PlayerMovement missing on " + gameObject.name);
        if (myPlayerShooting == null) Debug.LogError("MyPlayerShooting missing on " + gameObject.name);
    
    }

    // Update is called once per frame
    void Update()
    {
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

        //������Ч
        playerAudio.clip = PlayerDeathClip;
        playerAudio.Play();

        //������Ч
        playerAnim.SetTrigger("Die");

        //��ֹ�ƶ� ��ֹ���
        playerMovement.enabled = false;
        myPlayerShooting.enabled = false;

    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(0);
    }

}
