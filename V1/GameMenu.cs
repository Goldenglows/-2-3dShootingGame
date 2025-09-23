using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
[SerializeField] private GameObject pauseMenuPanel; // ��ͣ�˵����
    [SerializeField] private Button exitButton; // �˳���Ϸ��ť
    [SerializeField] private Button muteButton; // ������ť
    [SerializeField] private Text muteButtonText; // ������ť�ı�
    private bool isPaused = false; // �Ƿ���ͣ
    private bool isMuted = false; // �Ƿ���
    public static event System.Action<bool> OnPauseStateChanged; // ��ͣ״̬�仯�¼�
    private MyPlayerHealth playerHealth; // ������ҽ������

    private void Awake()
    {
        // ��֤ UI ���
        if (pauseMenuPanel == null) Debug.LogError("PauseMenuPanel δ���䣡");
        if (exitButton == null) Debug.LogError("ExitButton δ���䣡");
        if (muteButton == null) Debug.LogError("MuteButton δ���䣡");
        if (muteButtonText == null) Debug.LogError("MuteButtonText δ���䣡");
        // ��ʼ�����ز˵�
        pauseMenuPanel.SetActive(false);

        // ���ؾ���״̬
        isMuted = PlayerPrefs.GetInt("IsMuted", 0) == 1;
        AudioListener.volume = isMuted ? 0f : 1f;
        if (muteButtonText != null) muteButtonText.text = isMuted ? "ȡ������" : "����";

        // �󶨰�ť�¼�
        if (exitButton != null) exitButton.onClick.AddListener(ExitGame);
        if (muteButton != null) muteButton.onClick.AddListener(ToggleMute);

        // ������ҽ������
        playerHealth = FindObjectOfType<MyPlayerHealth>();
        if (playerHealth == null) Debug.LogError("MyPlayerHealth δ�ҵ���������� GameObject �Ƿ񸽼� MyPlayerHealth ���");

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (playerHealth != null && !playerHealth.IsPlayerDead)
            {
                TogglePause();
            }
            else
            {
                Debug.LogWarning("�޷�����ͣ�˵������δ�ҵ���������");
            }
        }
    }

    private void TogglePause()
    {
        isPaused = !isPaused;
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(isPaused);
        Time.timeScale = isPaused ? 0f : 1f; // ��ͣ/�ָ�ʱ��
        AudioListener.pause = isPaused; // ��ͣ/�ָ���Ƶ
        OnPauseStateChanged?.Invoke(isPaused); // ֪ͨ��ͣ״̬
    }


    // �˳���Ϸ
    private void ExitGame()
    {
        // �༭����ֹͣ���ţ��������˳�Ӧ��
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void ToggleMute()
    {
        isMuted = !isMuted;
        AudioListener.volume = isMuted ? 0f : 1f;
        if (muteButtonText != null) muteButtonText.text = isMuted ? "ȡ������" : "����";
        PlayerPrefs.SetInt("IsMuted", isMuted ? 1 : 0);
        PlayerPrefs.Save();
    }

    private void OnDestroy()
    {
        // ����ť�¼�
        if (exitButton != null) exitButton.onClick.RemoveAllListeners();
        if (muteButton != null) muteButton.onClick.RemoveAllListeners();
        // �ָ�ʱ�����Ƶ
        Time.timeScale = 1f;
        AudioListener.pause = false;
    }


}
