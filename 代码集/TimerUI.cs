using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerUI : MonoBehaviour
{
    [SerializeField] private Text timerText;  // 倒计时Text组件
    private GameManager gameManager;

    private void Awake()
    {
        timerText = GetComponent<Text>();
        gameManager = FindObjectOfType<GameManager>();
        if (timerText == null) Debug.LogError("TimerText 未分配！");
        if (gameManager == null) Debug.LogError("GameManager 未找到！");
    }

    private void Update()
    {
        if (gameManager != null)
        {
            float remainingTime = gameManager.levelDuration - (gameManager.levelDuration - gameManager.GetLevelTimer()); // 假设GameManager添加GetLevelTimer方法，稍后修改
            int minutes = Mathf.FloorToInt(remainingTime / 60);
            int seconds = Mathf.FloorToInt(remainingTime % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }
}