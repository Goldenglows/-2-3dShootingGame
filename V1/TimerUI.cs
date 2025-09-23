using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerUI : MonoBehaviour
{
    [SerializeField] private Text timerText;  // ����ʱText���
    private GameManager gameManager;

    private void Awake()
    {
        timerText = GetComponent<Text>();
        gameManager = FindObjectOfType<GameManager>();
        if (timerText == null) Debug.LogError("TimerText δ���䣡");
        if (gameManager == null) Debug.LogError("GameManager δ�ҵ���");
    }

    private void Update()
    {
        if (gameManager != null)
        {
            float remainingTime = gameManager.levelDuration - (gameManager.levelDuration - gameManager.GetLevelTimer()); // ����GameManager���GetLevelTimer�������Ժ��޸�
            int minutes = Mathf.FloorToInt(remainingTime / 60);
            int seconds = Mathf.FloorToInt(remainingTime % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }
}