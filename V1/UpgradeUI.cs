using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeUI : MonoBehaviour
{
    [SerializeField] private Button attackButton; // �ӹ�����ť
    [SerializeField] private Button healthButton; // ��������ť
    private GameManager gameManager;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        if (gameManager == null)
        {
            Debug.LogError("GameManager δ�ҵ���");
            return;
        }
        if (attackButton == null) Debug.LogError("AttackButton δ���䣡");
        if (healthButton == null) Debug.LogError("HealthButton δ���䣡");

        attackButton.onClick.RemoveAllListeners();
        attackButton.onClick.AddListener(() =>
        {
            Debug.Log("Attack Button clicked!");
            gameManager.UpgradeAttack();
        });

        healthButton.onClick.RemoveAllListeners();
        healthButton.onClick.AddListener(() =>
        {
            Debug.Log("Health Button clicked!");
            gameManager.UpgradeHealth();
        });
    }

    private void OnEnable()
    {
        if (attackButton != null) attackButton.interactable = true;
        if (healthButton != null) healthButton.interactable = true;
        Debug.Log($"[{gameObject.name}] UpgradePanel enabled, buttons interactable.");
    }

    private void OnDestroy()
    {
        if (attackButton != null) attackButton.onClick.RemoveAllListeners();
        if (healthButton != null) healthButton.onClick.RemoveAllListeners();
    }

}
