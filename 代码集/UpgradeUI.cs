using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeUI : MonoBehaviour
{
    [SerializeField] private Button attackButton; // 加攻击按钮
    [SerializeField] private Button healthButton; // 加生命按钮
    private GameManager gameManager;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        if (gameManager == null)
        {
            Debug.LogError("GameManager 未找到！");
            return;
        }
        if (attackButton == null) Debug.LogError("AttackButton 未分配！");
        if (healthButton == null) Debug.LogError("HealthButton 未分配！");

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
