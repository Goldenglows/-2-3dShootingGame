using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private Button restartButton; // ���¿�ʼ��ť
    private GameManager gameManager;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        if (gameManager == null)
        {
            Debug.LogError("GameManager δ�ҵ���");
            return;
        }
        if (restartButton == null) Debug.LogError("RestartButton δ���䣡");

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
            restartButton.interactable = true; // ȷ����ť�ɽ���
            //Debug.Log("GameOverPanel enabled, RestartButton interactable set to true");
        }
    }

    private void OnDestroy()
    {
        restartButton.onClick.RemoveAllListeners();
    }
}
