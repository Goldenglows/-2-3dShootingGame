using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
[SerializeField] private GameObject pauseMenuPanel; // 暂停菜单面板
    [SerializeField] private Button exitButton; // 退出游戏按钮
    [SerializeField] private Button muteButton; // 静音按钮
    [SerializeField] private Text muteButtonText; // 静音按钮文本
    private bool isPaused = false; // 是否暂停
    private bool isMuted = false; // 是否静音
    public static event System.Action<bool> OnPauseStateChanged; // 暂停状态变化事件
    private MyPlayerHealth playerHealth; // 缓存玩家健康组件

    private void Awake()
    {
        // 验证 UI 组件
        if (pauseMenuPanel == null) Debug.LogError("PauseMenuPanel 未分配！");
        if (exitButton == null) Debug.LogError("ExitButton 未分配！");
        if (muteButton == null) Debug.LogError("MuteButton 未分配！");
        if (muteButtonText == null) Debug.LogError("MuteButtonText 未分配！");
        // 初始化隐藏菜单
        pauseMenuPanel.SetActive(false);

        // 加载静音状态
        isMuted = PlayerPrefs.GetInt("IsMuted", 0) == 1;
        AudioListener.volume = isMuted ? 0f : 1f;
        if (muteButtonText != null) muteButtonText.text = isMuted ? "取消静音" : "静音";

        // 绑定按钮事件
        if (exitButton != null) exitButton.onClick.AddListener(ExitGame);
        if (muteButton != null) muteButton.onClick.AddListener(ToggleMute);

        // 缓存玩家健康组件
        playerHealth = FindObjectOfType<MyPlayerHealth>();
        if (playerHealth == null) Debug.LogError("MyPlayerHealth 未找到，请检查玩家 GameObject 是否附加 MyPlayerHealth 组件");

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
                Debug.LogWarning("无法打开暂停菜单：玩家未找到或已死亡");
            }
        }
    }

    private void TogglePause()
    {
        isPaused = !isPaused;
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(isPaused);
        Time.timeScale = isPaused ? 0f : 1f; // 暂停/恢复时间
        AudioListener.pause = isPaused; // 暂停/恢复音频
        OnPauseStateChanged?.Invoke(isPaused); // 通知暂停状态
    }


    // 退出游戏
    private void ExitGame()
    {
        // 编辑器中停止播放，构建后退出应用
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
        if (muteButtonText != null) muteButtonText.text = isMuted ? "取消静音" : "静音";
        PlayerPrefs.SetInt("IsMuted", isMuted ? 1 : 0);
        PlayerPrefs.Save();
    }

    private void OnDestroy()
    {
        // 清理按钮事件
        if (exitButton != null) exitButton.onClick.RemoveAllListeners();
        if (muteButton != null) muteButton.onClick.RemoveAllListeners();
        // 恢复时间和音频
        Time.timeScale = 1f;
        AudioListener.pause = false;
    }


}
