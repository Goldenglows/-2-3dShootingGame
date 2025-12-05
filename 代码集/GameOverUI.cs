using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private Button restartButton; // 重新开始按钮
    private GameManager gameManager;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        if (gameManager == null)
        {
            Debug.LogError("GameManager 未找到！");
            return;
        }
        if (restartButton == null) Debug.LogError("RestartButton 未分配！");

        restartButton.onClick.RemoveAllListeners();
        restartButton.onClick.AddListener(() =>
        {
            Debug.Log("Restart Button clicked!");
            gameManager.RestartGame();
        });
    }

    private void OnEnable()
    {
        if (restartButton != null)
        {
            restartButton.interactable = true; // 确保按钮可交互
            //Debug.Log("GameOverPanel enabled, RestartButton interactable set to true");
        }
    }

    private void OnDestroy()
    {
        restartButton.onClick.RemoveAllListeners();
    }
}
